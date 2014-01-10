using System;
using Sorting.CompetePools;
using Sorting.Json.Sorters;
using WpfUtils;

namespace KeypairSorting.ViewModels.Parts
{
    public interface ISorterEvalVm
    {
        int SwitchesUsed { get; }
        Guid SwitchableGroupGuid { get; }
        bool Success { get; }
        string SorterJson { get; }
        Guid SorterGuid { get; }
    }

    public static class SorterEvalVm
    {
        public static ISorterEvalVm ToSorterEvalVm(this ISorterEval sorterEval)
        {
            return new SorterEvalVmImpl(sorterEval);
        }
    }

    class SorterEvalVmImpl : ViewModelBase, ISorterEvalVm
    {
        public SorterEvalVmImpl(ISorterEval sorterEval)
        {
            SorterEval = sorterEval;
        }

        ISorterEval SorterEval { get; set; }

        public int SwitchesUsed { get { return SorterEval.SwitchesUsed; } }

        public string SorterJson
        {
            get
            {
                return (SorterEval.Sorter == null) ?
                    string.Empty :
                    SorterEval.Sorter.ToJsonString();
            }
        }

        public Guid SorterGuid
        {
            get { return SorterEval.Sorter.Guid; }
        }

        public bool Success { get { return SorterEval.Success; } }

        public Guid SwitchableGroupGuid { get { return SorterEval.SwitchableGroupGuid; } }

    }
}
