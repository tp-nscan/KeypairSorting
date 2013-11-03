using System.Collections.Generic;
using System.Linq;
using Sorting.CompetePool;
using Sorting.Switchables;
using Sorting.SwitchFunctionSets;

namespace Sorting.Sorters
{
    public static class SortingFunctions
    {
        public static SorterTestOnSwitchableGroup Sort<T>
            (
                this ISorter sorter,
                ISwitchableGroup<T> switchableGroup
            )
        {
            var switchUseList = Enumerable.Repeat(0, sorter.KeyPairCount).ToList();
            var totalSuccess = true;
            var switchSet = KeyPairSwitchSet.Make<T>(switchableGroup.KeyCount);

            foreach (var switchable in switchableGroup.Switchables)
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

                totalSuccess &= sortSuccess;
            }

            return new SorterTestOnSwitchableGroup(sorter, switchableGroup, switchUseList, totalSuccess);
        }

        public static SorterTestOnSwitchableGroup SortDetailed<T>
            (
                this ISorter sorter,
                IKeyPairSwitchSet<T> switchSet,
                ISwitchableGroup<T> switchableGroup
            )
        {
            var switchUseList = Enumerable.Repeat(0, sorter.KeyPairCount).ToList();
            var sortingResults = new List<ISortingResult<T>>();

            foreach (var switchable in switchableGroup.Switchables)
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
            //to do: fix last arg
            return new SorterTestOnSwitchableGroup(sorter, switchableGroup, switchUseList, true);
        }
    }
}
