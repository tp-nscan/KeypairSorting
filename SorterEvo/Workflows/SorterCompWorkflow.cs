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
        //ITracker<ISorterCompWorkflow> Tracker { get; }
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
                //ITracker<ISorterCompWorkflow> tracker,
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
                    //tracker: tracker,
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
                //ITracker<ISorterCompWorkflow> tracker,
                ILayer<ISorterGenome> sorterLayer,
                ILayer<ISwitchableGroupGenome> switchableGroupLayer,
                SorterCompPoolParams sorterCompPoolParams
            )
        {
            return new SorterCompWorkflowImpl
                (
                    //tracker: tracker,
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
                //ITracker<ISorterCompWorkflow> tracker,
                ILayer<ISorterGenome> sorterLayer,
                ILayer<ISwitchableGroupGenome> switchableGroupLayer,
                SorterCompPoolParams sorterCompPoolParams
            )
        {
            //_tracker = tracker;
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
                //ITracker<ISorterCompWorkflow> tracker, 
                ISorterCompWorkflow sorterCompWorkflow
            )
        {
            //_tracker = tracker;
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
                //ITracker<ISorterCompWorkflow> tracker,
                SorterCompState sorterPoolCompState,
                ILayer<ISorterGenome> sorterLayer,
                ILayer<ISwitchableGroupGenome> switchableGroupLayer,
                ICompPool compPool,
                ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> sorterLayerEval,
                ILayerEval<ISwitchableGroupGenome, IGenomeEval<ISwitchableGroupGenome>> switchableGroupLayerEval, 
                SorterCompPoolParams sorterPoolCompParams)
        {
            //_tracker = tracker;
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

        //private readonly ITracker<ISorterCompWorkflow> _tracker;
        //public ITracker<ISorterCompWorkflow> Tracker
        //{
        //    get { return _tracker; }
        //}

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
                //tracker:  Tracker.TrackItem(sorterCompWorkflow)
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

            return new SorterCompWorkflowImpl
                (
                    //tracker: Tracker,
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
                    //tracker: Tracker,
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
            ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> sorterLayerEval = null;
            ILayerEval<ISwitchableGroupGenome, IGenomeEval<ISwitchableGroupGenome>> switchableGroupLayerEval = null;
            return new SorterCompWorkflowImpl
                (
                    //tracker: Tracker,
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
            ILayer<ISorterGenome> sorterLayer = null;
            ILayer<ISwitchableGroupGenome> switchableGroupLayer = null;
            return new SorterCompWorkflowImpl
                (
                    //tracker: Tracker,
                    sorterPoolCompState: SorterCompState.ReproGenomes,
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