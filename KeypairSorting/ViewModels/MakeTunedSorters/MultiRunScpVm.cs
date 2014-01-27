using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using KeypairSorting.Resources;
using KeypairSorting.ViewModels.Parts;
using WpfUtils;

namespace KeypairSorting.ViewModels.MakeTunedSorters
{
    public class MultiRunScpVm : ViewModelBase, IConfigRunSelectorVm
    {
        public MultiRunScpVm(IEnumerable<ConfigScpVm> configScpVms)
        {
            _trajectoryGridVm = new SgHistoryGridVm();
            _reportFrequency = 1;
            _stopwatch = new Stopwatch();
            _scpRunnerVms = new ObservableCollection<ScpRunnerVm>(
                    configScpVms.Select
                    (
                        c => new ScpRunnerVm(
                            scpParams: c.ConfigScpParamVm.GetParams, 
                            sorterGenomeEvalVms:  c.SorterGenomeEvalGridVm.SorterGenomeEvalVms
                    ))
                );

            //var dict = _scpRunnerVms.Select(v=>v.OnIterationResult.Subscribe())
            foreach (var scpRunnerVm in ScpRunnerVms)
            {
                var disp = scpRunnerVm.OnIterationResult.Subscribe(ReportBestResult);
            }
        }

        void ReportBestResult(Tuple<string, int, string> result)
        {
            OnPropertyChanged("ProcTime");
            TrajectoryGridVm.SgHistoryVms.Add(new SgHistoryVm(result.Item1, result.Item2, result.Item3));
        }

        private readonly SgHistoryGridVm _trajectoryGridVm;
        public SgHistoryGridVm TrajectoryGridVm
        {
            get { return _trajectoryGridVm; }
        }

        private ObservableCollection<ScpRunnerVm> _scpRunnerVms;
        public ObservableCollection<ScpRunnerVm> ScpRunnerVms
        {
            get { return _scpRunnerVms; }
            set { _scpRunnerVms = value; }
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

            foreach (var scpRunnerVm in ScpRunnerVms)
            {
                await scpRunnerVm.OnRunAsync(_cancellationTokenSource);
            }

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

        private readonly Stopwatch _stopwatch;
        public string ProcTime
        {
            get { return _stopwatch.Elapsed.TotalSeconds.ToString("0"); }
        }

        private int? _reportFrequency;
        public int? ReportFrequency
        {
            get { return _reportFrequency; }
            set
            {
                _reportFrequency = value;
                foreach (var scpRunnerVm in ScpRunnerVms)
                {
                    scpRunnerVm.ReportFrequency = value;
                }
                CommandManager.InvalidateRequerySuggested();
                OnPropertyChanged("ReportFrequency");
            }
        }
    }
}
