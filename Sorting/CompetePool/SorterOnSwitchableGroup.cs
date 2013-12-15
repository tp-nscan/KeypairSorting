using System.Collections.Generic;
using System.Linq;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.CompetePool
{
    public interface ISorterEval
    {
        ISorter Sorter { get; }
        IReadOnlyList<double> SwitchUseList { get; }
        int SwitchesUsed { get; }
    }


    public interface ISorterOnSwitchableGroup : ISorterEval
    {
        ISwitchableGroup SwitchableGroup { get; }
        bool Success { get; }
    }

    public static class SorterOnSwitchableGroup
    {
        public static ISorterOnSwitchableGroup Make
            (
                ISorter sorter,
                ISwitchableGroup switchableGroup,
                IReadOnlyList<double> switchUseList, 
                bool success
            )
        {
            return new SorterOnSwitchableGroupImpl(
                    sorter: sorter,
                    switchableGroup: switchableGroup,
                    switchUseList: switchUseList, 
                    success: success
                );    
        }
    }

    public class SorterOnSwitchableGroupImpl : ISorterOnSwitchableGroup
    {
        private readonly ISorter _sorter;

        public SorterOnSwitchableGroupImpl
        (
            ISorter sorter,
            ISwitchableGroup switchableGroup,
            IReadOnlyList<double> switchUseList, 
            bool success
        )
        {
            _sorter = sorter;
            _switchUseList = switchUseList;
            _success = success;
            _switchableGroup = switchableGroup;
            _switchesUsed = switchUseList.Count(t => t > 0);
        }

        private readonly ISwitchableGroup _switchableGroup;
        public ISwitchableGroup SwitchableGroup
        {
            get { return _switchableGroup; }
        }

        public ISorter Sorter
        {
            get { return _sorter; }
        }

        private readonly bool _success;
        public bool Success
        {
            get { return _success; }
        }

        private readonly IReadOnlyList<double> _switchUseList;
        public IReadOnlyList<double> SwitchUseList
        {
            get { return _switchUseList; }
        }

        private readonly int _switchesUsed;
        public int SwitchesUsed
        {
            get { return _switchesUsed; }
        }
    }
}
