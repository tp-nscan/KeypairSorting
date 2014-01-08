using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.CompetePools
{
    public interface ISorterEvalSet
    {
        IEnumerable<ISorterEval> SorterOnSwitchableGroups { get; }
        ISorterEval SorterOnSwitchableGroup(ISwitchableGroup switchableGroup);
        ISorter Sorter { get; }
        IReadOnlyList<double> SwitchUseList { get; }
        int SwitchesUsed { get; }
    }

    public static class SorterEvalSet
    {

        public static ISorterEvalSet MakeSorterOnSwitchableGroups<T>
        (
            this ISorter sorter,
            IEnumerable<ISwitchableGroup<T>> switchableGroups
        )
        {
            return new SorterEvalSetImpl
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

    public class SorterEvalSetImpl : ISorterEvalSet
    {
        public SorterEvalSetImpl
        (
            ISorter sorter,
            IEnumerable<ISorterEval> sorterOnSwitchableGroups
        )
        {
            _sorter = sorter;
            _sorterOnSwitchableGroups = sorterOnSwitchableGroups.ToDictionary(t => t.SwitchableGroupGuid);
            _switchUseList = _sorterOnSwitchableGroups.Values.Select(T => T.SwitchUseList).VectorSumDouble();
            _switchesUsed = SwitchUseList.Count(T => T > 0);
        }

        private readonly ISorter _sorter;
        public ISorter Sorter
        {
            get { return _sorter; }
        }

        private readonly Dictionary<Guid, ISorterEval> _sorterOnSwitchableGroups;

        public ISorterEval SorterOnSwitchableGroup(ISwitchableGroup switchableGroup)
        {
            return _sorterOnSwitchableGroups[switchableGroup.Guid];
        }

        public IEnumerable<ISorterEval> SorterOnSwitchableGroups
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
