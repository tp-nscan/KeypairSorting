using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Entities.BackgroundWorkers;
using KeypairSorting.Resources;
using KeypairSorting.ViewModels.Parts;
using MathUtils.Rand;
using SorterEvo.Evals;
using SorterEvo.Layers;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.MakeTunedSorters
{
    public class ScpRunnerVm : ViewModelBase
    {
        public ScpRunnerVm(IScpParams scpParams, IEnumerable<ISorterGenomeEvalVm> sorterGenomeEvalVms)
        {
            _sorterGenomeEvalGridVmInitial = new SorterGenomeEvalGridVm("Progenitors");
            _sorterGenomeEvalGridVmInitial.SorterGenomeEvalVms.AddMany(sorterGenomeEvalVms);
            _sorterGenomeEvalGridVm = new SorterGenomeEvalGridVm("Current population");
            _scpParamsVm = new ScpParamVm(scpParams);
        }

        public ConfigRunTemplateType ConfigRunTemplateType
        {
            get { return ConfigRunTemplateType.Run; }
        }

        public string Description
        {
            get { return "Tune sorters"; }
        }

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            set
            {
                _busy = value;
                OnPropertyChanged("Busy");
            }
        }

        private readonly SorterGenomeEvalGridVm _sorterGenomeEvalGridVmInitial;
        public SorterGenomeEvalGridVm SorterGenomeEvalGridVmInitial
        {
            get { return _sorterGenomeEvalGridVmInitial; }
        }

        private readonly SorterGenomeEvalGridVm _sorterGenomeEvalGridVm;
        public SorterGenomeEvalGridVm SorterGenomeEvalGridVm
        {
            get { return _sorterGenomeEvalGridVm; }
        }

        private readonly ScpParamVm _scpParamsVm;
        public ScpParamVm ScpParamsVm
        {
            get { return _scpParamsVm; }
        }

        #region RunCommand


        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        async Task OnRunCommand()
        {
            Busy = true;
            _cancellationTokenSource = new CancellationTokenSource();

            var rando = Rando.Fast(ScpParamsVm.Seed);

            var rbw = RecursiveBackgroundWorker.Make
                (
                    initialState: ScpWorkflow.Make
                        (
                           sorterLayer: SorterLayer.Make(
                               sorterGenomes: _sorterGenomeEvalGridVmInitial.SorterGenomeEvalVms.Select(t => t.GetSorterGenomeEval().SorterGenome),
                               generation: 0
                           ),
                           scpParams: ScpParamsVm.GetParams,
                           generation: 0
                        ),
                   recursion: (i, c) =>
                   {
                       var nextStep = i;
                       while (true)
                       {
                           if (_cancellationTokenSource.IsCancellationRequested)
                           {
                               return IterationResult.Make<IScpWorkflow>(null, ProgressStatus.StepIncomplete);
                           }

                           nextStep = nextStep.Step(rando.NextInt());

                           if (nextStep.CompWorkflowState == CompWorkflowState.UpdateGenomes)
                           {
                               return IterationResult.Make(nextStep, ProgressStatus.StepComplete);
                           }
                       }
                   },
                    totalIterations: ScpParamsVm.TotalGenerations,
                    cancellationTokenSource: _cancellationTokenSource
                );

            rbw.OnIterationResult.Subscribe(UpdateSorterTuneResults);
            await rbw.Start();

            Busy = false;
        }

        readonly Dictionary<Guid, ISorterGenomeEval> _sorterGenomeEvals = new Dictionary<Guid, ISorterGenomeEval>();

        private void UpdateSorterTuneResults(IIterationResult<IScpWorkflow> result)
        {
            if (result.ProgressStatus == ProgressStatus.StepComplete)
            {
                ScpParamsVm.CurrentGeneration = result.Data.Generation;
                SorterGenomeEvalGridVm.SorterGenomeEvalVms.Clear();
                OnPropertyChanged("ProcTime");
                var currentGeneration = result.Data.Generation;

                var sorterEvalDict = result.Data.CompPool.SorterEvals.ToDictionary(e => e.Sorter.Guid);

                foreach (var sorterGenomeEval in result.Data.SorterLayerEval.GenomeEvals)
                {
                    if (_sorterGenomeEvals.ContainsKey(sorterGenomeEval.Genome.ParentGuid))
                    {
                        var oldSorterEval = _sorterGenomeEvals[sorterGenomeEval.Genome.ParentGuid];
                        _sorterGenomeEvals[sorterGenomeEval.Guid] = SorterGenomeEval.Make
                            (
                                sorterGenome: sorterGenomeEval.Genome,
                                parentGenomeEval: oldSorterEval,
                                sorterEval: sorterEvalDict[sorterGenomeEval.Guid],
                                generation: currentGeneration
                           );
                    }
                    else
                    {
                        _sorterGenomeEvals[sorterGenomeEval.Guid] = SorterGenomeEval.Make
                            (
                                sorterGenome: sorterGenomeEval.Genome,
                                ancestors: ImmutableStack<Guid>.Empty,
                                sorterEval: sorterEvalDict[sorterGenomeEval.Guid],
                                generation: currentGeneration
                           );
                    }
                }

                var evalGuidList = _sorterGenomeEvals.Select(t => t.Key).ToList();
                foreach (var evalGuid in evalGuidList)
                {
                    if (_sorterGenomeEvals[evalGuid].Generation < currentGeneration - 1)
                    {
                        _sorterGenomeEvals.Remove(evalGuid);
                    }
                }

                foreach (var sorterGenomeEval in _sorterGenomeEvals.Values.OrderBy(r => r.SorterEval.SwitchUseCount))
                {
                    SorterGenomeEvalGridVm.SorterGenomeEvalVms.Add(sorterGenomeEval.ToSorterGenomeEvalVm());
                }
            }
        }

        bool CanRunCommand()
        {
            return !_busy;
        }

        #endregion // RunCommand





    }
}
