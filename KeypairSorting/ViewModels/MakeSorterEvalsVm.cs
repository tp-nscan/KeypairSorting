using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Entities.BackgroundWorkers;
using KeypairSorting.Resources;
using KeypairSorting.ViewModels.Parts;
using Sorting.CompetePools;
using Sorting.Json.Sorters;
using Sorting.Sorters;
using WpfUtils;

namespace KeypairSorting.ViewModels
{
    public class MakeSorterEvalsVm : ViewModelBase, IToolTemplateVm
    {
        public MakeSorterEvalsVm()
        {
            ReportFrequency = 50;
            _sorterEvalGridVm = new SorterEvalGridVm();
            _sorterGridVm = new SorterGridVm();
        }

        public ToolTemplateType ToolTemplateType
        {
            get { return ToolTemplateType.SorterEval; }
        }

        public string Description
        {
            get { return "Evaluate Sorters"; }
        }

        private readonly SorterEvalGridVm _sorterEvalGridVm;
        public SorterEvalGridVm SorterEvalGridVm
        {
            get { return _sorterEvalGridVm; }
        }

        private readonly SorterGridVm _sorterGridVm;
        public SorterGridVm SorterGridVm
        {
            get { return _sorterGridVm; }
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

        private IEnumerativeBackgroundWorker<ISorter, ISorterOnSwitchableGroup> _sorterBackgroundWorker;
        private IDisposable _updateSubscription;
         
        IEnumerativeBackgroundWorker<ISorter, ISorterOnSwitchableGroup> SorterBackgroundWorker
        {
            get
            {
                if (_sorterBackgroundWorker == null)
                {

                    _sorterBackgroundWorker = EnumerativeBackgroundWorker.Make
                        (
                            inputs: SorterGridVm.SorterVms.Select(t => t.SorterJson.ToSorter()),
                            mapper: (s, c) => IterationResult.Make(s.FullTest(), ProgressStatus.StepComplete)
                        );

                    _updateSubscription = _sorterBackgroundWorker.OnIterationResult.Subscribe(UpdateSorterResults);
                }

                return _sorterBackgroundWorker;
            }
        }

       #region SorterEvalCommand

        RelayCommand _sorterEvalCommand;

        public ICommand SorterEvalCommand
        {
            get
            {
                return _sorterEvalCommand ?? (_sorterEvalCommand
                    = new RelayCommand
                        (
                            param => EvaluateSorters(),
                            param => CanRandGenCommand()
                        ));
            }
        }

        bool CanRandGenCommand()
        {
            return !_busy && ReportFrequency.HasValue && SorterGridVm.SorterVms.Count > 0;
        }

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        readonly Stopwatch _stopwatch = new Stopwatch();
        async Task EvaluateSorters()
        {
            Busy = true;
            _stopwatch.Reset();
            _stopwatch.Start();

            _cancellationTokenSource = new CancellationTokenSource();
            await SorterBackgroundWorker.Start(_cancellationTokenSource);
            Busy = false;
            _stopwatch.Stop();
            CommandManager.InvalidateRequerySuggested();
        }

        void UpdateSorterResults(IIterationResult<ISorterOnSwitchableGroup> result)
        {
            if (result.ProgressStatus == ProgressStatus.StepComplete)
            {
                SorterEvalGridVm.SorterOnSwitchableGroupVms.Add(result.Data.ToSorterOnSwitchableGroupVm());
            }
        }

        #endregion // RandGenCommand

        #region StopSorterEvalCommand

        RelayCommand _stopSorterEvalCommand;
        public ICommand StopSorterEvalCommand
        {
            get
            {
                return _stopSorterEvalCommand ?? (_stopSorterEvalCommand
                    = new RelayCommand
                        (
                            param => OnStopRandGenCommand(),
                            param => CanStopRandGenCommand()
                        ));
            }
        }

        protected void OnStopRandGenCommand()
        {
            _cancellationTokenSource.Cancel();
        }

        bool CanStopRandGenCommand()
        {
            return _busy;
        }

        #endregion // StopRandGenCommand

        #region ResetCommand

        RelayCommand _resetCommand;
        public ICommand ResetCommand
        {
            get
            {
                return _resetCommand ?? (_resetCommand
                    = new RelayCommand
                        (
                            param => OnResetCommand(),
                            param => CanResetCommand()
                        ));
            }
        }

        protected void OnResetCommand()
        {
            _updateSubscription.Dispose();
            _sorterBackgroundWorker = null;
            SorterEvalGridVm.SorterOnSwitchableGroupVms.Clear();
        }

        bool CanResetCommand()
        {
            return !_busy && (_sorterBackgroundWorker != null);
        }

        #endregion // ResetCommand


        private int? _reportFrequency;
        public int? ReportFrequency
        {
            get { return _reportFrequency; }
            set
            {
                _reportFrequency = value;
                OnPropertyChanged("ReportFrequency");
            }
        }
    }
}
