using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using KeypairSorting.Resources;
using KeypairSorting.ViewModels.Parts;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.MakeTunedSorters
{
    public class RunScpVm : ViewModelBase, IConfigRunSelectorVm
    {
        public RunScpVm(IScpParams scpParams, IEnumerable<ISorterGenomeEvalVm> sorterGenomeEvalVms)
        {
            ScpRunnerVm = new ScpRunnerVm(scpParams, sorterGenomeEvalVms);
            ScpRunnerVm.OnIterationResult.Subscribe(ReportBestResult);
            ReportFrequency = 1;
            _trajectoryGridVm = new SgHistoryGridVm();
            _stopwatch = new Stopwatch();
        }

        void ReportBestResult(Tuple<string, string> result)
        {
            OnPropertyChanged("ProcTime");
            TrajectoryGridVm.SgHistoryVms.Add(new SgHistoryVm(result.Item1, result.Item2));
        }

        private readonly SgHistoryGridVm _trajectoryGridVm;
        public SgHistoryGridVm TrajectoryGridVm
        {
            get { return _trajectoryGridVm; }
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

        public ScpRunnerVm ScpRunnerVm { get; set; }

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

            await ScpRunnerVm.OnRunAsync(_cancellationTokenSource);

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
                ScpRunnerVm.ReportFrequency = value;
                CommandManager.InvalidateRequerySuggested();
                OnPropertyChanged("ReportFrequency");
            }
        }

        private readonly Stopwatch _stopwatch;
        public string ProcTime
        {
            get { return _stopwatch.Elapsed.TotalSeconds.ToString("0"); }
        }

    }
}
