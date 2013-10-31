using System.Collections.Generic;
using System.Linq;
using Sorting.Switchables;
using Sorting.SwitchFunctionSets;

namespace Sorting.Sorters
{
    public static class SortingFunctions
    {
        //public static List<int> Sort<T>
        //    (
        //        this ISorter sorter, 
        //        IKeyPairSwitchSet<T> switchSet, 
        //        ISortingResult<T> switchableSortingRun, 
        //        List<int> switchUseList
        //    )
        //{
        //    var current = switchableSortingRun.Switchable.Item;
        //    for (var i = 0; i < sorter.KeyPairCount; i++)
        //    {
        //        var res = switchSet.SwitchFunction(sorter.KeyPair(i))(current);
        //        current = res.Item1;
        //        if (res.Item2)
        //        {
        //            switchUseList[i]++;
        //            if (switchSet.IsSorted(current))
        //            {
        //                switchableSortingRun.Result = res.Item1;
        //                return switchUseList;
        //            }
        //        }
        //    }
        //    switchableSortingRun.Result = current;
        //    return switchUseList;
        //}

        public static SorterTestResult<T> Sort<T>
            (
                this ISorter sorter,
                IKeyPairSwitchSet<T> switchSet,
                IEnumerable<ISwitchable<T>> switchables
            )
        {
            var switchUseList = Enumerable.Repeat(0, sorter.KeyPairCount).ToList();
            var sortingResults = new List<ISortingResult<T>>();

            foreach (var switchable in switchables)
            {
                var current = switchable.Item;
                var sortSuccess = false;
                for (var i = 0; i < sorter.KeyPairCount; i++)
                {
                    var res = switchSet.SwitchFunction(sorter.KeyPair(i))(current);
                    current = res.Item1;
                    if (res.Item2)
                    {
                        switchUseList[i]++;
                        if (switchSet.IsSorted(current))
                        {
                            sortSuccess = true;
                            break;
                        }
                    }
                }
                sortingResults.Add(new SortingResultImpl<T>(switchable, current, sortSuccess));
            }
            return new SorterTestResult<T>(sorter, sortingResults, switchUseList);
        }

        //public static IReadOnlyList<int> SortMany<T>
        //(
        //    this ISorter sorter, 
        //    IKeyPairSwitchSet<T> switchSet, 
        //    IReadOnlyList<ISortingResult<T>> switchableSortingRuns, 
        //    List<int> switchUseList
        //)
        //{
        //    foreach (var switchableResult in switchableSortingRuns)
        //    {
        //        switchUseList = sorter.Sort(switchSet, switchableResult, switchUseList);
        //    }

        //    return switchUseList;

        //    //return switchables.Aggregate(switchUseList, (current, SortingResultImpl) => sorter.Sort(switchSet, SortingResultImpl, current));
        //}

        //public static SorterTestResult<T> TestWith<T>(this ISorter sorter, IEnumerable<ISwitchable<T>> switchables)
        //{

        //    var switchableSortingRuns = switchables.Select(t => t.ToSwitchableSortingRun()).ToList();

        //    var switchUseList = SortMany
        //        (
        //            sorter : sorter,
        //            switchSet: KeyPairSwitchSet.Make<T>(switchableSortingRuns[0].Switchable.KeyCount), 
        //            switchableSortingRuns: switchableSortingRuns,
        //            switchUseList: Enumerable.Repeat(0, sorter.KeyPairCount).ToList()
        //        );

        //    return new SorterTestResult<T>(sorter, switchableSortingRuns, switchUseList);
        //}
    }
}
