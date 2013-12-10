using System;
using System.Linq;
using Genomic.Genomes;
using Genomic.Layers;
using Genomic.Trackers;
using MathUtils.Rand;
using SorterEvo.Genomes;
using SorterEvo.Layers;
using Sorting.CompetePool;
using Sorting.Switchables;

namespace SorterEvo.Workflows
{
    public interface ISorterPoolCompWorkflow
    {
        ICompPool CompPool { get; }
        ITracker<ISorterPoolCompWorkflow> Tracker { get; }
        SorterPoolCompParams SorterPoolCompParams { get; }
        ILayer<ISorterGenome> SorterLayer { get; }
        SorterPoolCompState SorterPoolCompState { get; }
        ISorterPoolCompWorkflow Step(int seed);
        ILayer<ISwitchableGroupGenome> SwitchableGroupLayer { get; }
        ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> SorterLayerEval { get; }
        ILayerEval<ISwitchableGroupGenome, IGenomeEval<ISwitchableGroupGenome>> SwitchableGroupLayerEval { get; }
    }

    public static class SorterPoolCompWorkflow
    {
        public static ISorterPoolCompWorkflow Make(
                ITracker<ISorterPoolCompWorkflow> tracker,
                int seed,
                SwitchableGroupGenomeType switchableGroupGenomeType,
                int keyCount,
                int keyPairCount,
                int switchableGroupSize,
                SorterPoolCompParams sorterPoolCompParams
            )
        {
            var randy = Rando.Fast(seed);

            return new SorterPoolCompWorkflowImpl
                (
                    tracker: tracker,
                    sorterLayer: SorterLayer.Create
                                (
                                      seed: randy.NextInt(),
                                      genomeCount: sorterPoolCompParams.SorterLayerStartingGenomeCount,
                                      keyCount: keyCount,
                                      keyPairCount: keyPairCount
                                ),
                    switchableGroupLayer: SwitchableGroupLayer.Create
                                (
                                      seed: randy.NextInt(),
                                      switchableGroupGenomeType: switchableGroupGenomeType,
                                      genomeCount: sorterPoolCompParams.SorterLayerStartingGenomeCount,
                                      keyCount: keyCount,
                                      groupSize: switchableGroupSize
                                ),
                    sorterPoolCompParams: sorterPoolCompParams
                );
        }

        public static ISorterPoolCompWorkflow Make(
                ITracker<ISorterPoolCompWorkflow> tracker,
                ILayer<ISorterGenome> sorterLayer,
                ILayer<ISwitchableGroupGenome> switchableGroupLayer,
                SorterPoolCompParams sorterPoolCompParams
            )
        {
            return new SorterPoolCompWorkflowImpl
                (
                    tracker: tracker,
                    sorterLayer: sorterLayer,
                    switchableGroupLayer: switchableGroupLayer,
                    sorterPoolCompParams: sorterPoolCompParams
                );
        }
    }

    class SorterPoolCompWorkflowImpl : ISorterPoolCompWorkflow
    {
        public SorterPoolCompWorkflowImpl
            (
                ITracker<ISorterPoolCompWorkflow> tracker,
                ILayer<ISorterGenome> sorterLayer,
                ILayer<ISwitchableGroupGenome> switchableGroupLayer,
                SorterPoolCompParams sorterPoolCompParams
            )
        {
            _tracker = tracker;
            _sorterPoolCompState = SorterPoolCompState.ReproGenomes;
            _sorterLayer = sorterLayer;
            _switchableGroupLayer = switchableGroupLayer;
            _compPool = null;
            _sorterPoolCompParams = sorterPoolCompParams;
            _sorterLayerEval = null;
            _switchableGroupLayerEval = null;
        }

        private SorterPoolCompWorkflowImpl
            (
                ITracker<ISorterPoolCompWorkflow> tracker, 
                ISorterPoolCompWorkflow sorterPoolCompWorkflow
            )
        {
            _tracker = tracker;
            _sorterPoolCompParams = sorterPoolCompWorkflow.SorterPoolCompParams;
            _sorterPoolCompState = sorterPoolCompWorkflow.SorterPoolCompState;
            _sorterLayer = sorterPoolCompWorkflow.SorterLayer;
            _switchableGroupLayer = sorterPoolCompWorkflow.SwitchableGroupLayer;
            _sorterLayerEval = sorterPoolCompWorkflow.SorterLayerEval;
            _switchableGroupLayerEval = sorterPoolCompWorkflow.SwitchableGroupLayerEval;
            _compPool = sorterPoolCompWorkflow.CompPool;
        }

        private SorterPoolCompWorkflowImpl
            (
                ITracker<ISorterPoolCompWorkflow> tracker,
                SorterPoolCompState sorterPoolCompState,
                ILayer<ISorterGenome> sorterLayer,
                ILayer<ISwitchableGroupGenome> switchableGroupLayer,
                ICompPool compPool,
                ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> sorterLayerEval,
                ILayerEval<ISwitchableGroupGenome, IGenomeEval<ISwitchableGroupGenome>> switchableGroupLayerEval, 
                SorterPoolCompParams sorterPoolCompParams)
        {
            _tracker = tracker;
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

        private readonly ITracker<ISorterPoolCompWorkflow> _tracker;
        public ITracker<ISorterPoolCompWorkflow> Tracker
        {
            get { return _tracker; }
        }

        private readonly SorterPoolCompParams _sorterPoolCompParams;
        public SorterPoolCompParams SorterPoolCompParams
        {
            get { return _sorterPoolCompParams; }
        }

        private readonly ILayer<ISorterGenome> _sorterLayer;
        public ILayer<ISorterGenome> SorterLayer
        {
            get { return _sorterLayer; }
        }


        private readonly SorterPoolCompState _sorterPoolCompState;
        public SorterPoolCompState SorterPoolCompState
        {
            get { return _sorterPoolCompState; }
        }

        public ISorterPoolCompWorkflow Step(int seed)
        {
            ISorterPoolCompWorkflow sorterPoolCompWorkflow;
            switch (SorterPoolCompState)
            {
                case SorterPoolCompState.ReproGenomes:
                    sorterPoolCompWorkflow = ReproStep(seed);
                    break;
                case SorterPoolCompState.RunCompetition:
                    sorterPoolCompWorkflow = RunCompetitionStep(seed);
                    break;
                case SorterPoolCompState.EvaluateResults:
                    sorterPoolCompWorkflow = EvaluateResultsStep(seed);
                    break;
                case SorterPoolCompState.UpdateGenomes:
                    sorterPoolCompWorkflow = UpdateGenomesStep(seed);
                    break;
                default:
                    throw new Exception(String.Format("SorterPoolCompState {0} not handled in SorterPoolCompWorkflowImpl.Step", SorterPoolCompState));
            }

            return new SorterPoolCompWorkflowImpl
            (
                sorterPoolCompWorkflow: sorterPoolCompWorkflow, 
                tracker:  Tracker.TrackItem(sorterPoolCompWorkflow)
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

        private ISorterPoolCompWorkflow ReproStep(int seed)
        {
            var randy = Rando.Fast(seed);

            var sorterLayer = Layers.SorterLayer.Multiply
            (
                sorterGenomeLayer:SorterLayer,
                seed: randy.NextInt(),
                newGenomeCount: SorterPoolCompParams.SorterLayerExpandedGenomeCount,
                mutationRate: SorterPoolCompParams.SorterMutationRate,
                insertionRate: SorterPoolCompParams.SorterMutationRate,
                deletionRate: SorterPoolCompParams.SorterDeletionRate
            );

            var switchableGroupLayer = Layers.SwitchableGroupLayer.Multiply
                (
                    switchableGroupGenomeLayer: SwitchableGroupLayer,
                    seed: randy.NextInt(),
                    newGenomeCount: SorterPoolCompParams.SwitchableLayerExpandedGenomeCount,
                    mutationRate: SorterPoolCompParams.SwitchableLayerExpandedGenomeCount,
                    insertionRate: SorterPoolCompParams.SwitchableGroupInsertionRate,
                    deletionRate: SorterPoolCompParams.SwitchableGroupDeletionRate
                );

            return new SorterPoolCompWorkflowImpl
                (
                    tracker: Tracker,
                    sorterPoolCompState: SorterPoolCompState.RunCompetition,
                    sorterLayer: sorterLayer,
                    switchableGroupLayer: switchableGroupLayer,
                    compPool: null,
                    sorterLayerEval: null,
                    switchableGroupLayerEval: null,
                    sorterPoolCompParams: SorterPoolCompParams
                );
            
        }

        private ISorterPoolCompWorkflow RunCompetitionStep(int seed)
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

            return new SorterPoolCompWorkflowImpl
                (
                    tracker: Tracker,
                    sorterPoolCompState: SorterPoolCompState.EvaluateResults,
                    sorterLayer: SorterLayer,
                    switchableGroupLayer: SwitchableGroupLayer,
                    compPool: compPool,
                    sorterLayerEval: null,
                    switchableGroupLayerEval: null,
                    sorterPoolCompParams: SorterPoolCompParams
                );
        }

        private ISorterPoolCompWorkflow EvaluateResultsStep(int seed)
        {
            ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> sorterLayerEval = null;
            ILayerEval<ISwitchableGroupGenome, IGenomeEval<ISwitchableGroupGenome>> switchableGroupLayerEval = null;
            return new SorterPoolCompWorkflowImpl
                (
                    tracker: Tracker,
                    sorterPoolCompState: SorterPoolCompState.UpdateGenomes,
                    sorterLayer: SorterLayer,
                    switchableGroupLayer: SwitchableGroupLayer,
                    compPool: CompPool,
                    sorterLayerEval: sorterLayerEval,
                    switchableGroupLayerEval: switchableGroupLayerEval,
                    sorterPoolCompParams: SorterPoolCompParams
                );
        }

        private ISorterPoolCompWorkflow UpdateGenomesStep(int seed)
        {
            ILayer<ISorterGenome> sorterLayer = null;
            ILayer<ISwitchableGroupGenome> switchableGroupLayer = null;
            return new SorterPoolCompWorkflowImpl
                (
                    tracker: Tracker,
                    sorterPoolCompState: SorterPoolCompState.ReproGenomes,
                    sorterLayer: sorterLayer,
                    switchableGroupLayer: switchableGroupLayer,
                    compPool: null,
                    sorterLayerEval: null,
                    switchableGroupLayerEval: null,
                    sorterPoolCompParams: SorterPoolCompParams
                );
         }

    }
}