using System;
using Genomic.Genomes;
using Genomic.Layers;
using Genomic.Trackers;
using SorterEvo.Genomes;
using SorterEvo.Layers;
using Sorting.CompetePool;

namespace SorterEvo.Workflows
{
    public interface ISorterPoolCompWorkflow
    {
        ICompPool CompPool { get; }
        ITracker<ISorterPoolCompWorkflow> Tracker { get; }
        SorterPoolCompParams SorterPoolCompParams { get; }
        ISorterLayer SorterLayer { get; }
        SorterPoolCompState SorterPoolCompState { get; }
        ISorterPoolCompWorkflow Step();
        ILayer<ISwitchableGroupGenome> SwitchableGroupLayer { get; }
        ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> SorterLayerEval { get; }
        ILayerEval<ISwitchableGroupGenome, IGenomeEval<ISwitchableGroupGenome>> SwitchableGroupLayerEval { get; }
    }

    public static class SorterPoolCompWorkflow
    {

    }

    class SorterPoolCompWorkflowImpl : ISorterPoolCompWorkflow
    {
        public SorterPoolCompWorkflowImpl
        (
            ITracker<ISorterPoolCompWorkflow> tracker,
            ISorterLayer sorterLayer,
            ILayer<ISwitchableGroupGenome> switchableGroupLayer, 
            ICompPool compPool, 
            SorterPoolCompParams sorterPoolCompParams)
        {
            _tracker = tracker;
            _sorterPoolCompState = SorterPoolCompState.ReproGenomes;
            _sorterLayer = sorterLayer;
            _switchableGroupLayer = switchableGroupLayer;
            _compPool = compPool;
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
                ISorterLayer sorterLayer,
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

        private readonly ISorterLayer _sorterLayer;
        public ISorterLayer SorterLayer
        {
            get { return _sorterLayer; }
        }


        private readonly SorterPoolCompState _sorterPoolCompState;
        public SorterPoolCompState SorterPoolCompState
        {
            get { return _sorterPoolCompState; }
        }

        public ISorterPoolCompWorkflow Step()
        {
            ISorterPoolCompWorkflow sorterPoolCompWorkflow;
            switch (SorterPoolCompState)
            {
                case SorterPoolCompState.ReproGenomes:

                    var sorterLayer = (ISorterLayer) Layer.Update
                        (
                            genomes: SorterLayer.Genomes,
                            generation: SorterLayer.Generation + 1,
                            seed: SorterLayer.Seed,
                            newGenomeCount: SorterLayer.Genomes.Count,
                            genomeCopyFunc: Layers.SorterLayer.CopyFunc
                                (
                                    mutationRate: SorterPoolCompParams.SorterMutationRate,
                                    insertionRate: SorterPoolCompParams.SorterInsertionRate,
                                    deletionRate: SorterPoolCompParams.SorterDeletionRate
                                )
                        );

                    var switchableGroupLayer = Layer.Update
                        (
                            genomes: SwitchableGroupLayer.Genomes,
                            generation: SwitchableGroupLayer.Generation + 1,
                            seed: SwitchableGroupLayer.Seed,
                            newGenomeCount: SwitchableGroupLayer.Genomes.Count,
                            genomeCopyFunc: Layers.SwitchableGroupLayer.CopyFunc
                                (
                                    mutationRate: SorterPoolCompParams.SwitchableGroupMutationRate,
                                    insertionRate: SorterPoolCompParams.SwitchableGroupInsertionRate,
                                    deletionRate: SorterPoolCompParams.SwitchableGroupDeletionRate
                                )
                        );


                    sorterPoolCompWorkflow = new SorterPoolCompWorkflowImpl
                        (
                            tracker: Tracker,
                            sorterPoolCompState: SorterPoolCompState.RunCompetition,
                            sorterLayer: sorterLayer,
                            switchableGroupLayer: switchableGroupLayer,
                            compPool : null, 
                            sorterLayerEval: null,
                            switchableGroupLayerEval: null,
                            sorterPoolCompParams: SorterPoolCompParams
                        );

                    break;
                case SorterPoolCompState.RunCompetition:
                    ICompPool compPool = null;
                    sorterPoolCompWorkflow = new SorterPoolCompWorkflowImpl
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
                    break;
                case SorterPoolCompState.EvaluateResults:
                    ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> sorterLayerEval = null;
                    ILayerEval<ISwitchableGroupGenome, IGenomeEval<ISwitchableGroupGenome>> switchableGroupLayerEval = null;
                    sorterPoolCompWorkflow = new SorterPoolCompWorkflowImpl
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
                    break;
                case SorterPoolCompState.UpdateGenomes:
                    sorterLayer = null;
                    switchableGroupLayer = null;
                    sorterPoolCompWorkflow = new SorterPoolCompWorkflowImpl
                        (
                            tracker: Tracker,
                            sorterPoolCompState: SorterPoolCompState.ReproGenomes,
                            sorterLayer: sorterLayer,
                            switchableGroupLayer: null,
                            compPool: null,
                            sorterLayerEval: null,
                            switchableGroupLayerEval: null,
                            sorterPoolCompParams: SorterPoolCompParams
                        );
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
    }
}