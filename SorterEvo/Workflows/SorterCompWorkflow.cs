using System;
using System.Linq;
using Genomic.Genomes;
using Genomic.Layers;
using MathUtils.Rand;
using SorterEvo.Genomes;
using SorterEvo.Layers;
using Sorting.CompetePool;
using Sorting.Switchables;

namespace SorterEvo.Workflows
{
    public interface ISorterCompWorkflow
    {
        ICompPool CompPool { get; }
        SorterCompPoolParams SorterPoolCompParams { get; }
        ILayer<ISorterGenome> SorterLayer { get; }
        SorterCompState SorterPoolCompState { get; }
        ISorterCompWorkflow Step(int seed);
        ILayer<ISwitchableGroupGenome> SwitchableGroupLayer { get; }
        ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> SorterLayerEval { get; }
        ILayerEval<ISwitchableGroupGenome, IGenomeEval<ISwitchableGroupGenome>> SwitchableGroupLayerEval { get; }
    }

    public static class SorterCompWorkflow
    {
        public static ISorterCompWorkflow Make(
                int seed,
                SwitchableGroupGenomeType switchableGroupGenomeType,
                int keyCount,
                int keyPairCount,
                int switchableGroupSize,
                SorterCompPoolParams sorterCompPoolParams
            )
        {
            var randy = Rando.Fast(seed);

            return new SorterCompWorkflowImpl
                (
                    sorterLayer: SorterLayer.Create
                                (
                                      seed: randy.NextInt(),
                                      genomeCount: sorterCompPoolParams.SorterLayerStartingGenomeCount,
                                      keyCount: keyCount,
                                      keyPairCount: keyPairCount
                                ),
                    switchableGroupLayer: SwitchableGroupLayer.Create
                                (
                                      seed: randy.NextInt(),
                                      switchableGroupGenomeType: switchableGroupGenomeType,
                                      genomeCount: sorterCompPoolParams.SorterLayerStartingGenomeCount,
                                      keyCount: keyCount,
                                      groupSize: switchableGroupSize
                                ),
                    sorterCompPoolParams: sorterCompPoolParams
                );
        }

        public static ISorterCompWorkflow Make(
                ILayer<ISorterGenome> sorterLayer,
                ILayer<ISwitchableGroupGenome> switchableGroupLayer,
                SorterCompPoolParams sorterCompPoolParams
            )
        {
            return new SorterCompWorkflowImpl
                (
                    sorterLayer: sorterLayer,
                    switchableGroupLayer: switchableGroupLayer,
                    sorterCompPoolParams: sorterCompPoolParams
                );
        }
    }

    class SorterCompWorkflowImpl : ISorterCompWorkflow
    {
        public SorterCompWorkflowImpl
            (
                ILayer<ISorterGenome> sorterLayer,
                ILayer<ISwitchableGroupGenome> switchableGroupLayer,
                SorterCompPoolParams sorterCompPoolParams
            )
        {
            _sorterPoolCompState = SorterCompState.ReproGenomes;
            _sorterLayer = sorterLayer;
            _switchableGroupLayer = switchableGroupLayer;
            _compPool = null;
            _sorterPoolCompParams = sorterCompPoolParams;
            _sorterLayerEval = null;
            _switchableGroupLayerEval = null;
        }

        private SorterCompWorkflowImpl
            (
                ISorterCompWorkflow sorterCompWorkflow
            )
        {
            _sorterPoolCompParams = sorterCompWorkflow.SorterPoolCompParams;
            _sorterPoolCompState = sorterCompWorkflow.SorterPoolCompState;
            _sorterLayer = sorterCompWorkflow.SorterLayer;
            _switchableGroupLayer = sorterCompWorkflow.SwitchableGroupLayer;
            _sorterLayerEval = sorterCompWorkflow.SorterLayerEval;
            _switchableGroupLayerEval = sorterCompWorkflow.SwitchableGroupLayerEval;
            _compPool = sorterCompWorkflow.CompPool;
        }

        private SorterCompWorkflowImpl
            (
                SorterCompState sorterPoolCompState,
                ILayer<ISorterGenome> sorterLayer,
                ILayer<ISwitchableGroupGenome> switchableGroupLayer,
                ICompPool compPool,
                ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> sorterLayerEval,
                ILayerEval<ISwitchableGroupGenome, IGenomeEval<ISwitchableGroupGenome>> switchableGroupLayerEval, 
                SorterCompPoolParams sorterPoolCompParams)
        {
            _sorterPoolCompState = sorterPoolCompState;
            _sorterLayer = sorterLayer;
            _switchableGroupLayer = switchableGroupLayer;
            _sorterLayerEval = sorterLayerEval;
            _switchableGroupLayerEval = switchableGroupLayerEval;
            _sorterPoolCompParams = sorterPoolCompParams;
            _compPool = compPool;
        }

        private readonly ICompPool _compPool;
        public ICompPool CompPool
        {
            get { return _compPool; }
        }

        private readonly SorterCompPoolParams _sorterPoolCompParams;
        public SorterCompPoolParams SorterPoolCompParams
        {
            get { return _sorterPoolCompParams; }
        }

        private readonly ILayer<ISorterGenome> _sorterLayer;
        public ILayer<ISorterGenome> SorterLayer
        {
            get { return _sorterLayer; }
        }


        private readonly SorterCompState _sorterPoolCompState;
        public SorterCompState SorterPoolCompState
        {
            get { return _sorterPoolCompState; }
        }

        public ISorterCompWorkflow Step(int seed)
        {
            ISorterCompWorkflow sorterCompWorkflow;
            switch (SorterPoolCompState)
            {
                case SorterCompState.ReproGenomes:
                    sorterCompWorkflow = ReproStep(seed);
                    break;
                case SorterCompState.RunCompetition:
                    sorterCompWorkflow = RunCompetitionStep(seed);
                    break;
                case SorterCompState.EvaluateResults:
                    sorterCompWorkflow = EvaluateResultsStep(seed);
                    break;
                case SorterCompState.UpdateGenomes:
                    sorterCompWorkflow = UpdateGenomesStep(seed);
                    break;
                default:
                    throw new Exception(String.Format("SorterPoolCompState {0} not handled in SorterCompWorkflowImpl.Step", SorterPoolCompState));
            }

            return new SorterCompWorkflowImpl
            (
                sorterCompWorkflow: sorterCompWorkflow
            );
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

        private ISorterCompWorkflow ReproStep(int seed)
        {
            var randy = Rando.Fast(seed);

            var sorterLayer = SorterLayer.Multiply(seed: randy.NextInt(), newGenomeCount: SorterPoolCompParams.SorterLayerExpandedGenomeCount, mutationRate: SorterPoolCompParams.SorterMutationRate, insertionRate: SorterPoolCompParams.SorterMutationRate, deletionRate: SorterPoolCompParams.SorterDeletionRate);

            var switchableGroupLayer = SwitchableGroupLayer.Multiply(seed: randy.NextInt(), newGenomeCount: SorterPoolCompParams.SwitchableLayerExpandedGenomeCount, mutationRate: SorterPoolCompParams.SwitchableLayerExpandedGenomeCount, insertionRate: SorterPoolCompParams.SwitchableGroupInsertionRate, deletionRate: SorterPoolCompParams.SwitchableGroupDeletionRate);

            return new SorterCompWorkflowImpl
                (
                    sorterPoolCompState: SorterCompState.RunCompetition,
                    sorterLayer: sorterLayer,
                    switchableGroupLayer: switchableGroupLayer,
                    compPool: null,
                    sorterLayerEval: null,
                    switchableGroupLayerEval: null,
                    sorterPoolCompParams: SorterPoolCompParams
                );
            
        }

        private ISorterCompWorkflow RunCompetitionStep(int seed)
        {
            var sorters = SorterLayer.Genomes.Select(t=>t.ToSorter());
            ICompPool compPool;

            switch (SwitchableGroupLayer.Genomes[0].SwitchableGroupGenomeType)
            {
                case SwitchableGroupGenomeType.UInt:
                    var switchableGroupsUint = SwitchableGroupLayer.Genomes.Select(t => (ISwitchableGroup<uint>)t.ToSwitchableGroup());
                    compPool = sorters.ToCompPoolParallel(switchableGroupsUint);
                break;
                case SwitchableGroupGenomeType.ULong:
                    var switchableGroupsULong = SwitchableGroupLayer.Genomes.Select(t => (ISwitchableGroup<ulong>)t.ToSwitchableGroup());
                    compPool = sorters.ToCompPoolParallel(switchableGroupsULong);
                break;
                case SwitchableGroupGenomeType.BitArray:
                    var switchableGroupsBitArray = SwitchableGroupLayer.Genomes.Select(t => (ISwitchableGroup<bool[]>)t.ToSwitchableGroup());
                    compPool = sorters.ToCompPoolParallel(switchableGroupsBitArray);
                break;
                case SwitchableGroupGenomeType.IntArray:
                    var switchableGroupsIntArray = SwitchableGroupLayer.Genomes.Select(t => (ISwitchableGroup<int[]>)t.ToSwitchableGroup());
                    compPool = sorters.ToCompPoolParallel(switchableGroupsIntArray);
                break;
                default:
                throw new Exception(String.Format("{0} is not handled in RunCompetitionStep", SwitchableGroupLayer.Genomes[0].SwitchableGroupGenomeType));
            }

            return new SorterCompWorkflowImpl
                (
                    sorterPoolCompState: SorterCompState.EvaluateResults,
                    sorterLayer: SorterLayer,
                    switchableGroupLayer: SwitchableGroupLayer,
                    compPool: compPool,
                    sorterLayerEval: null,
                    switchableGroupLayerEval: null,
                    sorterPoolCompParams: SorterPoolCompParams
                );
        }

        private ISorterCompWorkflow EvaluateResultsStep(int seed)
        {
            var sorterLayerEval =
                CompPool.SorterOnSwitchableGroupSets.Select(
                    t => GenomeEval.Make(
                        genome: SorterLayer.GetGenome(t.Sorter.Guid), 
                        score:  t.SwitchesUsed
                        )
                    ).Make<ISorterGenome, IGenomeEval<ISorterGenome>>();


            var switchableGroupLayerEval = 
                CompPool.SorterOnSwitchableGroups.GroupBy(t => t.SwitchableGroup).Select(
                    g => GenomeEval.Make(
                        genome: SwitchableGroupLayer.GetGenome(g.Key.Guid),
                        score: g.Sum(s=> - s.SwitchesUsed)
                        )
                    ).Make<ISwitchableGroupGenome, IGenomeEval<ISwitchableGroupGenome>>();

            return new SorterCompWorkflowImpl
                (
                    sorterPoolCompState: SorterCompState.UpdateGenomes,
                    sorterLayer: SorterLayer,
                    switchableGroupLayer: SwitchableGroupLayer,
                    compPool: CompPool,
                    sorterLayerEval: sorterLayerEval,
                    switchableGroupLayerEval: switchableGroupLayerEval,
                    sorterPoolCompParams: SorterPoolCompParams
                );
        }

        private ISorterCompWorkflow UpdateGenomesStep(int seed)
        {
            var rando = Rando.Fast(seed);
            SorterLayerEval.GenomeEvals.SubSortShuffle(t => t.Score, rando.NextInt()).Take(SorterPoolCompParams.SorterLayerStartingGenomeCount);
            ILayer<ISorterGenome> sorterLayer = null;

            ILayer<ISwitchableGroupGenome> switchableGroupLayer = null;


            return new SorterCompWorkflowImpl
                (
                    sorterPoolCompState: SorterCompState.ReproGenomes,
                    sorterLayer: Layer.Make(
                        generation: 0,
                        genomes: SorterLayerEval.GenomeEvals
                                                .SubSortShuffle(t => t.Score, rando.NextInt())
                                                .Select(e => e.Genome)
                                                .Take(SorterPoolCompParams.SorterLayerStartingGenomeCount)
                    ),
                    switchableGroupLayer: Layer.Make(
                        generation: 0,
                        genomes: SwitchableGroupLayerEval.GenomeEvals
                                                .SubSortShuffle(t => t.Score, rando.NextInt())
                                                .Select(e=>e.Genome)
                                                .Take(SorterPoolCompParams.SwitchableLayerStartingGenomeCount)
                    ),
                    compPool: null,
                    sorterLayerEval: null,
                    switchableGroupLayerEval: null,
                    sorterPoolCompParams: SorterPoolCompParams
                );
         }

    }
}