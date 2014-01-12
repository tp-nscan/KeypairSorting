using System;
using System.Collections.Immutable;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Entities.BackgroundWorkers;
using KeypairSorting.Resources;
using KeypairSorting.ViewModels.Parts;
using SorterEvo.Evals;
using SorterEvo.Genomes;
using SorterEvo.Json.Genomes;
using Sorting.Sorters;
using WpfUtils;

namespace KeypairSorting.ViewModels
{
    public class MakeSorterGenomeEvalsVm : ViewModelBase, IToolTemplateVm
    {
        public MakeSorterGenomeEvalsVm()
        {
            ReportFrequency = 50;
            _sorterGenomeEvalGridVm = new SorterGenomeEvalGridVm();
            _sorterGenomeEvalGridVm.SorterGenomeEvalVms.CollectionChanged 
                        += (s,e) => CommandManager.InvalidateRequerySuggested();
            _sorterGenomeGridVm = new SorterGenomeGridVm();
            _sorterGenomeGridVm.SorterGenomeVms.CollectionChanged
                        += (s, e) => CommandManager.InvalidateRequerySuggested();
        }

        public ToolTemplateType ToolTemplateType
        {
            get { return ToolTemplateType.SorterGenomeEval; }
        }

        public string Description
        {
            get { return "Evaluate Sorter Genomes"; }
        }


        private readonly SorterGenomeEvalGridVm _sorterGenomeEvalGridVm;
        public SorterGenomeEvalGridVm SorterGenomeEvalGridVm
        {
            get { return _sorterGenomeEvalGridVm; }
        }

        private readonly SorterGenomeGridVm _sorterGenomeGridVm;
        public SorterGenomeGridVm SorterGenomeGridVm
        {
            get { return _sorterGenomeGridVm; }
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

        private IEnumerativeBackgroundWorker<ISorterGenome, ISorterGenomeEval> _sorterBackgroundWorker;
        private IDisposable _updateSubscription;

        IEnumerativeBackgroundWorker<ISorterGenome, ISorterGenomeEval> SorterBackgroundWorker
        {
            get
            {
                if (_sorterBackgroundWorker == null)
                {

                    _sorterBackgroundWorker = EnumerativeBackgroundWorker.Make
                        (
                            inputs: SorterGenomeGridVm.SorterGenomeVms.Select(t => t.SorterGenomeJson.ToSorterGenome()),
                            mapper: (s, c) => 
                                IterationResult.Make
                                (
                                    data: SorterGenomeEval.Make(
                                        sorterGenome: s, 
                                        ancestors: ImmutableStack<Guid>.Empty, 
                                        sorterEval: s.ToSorter().FullTest()), 
                                    progressStatus: ProgressStatus.StepComplete
                                )
                        );

                    _updateSubscription = _sorterBackgroundWorker.OnIterationResult.Subscribe(UpdateSorterResults);
                }

                return _sorterBackgroundWorker;
            }
        }

        #region SorterGenomeEvalCommand

        RelayCommand _sorterGenomeEvalCommand;

        public ICommand SorterGenomeEvalCommand
        {
            get
            {
                return _sorterGenomeEvalCommand ?? (_sorterGenomeEvalCommand
                    = new RelayCommand
                        (
                            param => EvaluateSorters(),
                            param => CanEvaluateSorters()
                        ));
            }
        }

        bool CanEvaluateSorters()
        {
            return !_busy && ReportFrequency.HasValue && SorterGenomeGridVm.SorterGenomeVms.Count > 0;
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

        void UpdateSorterResults(IIterationResult<ISorterGenomeEval> result)
        {
            if (result.ProgressStatus == ProgressStatus.StepComplete)
            {
                SorterGenomeEvalGridVm.SorterGenomeEvalVms.Add(result.Data.ToSorterGenomeEvalVm());
            }
        }

        #endregion // SorterGenomeEvalCommand

        #region StopSorterGenomeEvalCommand

        RelayCommand _stopSorterGenomeEvalCommand;
        public ICommand StopSorterGenomeEvalCommand
        {
            get
            {
                return _stopSorterGenomeEvalCommand ?? (_stopSorterGenomeEvalCommand
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
            SorterGenomeEvalGridVm.SorterGenomeEvalVms.Clear();
        }

        bool CanResetCommand()
        {
            return !_busy && (_sorterBackgroundWorker != null);
        }

        #endregion // ResetCommand



    }
}
