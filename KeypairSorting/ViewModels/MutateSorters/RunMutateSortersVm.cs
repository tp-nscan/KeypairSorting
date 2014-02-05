using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using KeypairSorting.Resources;
using KeypairSorting.ViewModels.Parts;
using SorterEvo.Evals;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.MutateSorters
{
    public class RunMutateSortersVm : ViewModelBase, ICreateRunSelectorVm
    {
        public RunMutateSortersVm(ISorterMutateParams sorterMutateParams, IEnumerable<ISorterGenomeEvalVm> sorterGenomeEvalVms)
        {
            MutateSortersRunnerVm = new MutateSortersRunnerVm(sorterMutateParams, sorterGenomeEvalVms);
            MutateSortersRunnerVm.OnIterationResult.Subscribe(ReportBestResult);
            _sorterGenomeEvalGridVm = new SorterGenomeEvalGridVm("Selected mutants");
            ReportFrequency = 10;
            _stopwatch = new Stopwatch();
        }

        void ReportBestResult(IEnumerable<ISorterGenomeEval> results)
        {
            OnPropertyChanged("ProcTime");
            foreach (var sorterGenomeEval in results)
            {
                _sorterGenomeEvalGridVm.SorterGenomeEvalVms.Add(sorterGenomeEval.ToSorterGenomeEvalVm());
            }
        }

        public CreateRunTemplateType CreateRunTemplateType
        {
            get { return CreateRunTemplateType.Run; }
        }

        public string Description
        {
            get { return "Mutate sorters"; }
        }

        public MutateSortersRunnerVm MutateSortersRunnerVm { get; set; }

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

            await MutateSortersRunnerVm.OnRunAsync(_cancellationTokenSource);

            _stopwatch.Stop();
            Busy = false;
        }

        public bool CanRunCommand()
        {
            return !_busy
                   &&
                   ReportFrequency.HasValue;
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

        private int? _reportFrequency;
        public int? ReportFrequency
        {
            get { return _reportFrequency; }
            set
            {
                _reportFrequency = value;
                MutateSortersRunnerVm.ReportFrequency = value;
                CommandManager.InvalidateRequerySuggested();
                OnPropertyChanged("ReportFrequency");
            }
        }

        private readonly Stopwatch _stopwatch;
        public string ProcTime
        {
            get { return _stopwatch.Elapsed.TotalSeconds.ToString("0"); }
        }

        private SorterGenomeEvalGridVm _sorterGenomeEvalGridVm;
        public SorterGenomeEvalGridVm SorterGenomeEvalGridVm
        {
            get { return _sorterGenomeEvalGridVm; }
            set { _sorterGenomeEvalGridVm = value; }
        }
    }
}
