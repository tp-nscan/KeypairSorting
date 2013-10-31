using System.Collections.Generic;
using System.Linq;

namespace Sorting.Sorters
{
    public class SorterTestResult<T>
    {
        private readonly ISorter _sorter;
        private readonly IReadOnlyList<int> _switchUseList;
        private readonly IReadOnlyList<ISortingResult<T>> _sortingResults;

        public SorterTestResult
            (
                ISorter sorter, 
                IReadOnlyList<ISortingResult<T>> sortingResults, 
                IReadOnlyList<int> switchUseList
            )
        {
            _sorter = sorter;
            _switchUseList = switchUseList;
            _useCount = switchUseList.Count(t => t > 0);
            _sortingResults = sortingResults;
        }

        public ISorter Sorter
        {
            get { return _sorter; }
        }

        public IReadOnlyList<int> SwitchUseList
        {
            get { return _switchUseList; }
        }

        public IReadOnlyList<ISortingResult<T>> SortingResults
        {
            get { return _sortingResults; }
        }

        private readonly int _useCount;
        public int UseCount
        {
            get { return _useCount; }
        }
    }
}
