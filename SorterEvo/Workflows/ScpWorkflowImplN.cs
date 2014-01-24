//using System;
//using System.Linq;
//using Genomic.Genomes;
//using Genomic.Layers;
//using MathUtils.Rand;
//using SorterEvo.Evals;
//using SorterEvo.Genomes;
//using SorterEvo.Layers;
//using Sorting.CompetePools;

//namespace SorterEvo.Workflows
//{
//    class ScpWorkflowImplN : IScpWorkflow
//    {
//        public ScpWorkflowImplN(
//            ILayer<ISorterGenome> sorterLayer,
//            IScpParams scpParams,
//            int generation
//            )
//        {
//            _sorterLayer = sorterLayer;
//            _compWorkflowState = CompWorkflowState.RunCompetition;
//            _scpParams = scpParams;
//            _generation = generation;
//            _sorterLayerEval = Enumerable.Empty<ISorterGenomeEval>()
//                                         .Make<ISorterGenome, IGenomeEval<ISorterGenome>>();
//        }

//        private ScpWorkflowImplN
//            (
//                CompWorkflowState compWorkflowState,
//                ILayer<ISorterGenome> sorterLayer,
//                ICompPool compPool,
//                ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> sorterLayerEval,
//                IScpParams scpParams,
//                int generation
//            )
//        {
//            _compWorkflowState = compWorkflowState;
//            _sorterLayer = sorterLayer;
//            _sorterLayerEval = sorterLayerEval;
//            _scpParams = scpParams;
//            _generation = generation;
//            _compPool = compPool;
//        }

//        private readonly ICompPool _compPool;
//        public ICompPool CompPool
//        {
//            get { return _compPool; }
//        }


//        private readonly int _generation;
//        public int Generation
//        {
//            get { return _generation; }
//        }

//        private readonly IScpParams _scpParams;
//        public IScpParams ScpParams
//        {
//            get { return _scpParams; }
//        }

//        private readonly ILayer<ISorterGenome> _sorterLayer;
//        public ILayer<ISorterGenome> SorterLayer
//        {
//            get { return _sorterLayer; }
//        }

//        private readonly ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> _sorterLayerEval;
//        public ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> SorterLayerEval
//        {
//            get { return _sorterLayerEval; }
//        }

//        private readonly CompWorkflowState _compWorkflowState;
//        public CompWorkflowState CompWorkflowState
//        {
//            get { return _compWorkflowState; }
//        }

//        public IScpWorkflow Step(int seed)
//        {
//            IScpWorkflow scpWorkflow = null;
//            switch (CompWorkflowState)
//            {
//                case CompWorkflowState.ReproGenomes:
//                    scpWorkflow = ReproStep(seed);
//                    break;
//                case CompWorkflowState.RunCompetition:
//                    scpWorkflow = RunCompetitionStep(seed);
//                    break;
//                case CompWorkflowState.EvaluateResults:
//                    scpWorkflow = EvaluateResultsStep(seed);
//                    break;
//                case CompWorkflowState.UpdateGenomes:
//                    scpWorkflow = UpdateGenomesStep(seed);
//                    break;
//                default:
//                    throw new Exception(String.Format("CompWorkflowState {0} not handled in SorterCompParaPoolWorkflowImpl.Step", CompWorkflowState));
//            }

//            return scpWorkflow;
//        }

//        private IScpWorkflow ReproStep(int seed)
//        {
//            var randy = Rando.Fast(seed);

//            var sorterLayer = SorterLayer.Reproduce
//                (
//                    seed: randy.NextInt(),
//                    newGenomeCount: ScpParams.ChildCount,
//                    mutationRate: ScpParams.SorterMutationRate,
//                    insertionRate: ScpParams.SorterMutationRate,
//                    deletionRate: ScpParams.SorterDeletionRate
//                );

//            return new ScpWorkflowImplN
//                (
//                    compWorkflowState: CompWorkflowState.RunCompetition,
//                    sorterLayer: sorterLayer,
//                    compPool: null,
//                    sorterLayerEval: SorterLayerEval,
//                    scpParams: ScpParams,
//                    generation: Generation
//                );

//        }

//        private IScpWorkflow RunCompetitionStep(int seed)
//        {
//            var newGenomes = SorterLayer.Genomes
//                .Where(g => SorterLayerEval.GetGenomeEval(g.Guid) == null);

//            return new ScpWorkflowImplN
//                (
//                    compWorkflowState: CompWorkflowState.EvaluateResults,
//                    sorterLayer: SorterLayer,
//                    compPool: newGenomes.Select(t => t.ToSorter()).ToCompPoolParallel(),
//                    sorterLayerEval: SorterLayerEval,
//                    scpParams: ScpParams,
//                    generation: Generation
//                );
//        }

//        private IScpWorkflow EvaluateResultsStep(int seed)
//        {
//            var sorterLayerEval =
//                CompPool.SorterEvals.Select(
//                    t => GenomeEval.Make(
//                            genome: SorterLayer.GetGenome(t.Sorter.Guid),
//                            score: t.SwitchUseCount,
//                            generation: Generation
//                        )
//                    ).Concat(SorterLayerEval.GenomeEvals)
//                     .Make<ISorterGenome, IGenomeEval<ISorterGenome>>();

//            return new ScpWorkflowImplN
//                (
//                    compWorkflowState: CompWorkflowState.UpdateGenomes,
//                    sorterLayer: SorterLayer,
//                    compPool: CompPool,
//                    sorterLayerEval: sorterLayerEval,
//                    scpParams: ScpParams,
//                    generation: Generation
//                );
//        }

//        private IScpWorkflow UpdateGenomesStep(int seed)
//        {
//            var newGenomes = SorterLayerEval.GenomeEvals
//                                            .OrderBy(e => e.Score)
//                                            .Select(e => e.Genome)
//                                            .Take(ScpParams.PopulationCount)
//                                            .ToList();

//            return new ScpWorkflowImplN
//                (
//                    compWorkflowState: CompWorkflowState.ReproGenomes,
//                    sorterLayer: Layer.Make(
//                            generation: Generation + 1,
//                            genomes: newGenomes
//                        ),
//                        compPool: CompPool,
//                        sorterLayerEval: newGenomes.Select(g => SorterLayerEval.GetGenomeEval(g.Guid))
//                                                   .Make<ISorterGenome, IGenomeEval<ISorterGenome>>(),
//                        scpParams: ScpParams,
//                        generation: Generation + 1
//                );
//        }
//    }
//}
