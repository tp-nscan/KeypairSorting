using System.Linq;
using Sorting.CompetePools;
using Sorting.Switchables;
using Sorting.SwitchFunctionSets;

namespace Sorting.Sorters
{
    public static class SortingFunctions
    {
        public static ISorterEval Sort<T>
            (
                this ISorter sorter,
                ISwitchableGroup<T> switchableGroup
            )
        {
            var switchUseList = Enumerable.Repeat(0.0, sorter.KeyPairCount).ToList();
            var totalSuccess = true;
            var switchSet = KeyPairSwitchSet.Make<T>(switchableGroup.KeyCount);

            foreach (var switchable in switchableGroup.Switchables)
            {
                var current = switchable.Item;
                var sortSuccess = false;

                for (var i = 0; i < sorter.KeyPairCount; i++)
                {
                    if (switchSet.IsSorted(current))
                    {
                        sortSuccess = true;
                        break;
                    }

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

            return SorterEval.Make(
                sorter: sorter,
                switchableGroupGuid: switchableGroup.Guid,
                success: totalSuccess,
                switchUseList: switchUseList,
                coveringSet: null
                );
        }

        public static ISorterEval FullTest(this ISorter sorter)
        {
            return sorter.Sort(Switchable.AllSwitchablesForKeyCount(sorter.KeyCount)
                .ToSwitchableGroup(
                guid: SwitchableGroup.GuidOfAllSwitchableGroupsForKeyCount(sorter.KeyCount), 
                keyCount:sorter.KeyCount));
        }

    }
}
