using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.CompetePool
{
    public interface ISorterOnSwitchableGroupSet
    {
        IEnumerable<ISorterOnSwitchableGroup> SorterOnSwitchableGroups { get; }
        ISorterOnSwitchableGroup SorterOnSwitchableGroup(ISwitchableGroup switchableGroup);
        ISorter Sorter { get; }
        IReadOnlyList<double> SwitchUseList { get; }
        int SwitchesUsed { get; }
    }

    public static class SorterOnSwitchableGroupSet
    {

        public static ISorterOnSwitchableGroupSet MakeSorterOnSwitchableGroups<T>
        (
            this ISorter sorter,
            IEnumerable<ISwitchableGroup<T>> switchableGroups
        )
        {
            return new SorterOnSwitchableGroupSetImpl
                (
                    sorter: sorter,
                    sorterOnSwitchableGroups: switchableGroups.Select(sorter.Sort)
                );
        }

        public static int UsedKeyPairHash(this ISorterEval sorterEval)
        {
            //return sorterEval.Sorter.KeyPairs.Filter(i => sorterEval.SwitchUseList[i] > 0).ToHash(k => k.Index);
            return sorterEval.Sorter.KeyPairs.Where((t,i) => sorterEval.SwitchUseList[i] > 0).ToHash(k => k.Index);
        }
    }

    public class SorterOnSwitchableGroupSetImpl : ISorterOnSwitchableGroupSet
    {
        public SorterOnSwitchableGroupSetImpl
        (
            ISorter sorter,
            IEnumerable<ISorterOnSwitchableGroup> sorterOnSwitchableGroups
        )
        {
            _sorter = sorter;
            _sorterOnSwitchableGroups = sorterOnSwitchableGroups.ToDictionary(t => t.SwitchableGroup.Guid);
            _switchUseList = _sorterOnSwitchableGroups.Values.Select(T => T.SwitchUseList).VectorSumDouble();
            _switchesUsed = SwitchUseList.Count(T => T > 0);
        }

        private readonly ISorter _sorter;
        public ISorter Sorter
        {
            get { return _sorter; }
        }

        private readonly Dictionary<Guid, ISorterOnSwitchableGroup> _sorterOnSwitchableGroups;

        public ISorterOnSwitchableGroup SorterOnSwitchableGroup(ISwitchableGroup switchableGroup)
        {
            return _sorterOnSwitchableGroups[switchableGroup.Guid];
        }

        public IEnumerable<ISorterOnSwitchableGroup> SorterOnSwitchableGroups
        {
            get { return _sorterOnSwitchableGroups.Values; }
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
