using System.Collections.Generic;
using System.Linq;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.CompetePool
{
    public class SorterOnSwitchableGroup
    {
        private readonly ISorter _sorter;

        public SorterOnSwitchableGroup
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
