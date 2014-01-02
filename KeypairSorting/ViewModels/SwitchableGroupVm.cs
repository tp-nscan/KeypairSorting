using Sorting.Json.Switchables;
using Sorting.Switchables;
using WpfUtils;

namespace KeypairSorting.ViewModels
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
    }

    public static class SwitchableGroupVmEx
    {
        public static SwitchableGroupVm ToSwitchableGroupVm(this ISwitchableGroup switchableGroup)
        {
            return new SwitchableGroupVm(switchableGroup);
        }
    }
}
