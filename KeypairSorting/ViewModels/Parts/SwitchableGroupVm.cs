using System;
using Sorting.Json.Switchables;
using Sorting.Switchables;
using WpfUtils;

namespace KeypairSorting.ViewModels.Parts
{
    public class SwitchableGroupVm : ViewModelBase
    {
        public SwitchableGroupVm(ISwitchableGroup switchableGroup)
        {
            _switchableGroup = switchableGroup;
        }

        private readonly ISwitchableGroup _switchableGroup;

        public string SwitchableGroupJson
        {
            get { return _switchableGroup.ToJsonString(); }
        }

        public int GroupSize
        {
            get { return _switchableGroup.SwitchableCount; }
        }

        public Guid Guid
        {
            get { return _switchableGroup.Guid; }
        }
    }
     
    public static class SwitchableGroupVmEx
    {
        public static SwitchableGroupVm ToSwitchableGroupVm(this ISwitchableGroup switchableGroup)
        {
            return new SwitchableGroupVm(switchableGroup);
        }
    }
}
