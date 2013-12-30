using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Entities.BackgroundWorkers;
using KeypairSorting.Models;
using MathUtils;
using MathUtils.Collections;
using MathUtils.Rand;
using Sorting.CompetePools;
using WpfUtils;

namespace KeypairSorting.ViewModels
{
    public class SorterRandomSamplerVm : ViewModelBase
    {
        public SorterRandomSamplerVm()
        {
            KeyCount = 16;
            ReportFrequency = 100;
            LowRangeMax = 50;
            HighRangeMin = 70;
            Seed = 1234;
            for (var i = 0; i < 500; i++)
            {
                _switchUseHistoGram[i] = 0;
            }
        }

        private int? _keyCount;
        public int? KeyCount
        {
            get { return _keyCount; }
            set
            {
                _keyCount = value;
                OnPropertyChanged("KeyCount");
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

        private int? _lowRangeMax;
        public int? LowRangeMax
        {
            get { return _lowRangeMax; }
            set
            {
                _lowRangeMax = value;
                OnPropertyChanged("LowRangeMax");
            }
        }

        private int? _highRangeMin;
        public int? HighRangeMin
        {
            get { return _highRangeMin; }
            set
            {
                _highRangeMin = value;
                OnPropertyChanged("HighRangeMin");
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

        private int? _sortFails;
        public int? SortFails
        {
            get { return _sortFails; }
            set
            {
                _sortFails = value;
                OnPropertyChanged("SortFails");
            }
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

        #region RandGenCommand

        RelayCommand _randGenCommand;

        public ICommand RandGenCommand
        {
            get
            {
                return _randGenCommand ?? (_randGenCommand
                    = new RelayCommand
                        (
                            param => GenerateSamples(),
                            param => CanRandGenCommand()
                        ));
            }
        }

        bool CanRandGenCommand()
        {
            return !_busy && Seed.HasValue && ReportFrequency.HasValue && KeyCount.HasValue;
        }

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        readonly Stopwatch _stopwatch = new Stopwatch();
        async Task GenerateSamples()
        {
            Busy = true;
            var samplerParams =  GetSorterSamplerParams(KeyCount.Value);
            _stopwatch.Reset();
            _stopwatch.Start();

            _cancellationTokenSource = new CancellationTokenSource();
            var rando = Rando.Fast(Seed.Value);
            var inputs = Enumerable.Range(0, 10000).Select(t => rando.NextInt());

            IEnumerativeBackgroundWorker<int, SorterSamplerResults> ibw = EnumerativeBackgroundWorker.Make
                (
                    inputs: inputs,
                    mapper: (i, c) =>
                        {
                            var sorterSamplerResults = SorterRandomSampler.SorterSampler(
                                keyCount: KeyCount.Value,
                                switchCount: samplerParams.SwitchCount,
                                histogramMin: samplerParams.HistogramMin,
                                histogramMax: samplerParams.HistogramMax,
                                seed: i,
                                repCount: ReportFrequency.Value,
                                lowRangeMax: LowRangeMax.Value,
                                highRangeMin: HighRangeMin.Value,
                                cancellationToken: _cancellationTokenSource.Token
                                );

                            if (sorterSamplerResults.WasCancelled)
                            {
                                return IterationResult.Make(default(SorterSamplerResults), ProgressStatus.StepIncomplete);
                            }

                            return IterationResult.Make(sorterSamplerResults, ProgressStatus.StepComplete);
                        },
                    cancellationTokenSource: _cancellationTokenSource
                );

            ibw.OnIterationResult.Subscribe(UpdateSorterSamplerResults);
            await ibw.Start();
            Busy = false;
            _stopwatch.Stop();
            CommandManager.InvalidateRequerySuggested();
        }

        void UpdateSorterSamplerResults(IIterationResult<SorterSamplerResults> result)
        {
            if (result.Data == null)
            {
                return;
            }

            SorterOnSwitchableGroupVms.AddMany
                (
                    result.Data.SwitchResults.Select(r=>r.ToSorterOnSwitchableGroupVm())
                );

            _switchUseHistoGram.Merge(result.Data.SwitchUseHistogram, (a, b) => a + b);
            OnPropertyChanged("SizeDistributionReport");
            OnPropertyChanged("ProcTime");
            SortFails = (SortFails.HasValue) ? SortFails.Value + result.Data.SortFails : result.Data.SortFails;
        }

        private readonly Dictionary<int, int> _switchUseHistoGram = new Dictionary<int, int>();

        private ObservableCollection<ISorterOnSwitchableGroupVm> _sorterOnSwitchableGroupVms 
                = new ObservableCollection<ISorterOnSwitchableGroupVm>();
        public ObservableCollection<ISorterOnSwitchableGroupVm> SorterOnSwitchableGroupVms
        {
            get { return _sorterOnSwitchableGroupVms; }
            set { _sorterOnSwitchableGroupVms = value; }
        }

        #endregion // RandGenCommand


        #region StopRandGenCommand

        RelayCommand _stopRandGenCommand;
        public ICommand StopRandGenCommand
        {
            get
            {
                return _stopRandGenCommand ?? (_stopRandGenCommand
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
            _cancellationTokenSource.Cancel();
        }

        bool CanResetCommand()
        {
            return ! _busy;
        }

        #endregion // ResetCommand


        #region CopyGridCommand

        RelayCommand _copyGridCommand;
        public ICommand CopyGridCommand
        {
            get
            {
                return _copyGridCommand ?? (_copyGridCommand
                    = new RelayCommand
                        (
                            param => OnCopyGridCommand(),
                            param => CanCopyGridCommand()
                        ));
            }
        }

        protected void OnCopyGridCommand()
        {
            var sb = new StringBuilder();

            foreach (var sorterOnSwitchableGroupVm in SorterOnSwitchableGroupVms)
            {
                sb.AppendLine(
                                sorterOnSwitchableGroupVm.SwitchesUsed + "\t" + 
                                sorterOnSwitchableGroupVm.Success + "\t" +
                                sorterOnSwitchableGroupVm.SorterJson
                              );
            }

            Clipboard.SetText(sb.ToString());
        }

        bool CanCopyGridCommand()
        {
            return true;
        }

        #endregion // CopyGridCommand


        #region PasteGridCommand

        RelayCommand _pasteGridCommand;
        public ICommand PasteGridCommand
        {
            get
            {
                return _pasteGridCommand ?? (_pasteGridCommand
                    = new RelayCommand
                        (
                            param => OnPasteGridCommand(),
                            param => CanPasteGridCommand()
                        ));
            }
        }

        protected void OnPasteGridCommand()
        {
            var sb = new StringBuilder();

            foreach (var sorterOnSwitchableGroupVm in SorterOnSwitchableGroupVms)
            {
                sb.AppendLine(
                                sorterOnSwitchableGroupVm.SwitchesUsed + "\t" +
                                sorterOnSwitchableGroupVm.Success + "\t" +
                                sorterOnSwitchableGroupVm.SorterJson
                              );
            }

            Clipboard.SetText(sb.ToString());
        }

        bool CanPasteGridCommand()
        {
            return Clipboard.ContainsText();
        }

        #endregion // PasteGridCommand


        public string SizeDistributionReport
        {
            get
            {
                var samplerParams = GetSorterSamplerParams(KeyCount.Value);

                return Enumerable.Range(samplerParams.HistogramMin, samplerParams.HistogramMax - samplerParams.HistogramMin)
                    .Select(c => new Tuple<int, int>(c, _switchUseHistoGram[c]))
                    .Aggregate(String.Empty, (o, n) => o + "\n" + n.Item1 + "\t" + n.Item2);
            }
        }

        private string _procTime;
        public string ProcTime
        {
            get { return _stopwatch.Elapsed.TotalSeconds.ToString("0"); }
        }

        static SorterSamplerParams GetSorterSamplerParams(int keyCount)
        {
            if (keyCount == 5)
            {
                return new SorterSamplerParams { KeyCount = 5, HistogramMin = 7, HistogramMax = 11, SwitchCount = 400 };
            }
            if (keyCount == 6)
            {
                return new SorterSamplerParams { KeyCount = 6, HistogramMin = 11, HistogramMax = 16, SwitchCount = 500 };
            }
            if (keyCount == 7)
            {
                return new SorterSamplerParams { KeyCount = 7, HistogramMin = 16, HistogramMax = 22, SwitchCount = 600 };
            }
            if (keyCount == 8)
            {
                return new SorterSamplerParams { KeyCount = 8, HistogramMin = 19, HistogramMax = 30, SwitchCount = 700 };
            }
            if (keyCount == 9)
            {
                return new SorterSamplerParams { KeyCount = 9, HistogramMin = 25, HistogramMax = 38, SwitchCount = 900 };
            }
            if (keyCount == 10)
            {
                return new SorterSamplerParams { KeyCount = 10, HistogramMin = 30, HistogramMax = 49, SwitchCount = 900 };
            }
            if (keyCount == 11)
            {
                return new SorterSamplerParams { KeyCount = 11, HistogramMin = 38, HistogramMax = 59, SwitchCount = 1000 };
            }
            if (keyCount == 12)
            {
                return new SorterSamplerParams { KeyCount = 12, HistogramMin = 46, HistogramMax = 70, SwitchCount = 1200 };
            }
            if (keyCount == 13)
            {
                return new SorterSamplerParams { KeyCount = 13, HistogramMin = 54, HistogramMax = 81, SwitchCount = 1400 };
            }
            if (keyCount == 14)
            {
                return new SorterSamplerParams { KeyCount = 14, HistogramMin = 65, HistogramMax = 94, SwitchCount = 1600 };
            }
            if (keyCount == 15)
            {
                return new SorterSamplerParams { KeyCount = 15, HistogramMin = 75, HistogramMax = 109, SwitchCount = 2000 };
            }
            if (keyCount == 16)
            {
                return new SorterSamplerParams { KeyCount = 16, HistogramMin = 85, HistogramMax = 119, SwitchCount = 2200 };
            }
            if (keyCount == 17)
            {
                return new SorterSamplerParams { KeyCount = 17, HistogramMin = 95, HistogramMax = 148, SwitchCount = 2400 };
            }
            if (keyCount == 18)
            {
                return new SorterSamplerParams { KeyCount = 18, HistogramMin = 105, HistogramMax = 162, SwitchCount = 2600 };
            }
            if (keyCount == 19)
            {
                return new SorterSamplerParams { KeyCount = 19, HistogramMin = 118, HistogramMax = 174, SwitchCount = 3000 };
            }
            if (keyCount == 20)
            {
                return new SorterSamplerParams { KeyCount = 20, HistogramMin = 132, HistogramMax = 184, SwitchCount = 3200 };
            }
            if (keyCount == 24)
            {
                return new SorterSamplerParams { KeyCount = 24, HistogramMin = 125, HistogramMax = 7, SwitchCount = 4000 };
            }
            if (keyCount == 32)
            {
                return new SorterSamplerParams { KeyCount = 32, HistogramMin = 175, HistogramMax = 500, SwitchCount = 8000 };
            }

            throw new Exception("Keycount:" + keyCount + " not handled");
        }

    }

    class SorterSamplerParams
    {
        public int KeyCount { get; set; }
        public int SwitchCount { get; set; }
        public int HistogramMin { get; set; }
        public int HistogramMax { get; set; }
    }
}
