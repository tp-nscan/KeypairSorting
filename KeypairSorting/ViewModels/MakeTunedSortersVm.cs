using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Entities.BackgroundWorkers;
using KeypairSorting.Resources;
using KeypairSorting.ViewModels.Parts;
using MathUtils.Rand;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels
{
    public class MakeTunedSortersVm : ViewModelBase, IToolTemplateVm
    {

        public MakeTunedSortersVm()
        {
            _sorterGenomeGridVm = new SorterGenomeGridVm();
            _sorterCompPoolParamsVm = new SorterCompPoolParamsVm
                (
                    SorterCompPoolParams.MakeStandard(
                        sorterLayerStartingGenomeCount: 10,
                        sorterLayerExpandedGenomeCount: 30,
                        sorterMutationRate: 0.03,
                        sorterInsertionRate: 0.03,
                        sorterDeletionRate: 0.03,
                        name: "standard"
                    )
                );
            _sorterEvalGridVm = new SorterEvalGridVm();
            _stopwatch = new Stopwatch();
        }

        public ToolTemplateType ToolTemplateType
        {
            get { return ToolTemplateType.SorterTune; }
        }


        public string Description
        {
            get { return "Optimize Sorters"; }
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

        private readonly SorterGenomeGridVm _sorterGenomeGridVm;
        public SorterGenomeGridVm SorterGenomeGridVm
        {
            get { return _sorterGenomeGridVm; }
        }

        private readonly SorterCompPoolParamsVm _sorterCompPoolParamsVm;
        public SorterCompPoolParamsVm SorterCompPoolParamsVm
        {
            get { return _sorterCompPoolParamsVm; }
        }

        private readonly SorterEvalGridVm _sorterEvalGridVm;
        public SorterEvalGridVm SorterEvalGridVm
        {
            get { return _sorterEvalGridVm; }
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

        //ISorterCompPoolWorkflow
        async Task OnRunCommand()
        {
            Busy = true;
            _stopwatch.Reset();
            _stopwatch.Start();
            _cancellationTokenSource = new CancellationTokenSource();

            var rando = Rando.Fast(Seed.Value);


            var rbw = RecursiveBackgroundWorker.Make
                (
                    42,
                    (i, c) =>
                    {
                        Thread.Sleep(100);
                        if (_cancellationTokenSource.IsCancellationRequested)
                        {
                            return IterationResult.Make(0, ProgressStatus.StepIncomplete);
                        }

                        return IterationResult.Make(i + 1, ProgressStatus.StepComplete);
                    },
                    int.MaxValue,
                    _cancellationTokenSource
                );

            rbw.OnIterationResult.Subscribe(UpdateSorterTuneResults);
            await rbw.Start();

            _stopwatch.Stop();
            Busy = false;
        }

        private void UpdateSorterTuneResults(IIterationResult<int> result)
        {
            OnPropertyChanged("ProcTime");
        }

        bool CanRunCommand()
        {
            return !_busy && Seed.HasValue;
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
