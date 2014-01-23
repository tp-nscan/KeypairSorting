using System;
using System.Linq;
using System.Windows.Input;
using KeypairSorting.Resources;
using KeypairSorting.ViewModels.Parts;
using MathUtils.Rand;
using WpfUtils;
using Sorting.Switchables;

namespace KeypairSorting.ViewModels
{
    public class MakeRandomSwitchablesVm : ViewModelBase, IToolTemplateVm
    {
        public ToolTemplateType ToolTemplateType
        {
            get { return ToolTemplateType.SwitchableGen; }
        }

        public string Description
        {
            get { return "Make random switchables"; }
        }

        private int? _groupCount;
        public int? GroupCount
        {
            get { return _groupCount; }
            set
            {
                _groupCount = value;
                OnPropertyChanged("GroupCount");
            }
        }

        private int? _groupSize;
        public int? GroupSize
        {
            get { return _groupSize; }
            set
            {
                _groupSize = value;
                OnPropertyChanged("GroupSize");
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
                            param => GenerateSwitchableGroups(),
                            param => CanGenerateSwitchableGroups()
                        ));
            }
        }

        private void GenerateSwitchableGroups()
        {
            var randoK = Rando.Fast(Seed.Value);
            var randoG = Rando.Fast(Seed.Value * 377);

            for (int i = 1; i < 41; i++)
            {
                foreach (var switchableGroupVm in randoK.ToRandomEnumerator().Take(GroupCount.Value)
                    .Select(r => r.ToSwitchableGroup<uint>(randoG.NextGuid(), KeyCount.Value, GroupSize.Value * i))
                    .Select(s => s.ToSwitchableGroupVm()))
                {
                    SwitchableGroupGridVm.SwitchableGroupVms.Add(switchableGroupVm);
                }
            }

        }

        bool CanGenerateSwitchableGroups()
        {
            return !_busy && Seed.HasValue && GroupSize.HasValue && KeyCount.HasValue && GroupCount.HasValue;
        }


        #endregion // RandGenCommand

        #region ResetCommand

        RelayCommand _resetCommand;
        public ICommand ResetCommand
        {
            get
            {
                return _resetCommand ?? (_resetCommand
                    = new RelayCommand
                        (
                            param => ResetSwitchableGroups(),
                            param => CanResetSwitchableGroups()
                        ));
            }
        }

        private void ResetSwitchableGroups()
        {
            SwitchableGroupGridVm.SwitchableGroupVms.Clear();
        }

        bool CanResetSwitchableGroups()
        {
            return true;
        }


        #endregion // RandGenCommand

        private readonly SwitchableGroupGridVm _switchableGroupGridVm = new SwitchableGroupGridVm();
        public SwitchableGroupGridVm SwitchableGroupGridVm
        {
            get { return _switchableGroupGridVm; }
        }
    }
}
