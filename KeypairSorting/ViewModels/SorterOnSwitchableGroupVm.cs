using System;
using Sorting.CompetePools;
using Sorting.Json.Sorters;
using WpfUtils;

namespace KeypairSorting.ViewModels
{
    public interface ISorterOnSwitchableGroupVm
    {
        int SwitchesUsed { get; }

        string SorterJson { get; }

        bool Success { get; }
    }

    public static class SorterOnSwitchableGroupVm
    {
        public static ISorterOnSwitchableGroupVm ToSorterOnSwitchableGroupVm(this ISorterOnSwitchableGroup sorterOnSwitchableGroup)
        {
            return new SorterOnSwitchableGroupVmImpl(sorterOnSwitchableGroup);
        }
    }

    class SorterOnSwitchableGroupVmImpl : ViewModelBase, ISorterOnSwitchableGroupVm
    {
        public SorterOnSwitchableGroupVmImpl(ISorterOnSwitchableGroup sorterOnSwitchableGroup)
        {
            SorterOnSwitchableGroup = sorterOnSwitchableGroup;
        }

        ISorterOnSwitchableGroup SorterOnSwitchableGroup { get; set; }

        public int SwitchesUsed { get { return SorterOnSwitchableGroup.SwitchesUsed; } }

        public string SorterJson
        {
            get
            {
                return (SorterOnSwitchableGroup.Sorter == null) ?
                    string.Empty :
                    SorterOnSwitchableGroup.Sorter.ToJsonString();
            }
        }

        public bool Success { get { return SorterOnSwitchableGroup.Success; } }

        public Guid SwitchableGroupGuid { get { return SorterOnSwitchableGroup.SwitchableGroupGuid; } }

    }
}
