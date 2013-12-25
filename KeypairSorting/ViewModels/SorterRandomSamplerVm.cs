using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using KeypairSorting.Models;
using MathUtils.Collections;
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
            //Range1Min = 10;
            Range1Max = 50;
            Range2Min = 70;
            //Range2Max = 1000;
            Seed = 1234;
        }

        private int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
            set
            {
                _keyCount = value;
                OnPropertyChanged("KeyCount");
            }
        }

        private int _reportFrequency;
        public int ReportFrequency
        {
            get { return _reportFrequency; }
            set
            {
                _reportFrequency = value;
                OnPropertyChanged("ReportFrequency");
            }
        }

        //private int _range1Min;
        //public int Range1Min
        //{
        //    get { return _range1Min; }
        //    set
        //    {
        //        _range1Min = value;
        //        OnPropertyChanged("Range1Min");
        //    }
        //}

        private int _range1Max;
        public int Range1Max
        {
            get { return _range1Max; }
            set
            {
                _range1Max = value;
                OnPropertyChanged("Range1Max");
            }
        }

        private int _range2Min;
        public int Range2Min
        {
            get { return _range2Min; }
            set
            {
                _range2Min = value;
                OnPropertyChanged("Range2Min");
            }
        }

        //private int _range2Max;
        //public int Range2Max
        //{
        //    get { return _range2Max; }
        //    set
        //    {
        //        _range2Max = value;
        //        OnPropertyChanged("Range2Max");
        //    }
        //}

        private int _seed;
        public int Seed
        {
            get { return _seed; }
            set
            {
                _seed = value;
                OnPropertyChanged("Seed");
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
                            param => TryThis(),
                            param => CanRandGenCommand()
                        ));
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

        protected void OnRandGenCommand()
        {
           TryThis();
        }

        bool CanRandGenCommand()
        {
            return !_busy;
        }

        async Task TryThis()
        {
            Busy = true;
            await Task.Run(() => Proc());
            Busy = false;
            CommandManager.InvalidateRequerySuggested();
        }

        private Dictionary<int, int> _switchUseHistoGram;
        private List<ISorterOnSwitchableGroup> _sorterOnSwitchableGroups;
        private bool _continue;
        void Proc()
        {
            SizeDistributionReport = string.Empty;
            SelectedSwitchReport = string.Empty;

            _switchUseHistoGram = new Dictionary<int, int>();
            for (var i = 0; i < 500; i++)
            {
                _switchUseHistoGram[i] = 0;
            }

            _sorterOnSwitchableGroups = new List<ISorterOnSwitchableGroup>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            _continue = true;
            while (_continue)
            {
                var results = SorterRandomSampler.SorterSampler(KeyCount, Seed, ReportFrequency);
                _switchUseHistoGram.Merge(results.SwitchUseHistogram, (a, b) => a + b);
                _sorterOnSwitchableGroups.AddRange(results.SwitchResults);

                OnPropertyChanged("SizeDistributionReport");
                OnPropertyChanged("SelectedSwitchReport");

                ProcTime = stopwatch.Elapsed.TotalSeconds.ToString("0");
            }
            stopwatch.Stop();
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
            _continue = false;
        }

        bool CanStopRandGenCommand()
        {
            return _busy;
        }

        #endregion // StopRandGenCommand

        private string _sizeDistributionReport;
        public string SizeDistributionReport
        {
            get { return _sizeDistributionReport; }
            set
            {
                _sizeDistributionReport = value;
                OnPropertyChanged("SizeDistributionReport");
            }
        }

        private string _selectedSwitchReport;
        public string SelectedSwitchReport
        {
            get { return _selectedSwitchReport; }
            set
            {
                _selectedSwitchReport = value;
                OnPropertyChanged("SelectedSwitchReport");
            }
        }

        private string _procTime;
        public string ProcTime
        {
            get { return _procTime; }
            set
            {
                _procTime = value;
                OnPropertyChanged("ProcTime");
            }
        }
    }


}
