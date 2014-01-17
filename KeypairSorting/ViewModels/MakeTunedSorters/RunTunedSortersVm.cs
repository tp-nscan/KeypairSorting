using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Entities.BackgroundWorkers;
using KeypairSorting.Resources;
using KeypairSorting.ViewModels.Parts;
using MathUtils.Rand;
using SorterEvo.Evals;
using SorterEvo.Genomes;
using SorterEvo.Layers;
using SorterEvo.Workflows;
using Sorting.CompetePools;
using WpfUtils;

namespace KeypairSorting.ViewModels.MakeTunedSorters
{
    public class RunTunedSortersVm : ViewModelBase, IConfigRunSelectorVm
    {
        public RunTunedSortersVm(ISorterCompPoolParams sorterCompPoolParams)
        {
            _sorterGenomeEvalGridVmInitial = new SorterGenomeEvalGridVm(string.Empty);
            _sorterGenomeEvalGridVm = new SorterGenomeEvalGridVm(string.Empty);
            _sorterCompPoolParamsVm = new SorterCompPoolParamsVm(sorterCompPoolParams);
            _stopwatch = new Stopwatch();
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
                CommandManager.InvalidateRequerySuggested();
                OnPropertyChanged("Busy");
            }
        }

        private int? _seed;
        public int? Seed
        {
            get { return _seed; }
            set
            {
                _seed = value;
                OnPropertyChanged("Seed");
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

        private readonly SorterCompPoolParamsVm _sorterCompPoolParamsVm;
        public SorterCompPoolParamsVm SorterCompPoolParamsVm
        {
            get { return _sorterCompPoolParamsVm; }
        }

        #region RunCommand

        RelayCommand _runCommand;
        public ICommand RunCommand
        {
            get
            {
                return _runCommand ?? (_runCommand
                    = new RelayCommand
                        (
                            param => OnRunCommand(),
                            param => CanRunCommand()
                        ));
            }
        }

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        async Task OnRunCommand()
        {
            Busy = true;
            _stopwatch.Reset();
            _stopwatch.Start();
            _cancellationTokenSource = new CancellationTokenSource();

            var rando = Rando.Fast(Seed.Value);


            var rbw = RecursiveBackgroundWorker.Make
                (
                    initialState: SorterCompPoolWorkflow.Make
                        (
                           sorterLayer: SorterLayer.Make(
                               sorterGenomes: _sorterGenomeEvalGridVmInitial.SorterGenomeEvalVms.Select(t => t.GetSorterGenomeEval().SorterGenome),
                               generation: 0
                           ),
                           sorterCompPoolParams: SorterCompPoolParamsVm.GetParams,
                           generation: 0
                        ),
                   recursion: (i, c) =>
                   {
                       var nextStep = i;
                       while (true)
                       {
                           if (_cancellationTokenSource.IsCancellationRequested)
                           {
                               return IterationResult.Make<ISorterCompPoolWorkflow>(null, ProgressStatus.StepIncomplete);
                           }

                           nextStep = nextStep.Step(rando.NextInt());

                           if (nextStep.CompWorkflowState == CompWorkflowState.UpdateGenomes)
                           {
                               return IterationResult.Make(nextStep, ProgressStatus.StepComplete);
                           }
                       }
                   },
                    totalIterations: int.MaxValue,
                    cancellationTokenSource: _cancellationTokenSource
                );

            rbw.OnIterationResult.Subscribe(UpdateSorterTuneResults);
            await rbw.Start();

            _stopwatch.Stop();
            Busy = false;
        }

        readonly Dictionary<Guid, ISorterGenomeEval> _sorterGenomeEvals = new Dictionary<Guid, ISorterGenomeEval>();

        private void UpdateSorterTuneResults(IIterationResult<ISorterCompPoolWorkflow> result)
        {
            if (result.ProgressStatus == ProgressStatus.StepComplete)
            {
                _generation = result.Data.Generation;
                SorterGenomeEvalGridVm.SorterGenomeEvalVms.Clear();
                OnPropertyChanged("Generation");
                OnPropertyChanged("ProcTime");
                var currentGeneration = result.Data.Generation;
                foreach (var sorterGenomeEval in result.Data.SorterLayerEval.GenomeEvals)
                {
                    if (_sorterGenomeEvals.ContainsKey(sorterGenomeEval.Genome.ParentGuid))
                    {
                        var oldSorterEval = _sorterGenomeEvals[sorterGenomeEval.Genome.ParentGuid];
                        _sorterGenomeEvals[sorterGenomeEval.Guid] = SorterGenomeEval.Make(
                            sorterGenome: sorterGenomeEval.Genome,
                            parentGenomeEval: oldSorterEval,
                            sorterEval: SorterEval.Make(
                                            sorter: sorterGenomeEval.Genome.ToSorter(),
                                            switchableGroupGuid: Guid.Empty,
                                            success: true,
                                            switchUseCount: (int)sorterGenomeEval.Score
                                        ),
                            generation: currentGeneration
                        );

                    }
                    else
                    {
                        _sorterGenomeEvals[sorterGenomeEval.Guid] = SorterGenomeEval.Make(
                            sorterGenome: sorterGenomeEval.Genome,
                            ancestors: ImmutableStack<Guid>.Empty,
                            sorterEval: SorterEval.Make(
                                            sorter: sorterGenomeEval.Genome.ToSorter(),
                                            switchableGroupGuid: Guid.Empty,
                                            success: true,
                                            switchUseCount: (int)sorterGenomeEval.Score
                                        ),
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
            return
                !_busy
                && Seed.HasValue;
        }

        #endregion // RunCommand


        #region StopCommand

        RelayCommand _stopCommand;

        public ICommand StopCommand
        {
            get
            {
                return _stopCommand ?? (_stopCommand
                    = new RelayCommand
                        (
                            param => OnStopCommand(),
                            param => CanStopCommand()
                        ));
            }
        }

        void OnStopCommand()
        {
            _cancellationTokenSource.Cancel();
        }

        bool CanStopCommand()
        {
            return _busy;
        }

        #endregion // StopCommand


        private readonly Stopwatch _stopwatch;
        public string ProcTime
        {
            get { return _stopwatch.Elapsed.TotalSeconds.ToString("0"); }
        }

        private int _generation = 0;
        public int Generation
        {
            get { return _generation; }
        }
    }
}
