using System;
using System.Linq;
using Genomic.Genomes;
using Genomic.Layers;
using MathUtils.Rand;
using SorterEvo.Genomes;
using SorterEvo.Layers;
using Sorting.CompetePools;
using Sorting.Switchables;

namespace SorterEvo.Workflows
{
    public interface ISorterCompParaPoolWorkflow
    {
        ICompParaPool CompParaPool { get; }
        int Generation { get; }
        SorterCompParaPoolParams SorterCompParaPoolParams { get; }
        ILayer<ISorterGenome> SorterLayer { get; }
        CompWorkflowState CompWorkflowState { get; }
        ISorterCompParaPoolWorkflow Step(int seed);
        ILayer<ISwitchableGroupGenome> SwitchableGroupLayer { get; }
        ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> SorterLayerEval { get; }
        ILayerEval<ISwitchableGroupGenome, IGenomeEval<ISwitchableGroupGenome>> SwitchableGroupLayerEval { get; }
    }

    public static class SorterCompParaPoolWorkflow
    {
        public static ISorterCompParaPoolWorkflow Make(
                int seed,
                SwitchableGroupGenomeType switchableGroupGenomeType,
                int keyCount,
                int keyPairCount,
                int switchableGroupSize,
                SorterCompParaPoolParams sorterCompParaPoolParams
            )
        {
            var randy = Rando.Fast(seed);
            return new SorterCompParaPoolWorkflowImpl
                (
                    sorterLayer: SorterLayer.Create
                                (
                                      seed: randy.NextInt(),
                                      genomeCount: sorterCompParaPoolParams.PopulationSize,
                                      keyCount: keyCount,
                                      keyPairCount: keyPairCount
                                ),
                    switchableGroupLayer: SwitchableLayer.Create
                                (
                                      seed: randy.NextInt(),
                                      switchableGroupGenomeType: switchableGroupGenomeType,
                                      genomeCount: sorterCompParaPoolParams.SwitchableLayerStartingGenomeCount,
                                      keyCount: keyCount,
                                      groupSize: switchableGroupSize
                                ),
                    sorterCompParaPoolParams: sorterCompParaPoolParams, 
                    generation: 0
                );
        }

        public static ISorterCompParaPoolWorkflow Make(
                ILayer<ISorterGenome> sorterLayer,
                ILayer<ISwitchableGroupGenome> switchableGroupLayer,
                SorterCompParaPoolParams sorterCompParaPoolParams,
                int generation
            )
        {
            return new SorterCompParaPoolWorkflowImpl
                (
                    sorterLayer: sorterLayer,
                    switchableGroupLayer: switchableGroupLayer,
                    sorterCompParaPoolParams: sorterCompParaPoolParams,
                    generation: generation
                );
        }
    }

    class SorterCompParaPoolWorkflowImpl : ISorterCompParaPoolWorkflow
    {
        public SorterCompParaPoolWorkflowImpl
            (
                ILayer<ISorterGenome> sorterLayer,
                ILayer<ISwitchableGroupGenome> switchableGroupLayer,
                SorterCompParaPoolParams sorterCompParaPoolParams, 
                int generation
            )
        {
            _compWorkflowState = CompWorkflowState.ReproGenomes;
            _sorterLayer = sorterLayer;
            _switchableGroupLayer = switchableGroupLayer;
            _compParaPool = null;
            _sorterCompParaPoolParams = sorterCompParaPoolParams;
            _generation = generation;
            _sorterLayerEval = null;
            _switchableGroupLayerEval = null;
        }

        private SorterCompParaPoolWorkflowImpl
            (
                CompWorkflowState compWorkflowState,
                ILayer<ISorterGenome> sorterLayer,
                ILayer<ISwitchableGroupGenome> switchableGroupLayer,
                ICompParaPool compParaPool,
                ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> sorterLayerEval,
                ILayerEval<ISwitchableGroupGenome, IGenomeEval<ISwitchableGroupGenome>> switchableGroupLayerEval, 
                SorterCompParaPoolParams sorterCompParaPoolParams, 
                int generation
            )
        {
            _compWorkflowState = compWorkflowState;
            _sorterLayer = sorterLayer;
            _switchableGroupLayer = switchableGroupLayer;
            _sorterLayerEval = sorterLayerEval;
            _switchableGroupLayerEval = switchableGroupLayerEval;
            _sorterCompParaPoolParams = sorterCompParaPoolParams;
            _generation = generation;
            _compParaPool = compParaPool;
        }

        private readonly ICompParaPool _compParaPool;
        public ICompParaPool CompParaPool
        {
            get { return _compParaPool; }
        }

        private readonly int _generation;
        public int Generation
        {
            get { return _generation; }
        }

        private readonly SorterCompParaPoolParams _sorterCompParaPoolParams;
        public SorterCompParaPoolParams SorterCompParaPoolParams
        {
            get { return _sorterCompParaPoolParams; }
        }

        private readonly ILayer<ISorterGenome> _sorterLayer;
        public ILayer<ISorterGenome> SorterLayer
        {
            get { return _sorterLayer; }
        }

        private readonly CompWorkflowState _compWorkflowState;
        public CompWorkflowState CompWorkflowState
        {
            get { return _compWorkflowState; }
        }

        private readonly ILayer<ISwitchableGroupGenome> _switchableGroupLayer;
        public ILayer<ISwitchableGroupGenome> SwitchableGroupLayer
        {
            get { return _switchableGroupLayer; }
        }

        private readonly ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> _sorterLayerEval;
        public ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> SorterLayerEval
        {
            get { return _sorterLayerEval; }
        }

        private readonly ILayerEval<ISwitchableGroupGenome, IGenomeEval<ISwitchableGroupGenome>> _switchableGroupLayerEval;

        public ILayerEval<ISwitchableGroupGenome, IGenomeEval<ISwitchableGroupGenome>> SwitchableGroupLayerEval
        {
            get { return _switchableGroupLayerEval; }
        }

        public ISorterCompParaPoolWorkflow Step(int seed)
        {
            ISorterCompParaPoolWorkflow sorterCompParaPoolWorkflow;
            switch (CompWorkflowState)
            {
                case CompWorkflowState.ReproGenomes:
                    sorterCompParaPoolWorkflow = ReproStep(seed);
                    break;
                case CompWorkflowState.RunCompetition:
                    sorterCompParaPoolWorkflow = RunCompetitionStep(seed);
                    break;
                case CompWorkflowState.EvaluateResults:
                    sorterCompParaPoolWorkflow = EvaluateResultsStep(seed);
                    break;
                case CompWorkflowState.UpdateGenomes:
                    sorterCompParaPoolWorkflow = UpdateGenomesStep(seed);
                    break;
                default:
                    throw new Exception(String.Format("CompWorkflowState {0} not handled in SorterCompParaPoolWorkflowImpl.Step", CompWorkflowState));
            }

            return sorterCompParaPoolWorkflow;
        }

        private ISorterCompParaPoolWorkflow ReproStep(int seed)
        {
            var randy = Rando.Fast(seed);

            var sorterLayer = SorterLayer.ReproducePreserveParents(seed: randy.NextInt(), newGenomeCount: SorterCompParaPoolParams.ChildCount, mutationRate: SorterCompParaPoolParams.SorterMutationRate, insertionRate: SorterCompParaPoolParams.SorterMutationRate, deletionRate: SorterCompParaPoolParams.SorterDeletionRate);

            var switchableGroupLayer = SwitchableGroupLayer.Reproduce(seed: randy.NextInt(), newGenomeCount: SorterCompParaPoolParams.SwitchableLayerExpandedGenomeCount, mutationRate: SorterCompParaPoolParams.SwitchableLayerExpandedGenomeCount, insertionRate: SorterCompParaPoolParams.SwitchableGroupInsertionRate, deletionRate: SorterCompParaPoolParams.SwitchableGroupDeletionRate);

            return new SorterCompParaPoolWorkflowImpl
                (
                    compWorkflowState: CompWorkflowState.RunCompetition,
                    sorterLayer: sorterLayer,
                    switchableGroupLayer: switchableGroupLayer,
                    compParaPool: null,
                    sorterLayerEval: null,
                    switchableGroupLayerEval: null,
                    sorterCompParaPoolParams: SorterCompParaPoolParams,
                    generation: Generation
                );
            
        }

        private ISorterCompParaPoolWorkflow RunCompetitionStep(int seed)
        {
            var sorters = SorterLayer.Genomes.Select(t=>t.ToSorter());
            ICompParaPool compParaPool;

            switch (SwitchableGroupLayer.Genomes[0].SwitchableGroupGenomeType)
            {
                case SwitchableGroupGenomeType.UInt:
                    var switchableGroupsUint = SwitchableGroupLayer.Genomes.Select(t => (ISwitchableGroup<uint>)t.ToSwitchableGroup());
                    compParaPool = sorters.ToCompParaPoolParallel(switchableGroupsUint);
                break;
                case SwitchableGroupGenomeType.ULong:
                    var switchableGroupsULong = SwitchableGroupLayer.Genomes.Select(t => (ISwitchableGroup<ulong>)t.ToSwitchableGroup());
                    compParaPool = sorters.ToCompParaPoolParallel(switchableGroupsULong);
                break;
                case SwitchableGroupGenomeType.BitArray:
                    var switchableGroupsBitArray = SwitchableGroupLayer.Genomes.Select(t => (ISwitchableGroup<bool[]>)t.ToSwitchableGroup());
                    compParaPool = sorters.ToCompParaPoolParallel(switchableGroupsBitArray);
                break;
                case SwitchableGroupGenomeType.IntArray:
                    var switchableGroupsIntArray = SwitchableGroupLayer.Genomes.Select(t => (ISwitchableGroup<int[]>)t.ToSwitchableGroup());
                    compParaPool = sorters.ToCompParaPoolParallel(switchableGroupsIntArray);
                break;
                default:
                throw new Exception(String.Format("{0} is not handled in RunCompetitionStep", SwitchableGroupLayer.Genomes[0].SwitchableGroupGenomeType));
            }

            return new SorterCompParaPoolWorkflowImpl
                (
                    compWorkflowState: CompWorkflowState.EvaluateResults,
                    sorterLayer: SorterLayer,
                    switchableGroupLayer: SwitchableGroupLayer,
                    compParaPool: compParaPool,
                    sorterLayerEval: null,
                    switchableGroupLayerEval: null,
                    sorterCompParaPoolParams: SorterCompParaPoolParams,
                    generation: Generation
                );
        }

        private ISorterCompParaPoolWorkflow EvaluateResultsStep(int seed)
        {
            var sorterLayerEval =
                CompParaPool.SorterOnSwitchableGroupSets.Select(
                    t => GenomeEval.Make(
                            genome: SorterLayer.GetGenome(t.Sorter.Guid), 
                            score:  t.SwitchesUsed,
                            generation: Generation,
                            success: t.SorterOnSwitchableGroups.All(sg=>sg.Success)
                        )
                    ).Make<ISorterGenome, IGenomeEval<ISorterGenome>>();


            var switchableGroupLayerEval = 
                CompParaPool.SorterOnSwitchableGroups.GroupBy(t => t.SwitchableGroupGuid).Select(
                    g => GenomeEval.Make(
                            genome: SwitchableGroupLayer.GetGenome(g.Key),
                            score: g.Sum(s=> - s.SwitchUseCount),
                            generation: Generation,
                            success: g.All(m=>m.Success)
                        )
                    ).Make<ISwitchableGroupGenome, IGenomeEval<ISwitchableGroupGenome>>();

            return new SorterCompParaPoolWorkflowImpl
                (
                    compWorkflowState: CompWorkflowState.UpdateGenomes,
                    sorterLayer: SorterLayer,
                    switchableGroupLayer: SwitchableGroupLayer,
                    compParaPool: CompParaPool,
                    sorterLayerEval: sorterLayerEval,
                    switchableGroupLayerEval: switchableGroupLayerEval,
                    sorterCompParaPoolParams: SorterCompParaPoolParams,
                    generation: Generation
                );
        }

        private ISorterCompParaPoolWorkflow UpdateGenomesStep(int seed)
        {
            var rando = Rando.Fast(seed);
            return new SorterCompParaPoolWorkflowImpl
                (
                    compWorkflowState: CompWorkflowState.ReproGenomes,
                    sorterLayer: Layer.Make(
                        generation: Generation + 1,
                        genomes: SorterLayerEval.GenomeEvals
                                                .SubSortShuffle(t => t.Score, rando.NextInt())
                                                .Select(e => e.Genome)
                                                .Take(SorterCompParaPoolParams.PopulationSize)
                        ),
                    switchableGroupLayer: Layer.Make(
                        generation: Generation + 1,
                        genomes: SwitchableGroupLayerEval.GenomeEvals
                                                .SubSortShuffle(t => t.Score, rando.NextInt())
                                                .Select(e=>e.Genome)
                                                .Take(SorterCompParaPoolParams.SwitchableLayerStartingGenomeCount)
                        ),
                    compParaPool: null,
                    sorterLayerEval: null,
                    switchableGroupLayerEval: null,
                    sorterCompParaPoolParams: SorterCompParaPoolParams,
                    generation: Generation + 1
                );
         }

    }
}