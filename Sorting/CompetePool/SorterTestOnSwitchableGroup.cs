using System.Collections.Generic;
using System.Linq;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.CompetePool
{
    public class SorterTestOnSwitchableGroup
    {
        private readonly ISorter _sorter;

        public SorterTestOnSwitchableGroup
            (
                ISorter sorter,
                ISwitchableGroup switchableGroup,
                IReadOnlyList<int> switchUseList, 
                bool success)
        {
            _sorter = sorter;
            _switchUseList = switchUseList;
            _success = success;
            _switchableGroup = switchableGroup;
            _useCount = switchUseList.Count(t => t > 0);
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

        private readonly IReadOnlyList<int> _switchUseList;
        public IReadOnlyList<int> SwitchUseList
        {
            get { return _switchUseList; }
        }

        private readonly int _useCount;
        public int UseCount
        {
            get { return _useCount; }
        }
    }
}
