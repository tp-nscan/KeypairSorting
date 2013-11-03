using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.CompetePool
{
    public interface ISorterTestOnCompPool
    {
        ISorter Sorter { get; }
        IReadOnlyList<int> SwitchUseList { get; }
        int SwitchesUsed { get; }
        SorterTestOnSwitchableGroup SorterTestOnSwitchableGroup(ISwitchableGroup switchableGroup);
    }

    public static class SorterTestOnCompPool
    {
        public static ISorterTestOnCompPool MakeSorterTestOnCompPool<T>
                (
                    this ISorter sorter, 
                    IEnumerable<ISwitchableGroup<T>> switchableGroups
                )
        {
            return new SorterTestOnCompPoolImpl
                (
                    sorter: sorter,
                    sorterTestOnSwitchableGroups: switchableGroups.Select(sorter.Sort)
                );
        }
    }

    public class SorterTestOnCompPoolImpl : ISorterTestOnCompPool
    {
        public SorterTestOnCompPoolImpl
        (
            ISorter sorter, 
            IEnumerable<SorterTestOnSwitchableGroup> sorterTestOnSwitchableGroups
        )
        {
            _sorter = sorter;
            _sorterTestOnSwitchableGroups = sorterTestOnSwitchableGroups.ToDictionary(t => t.SwitchableGroup.Guid);
            _switchUseList = _sorterTestOnSwitchableGroups.Values.Select(T=>T.SwitchUseList).VectorSumInts();
            _switchesUsed = SwitchUseList.Count(T => T > 0);
        }

        private readonly ISorter _sorter;
        public ISorter Sorter
        {
            get { return _sorter; }
        }

        private readonly Dictionary<Guid, SorterTestOnSwitchableGroup> _sorterTestOnSwitchableGroups;

        public SorterTestOnSwitchableGroup SorterTestOnSwitchableGroup(ISwitchableGroup switchableGroup)
        {
            return _sorterTestOnSwitchableGroups[switchableGroup.Guid];
        }

        private readonly IReadOnlyList<int> _switchUseList;
        public IReadOnlyList<int> SwitchUseList
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
