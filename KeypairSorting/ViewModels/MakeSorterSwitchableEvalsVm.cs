using System;
using System.Collections.Generic;
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
using Sorting.Json.Switchables;
using Sorting.Sorters;
using Sorting.Switchables;
using Sorting.SwitchFunctionSets;
using WpfUtils;

namespace KeypairSorting.ViewModels
{
    public class MakeSorterSwitchableEvalsVm : ViewModelBase, IToolTemplateVm
    {
        public MakeSorterSwitchableEvalsVm()
        {
            ReportFrequency = 50;
            _sorterEvalGridVm = new SorterEvalsGridVm();
            _sorterGridVm = new SorterGridVm();
            _switchableGroupGridVm = new SwitchableGroupGridVm();
        }

        public ToolTemplateType ToolTemplateType
        {
            get { return ToolTemplateType.SorterSwitchableEval; }
        }

        public string Description
        {
            get { return "Test Sorters against Switchable groups"; }
        }

        private readonly SorterEvalsGridVm _sorterEvalGridVm;
        public SorterEvalsGridVm SorterEvalGridVm
        {
            get { return _sorterEvalGridVm; }
        }

        private readonly SorterGridVm _sorterGridVm;
        public SorterGridVm SorterGridVm
        {
            get { return _sorterGridVm; }
        }

        private readonly SwitchableGroupGridVm _switchableGroupGridVm;
        public SwitchableGroupGridVm SwitchableGroupGridVm
        {
            get { return _switchableGroupGridVm; }
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

        private IEnumerativeBackgroundWorker<ISorter, IEnumerable<ISorterEval>> _sorterBackgroundWorker;
        private IDisposable _updateSubscription;

        IEnumerativeBackgroundWorker<ISorter, IEnumerable<ISorterEval>> SorterBackgroundWorker
        {
            get
            {
                if (_sorterBackgroundWorker == null)
                {

                    _sorterBackgroundWorker = EnumerativeBackgroundWorker.Make
                        (
                            inputs: SorterGridVm.SorterVms.Select(t => t.SorterJson.ToSorter()),
                            mapper: (s, c) => IterationResult.Make(SorterOnSwitchableGroupTests(s), ProgressStatus.StepComplete)
                        );

                    _updateSubscription = _sorterBackgroundWorker.OnIterationResult.Subscribe(UpdateSorterResults);
                }

                return _sorterBackgroundWorker;
            }
        }

        IEnumerable<ISorterEval> SorterOnSwitchableGroupTests(ISorter sorter)
        {
            KeyPairSwitchSet.Make<uint>(sorter.KeyCount);

            foreach (var switchableGroupResult in SwitchableGroupGridVm.SwitchableGroupVms.Select(v => v.SwitchableGroupJson.ToSwitchableGroup())
                .AsParallel().Select(g => sorter.Sort((ISwitchableGroup<uint>)g)))
            {
                yield return switchableGroupResult;
            }

            //SwitchableGroupGridVm.SwitchableGroupVms.Select(v => v.SwitchableGroupJson.ToSwitchableGroup())
            //    .AsParallel().Select(g => sorter.Sort((ISwitchableGroup<uint>)g));

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

        void UpdateSorterResults(IIterationResult<IEnumerable<ISorterEval>> results)
        {
            if (results.ProgressStatus == ProgressStatus.StepComplete)
            {
                SorterEvalGridVm.SorterEvalVms
                    .AddMany(results.Data.Select(r=>r.ToSorterEvalVm()));
            }
            OnPropertyChanged("ProcTime");
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

        private string _procTime;
        public string ProcTime
        {
            get { return _stopwatch.Elapsed.TotalSeconds.ToString("0"); }
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
            SorterEvalGridVm.SorterEvalVms.Clear();
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
