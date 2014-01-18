using System;
using System.Linq;
using Genomic.Genomes;
using Genomic.Layers;
using MathUtils.Rand;
using SorterEvo.Evals;
using SorterEvo.Genomes;
using SorterEvo.Layers;
using Sorting.CompetePools;

namespace SorterEvo.Workflows
{
    public interface IScpWorkflow
    {
        ICompPool CompPool { get; }
        int Generation { get; }
        IScpParams ScpParams { get; }
        CompWorkflowState CompWorkflowState { get; }
        IScpWorkflow Step(int seed);
        ILayer<ISorterGenome> SorterLayer { get; }
        ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> SorterLayerEval { get; }
    }

    public static class ScpWorkflow
    {
        public static IScpWorkflow Make(
            int seed,
            int keyCount,
            int keyPairCount,
            IScpParams scpParams
        )
        {
            var randy = Rando.Fast(seed);
            return new ScpWorkflowImpl
                (
                    sorterLayer: SorterLayer.Create
                                (
                                      seed: randy.NextInt(),
                                      genomeCount: scpParams.SorterLayerStartingGenomeCount,
                                      keyCount: keyCount,
                                      keyPairCount: keyPairCount
                                ),
                    scpParams: scpParams,
                    generation: 0
                );
        }

        public static IScpWorkflow Make(
            ILayer<ISorterGenome> sorterLayer,
            IScpParams scpParams,
            int generation
        )
        {
            return new ScpWorkflowImpl
                (
                    sorterLayer: sorterLayer,
                    scpParams: scpParams,
                    generation: generation
                );
        }
    }

    class ScpWorkflowImpl : IScpWorkflow
    {
        public ScpWorkflowImpl(
            ILayer<ISorterGenome> sorterLayer,
            IScpParams scpParams, 
            int generation
            )
        {
            _sorterLayer = sorterLayer;
            _compWorkflowState = CompWorkflowState.ReproGenomes;
            _scpParams = scpParams;
            _generation = generation;
        }

        private ScpWorkflowImpl
            (
                CompWorkflowState compWorkflowState,
                ILayer<ISorterGenome> sorterLayer,
                ICompPool compPool,
                ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> sorterLayerEval,
                IScpParams scpParams, 
                int generation
            )
        {
            _compWorkflowState = compWorkflowState;
            _sorterLayer = sorterLayer;
            _sorterLayerEval = sorterLayerEval;
            _scpParams = scpParams;
            _generation = generation;
            _compPool = compPool;
        }

        private readonly ICompPool _compPool;
        public ICompPool CompPool
        {
            get { return _compPool; }
        }


        private readonly int _generation;
        public int Generation
        {
            get { return _generation; }
        }

        private readonly IScpParams _scpParams;
        public IScpParams ScpParams
        {
            get { return _scpParams; }
        }

        private readonly ILayer<ISorterGenome> _sorterLayer;
        public ILayer<ISorterGenome> SorterLayer
        {
            get { return _sorterLayer; }
        }

        private readonly ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> _sorterLayerEval;
        public ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> SorterLayerEval
        {
            get { return _sorterLayerEval; }
        }

        private readonly CompWorkflowState _compWorkflowState;
        public CompWorkflowState CompWorkflowState
        {
            get { return _compWorkflowState; }
        }

        public IScpWorkflow Step(int seed)
        {
            IScpWorkflow scpWorkflow = null;
            switch (CompWorkflowState)
            {
                case CompWorkflowState.ReproGenomes:
                    scpWorkflow = ReproStep(seed);
                    break;
                case CompWorkflowState.RunCompetition:
                    scpWorkflow = RunCompetitionStep(seed);
                    break;
                case CompWorkflowState.EvaluateResults:
                    scpWorkflow = EvaluateResultsStep(seed);
                    break;
                case CompWorkflowState.UpdateGenomes:
                    scpWorkflow = UpdateGenomesStep(seed);
                    break;
                default:
                    throw new Exception(String.Format("CompWorkflowState {0} not handled in SorterCompParaPoolWorkflowImpl.Step", CompWorkflowState));
            }

            return scpWorkflow;
        }

        private IScpWorkflow ReproStep(int seed)
        {
            var randy = Rando.Fast(seed);

            var sorterLayer = SorterLayer.Reproduce
                (
                    seed: randy.NextInt(), 
                    newGenomeCount: ScpParams.SorterLayerExpandedGenomeCount,
                    mutationRate: ScpParams.SorterMutationRate,
                    insertionRate: ScpParams.SorterMutationRate, 
                    deletionRate: ScpParams.SorterDeletionRate
                );

            return new ScpWorkflowImpl
                (
                    compWorkflowState: CompWorkflowState.RunCompetition,
                    sorterLayer: sorterLayer,
                    compPool: null,
                    sorterLayerEval: null,
                    scpParams: ScpParams,
                    generation: Generation
                );

        }

        private IScpWorkflow RunCompetitionStep(int seed)
        {
            return new ScpWorkflowImpl
                (
                    compWorkflowState: CompWorkflowState.EvaluateResults,
                    sorterLayer: SorterLayer,
                    compPool: SorterLayer.Genomes.Select(t => t.ToSorter()).ToCompPoolParallel(),
                    sorterLayerEval: null,
                    scpParams: ScpParams,
                    generation: Generation
                );
        }

        private IScpWorkflow EvaluateResultsStep(int seed)
        {
            var sorterLayerEval =
                CompPool.SorterEvals.Select(
                    t => GenomeEval.Make(
                            genome: SorterLayer.GetGenome(t.Sorter.Guid),
                            score: t.SwitchUseCount,
                            generation: Generation
                        )
                    ).Make<ISorterGenome, IGenomeEval<ISorterGenome>>();

            return new ScpWorkflowImpl
                (
                    compWorkflowState: CompWorkflowState.UpdateGenomes,
                    sorterLayer: SorterLayer,
                    compPool: CompPool,
                    sorterLayerEval: sorterLayerEval,
                    scpParams: ScpParams,
                    generation: Generation
                );
        }

        private IScpWorkflow UpdateGenomesStep(int seed)
        {
            var rando = Rando.Fast(seed);
            return new ScpWorkflowImpl
                (
                    compWorkflowState: CompWorkflowState.ReproGenomes,
                    sorterLayer: Layer.Make(
                        generation: Generation + 1,
                        genomes: SorterLayerEval.GenomeEvals
                                                .SubSortShuffle(t => t.Score, rando.NextInt())
                                                .Select(e => e.Genome)
                                                .Take(ScpParams.SorterLayerStartingGenomeCount)

                                                //.Take(
                                                //    (Generation % 4 == 0) ?
                                                //        ScpParams.SorterLayerStartingGenomeCount/4
                                                //    :
                                                //        ScpParams.SorterLayerStartingGenomeCount
                                                // )
                        ),
                        compPool: null,
                        sorterLayerEval: null,
                        scpParams: ScpParams,
                        generation: Generation + 1
                );
        }
    }
}
