using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
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

    public interface ISorterEvalOnCompPool : ISorterEval
    {
        IEnumerable<SorterOnSwitchableGroup> SorterOnSwitchableGroups { get; }
        SorterOnSwitchableGroup SorterOnSwitchableGroup(ISwitchableGroup switchableGroup);
    }

    public static class SorterEval
    {
        public static ISorterEvalOnCompPool MakeSorterTestOnCompPool<T>
        (
            this ISorter sorter, 
            IEnumerable<ISwitchableGroup<T>> switchableGroups
        )
        {
            return new SorterEvalOnCompPoolImpl
                (
                    sorter: sorter,
                    sorterTestOnSwitchableGroups: switchableGroups.Select(sorter.Sort)
                );
        }

        public static IEnumerable<ISorter> GetBestSorters(this IReadOnlyList<ISorterEval> sorterEvals, int fraction)
        {
            return sorterEvals.OrderBy(t => t.SwitchesUsed)
                              .Select(e=>e.Sorter)
                              .Take(sorterEvals.Count/fraction);
        }

        public static int UsedKeyPairHash(this ISorterEval sorterEval)
        {
            //return sorterEval.Sorter.KeyPairs.Filter(i => sorterEval.SwitchUseList[i] > 0).ToHash(k => k.Index);
            return sorterEval.Sorter.KeyPairs.Where((t,i) => sorterEval.SwitchUseList[i] > 0).ToHash(k => k.Index);
        }
    }

    public class SorterEvalOnCompPoolImpl : ISorterEvalOnCompPool
    {
        public SorterEvalOnCompPoolImpl
        (
            ISorter sorter, 
            IEnumerable<SorterOnSwitchableGroup> sorterTestOnSwitchableGroups
        )
        {
            _sorter = sorter;
            _sorterTestOnSwitchableGroups = sorterTestOnSwitchableGroups.ToDictionary(t => t.SwitchableGroup.Guid);
            _switchUseList = _sorterTestOnSwitchableGroups.Values.Select(T => T.SwitchUseList).VectorSumDouble();
            _switchesUsed = SwitchUseList.Count(T => T > 0);
        }

        private readonly ISorter _sorter;
        public ISorter Sorter
        {
            get { return _sorter; }
        }

        private readonly Dictionary<Guid, SorterOnSwitchableGroup> _sorterTestOnSwitchableGroups;

        public SorterOnSwitchableGroup SorterOnSwitchableGroup(ISwitchableGroup switchableGroup)
        {
            return _sorterTestOnSwitchableGroups[switchableGroup.Guid];
        }

        public IEnumerable<SorterOnSwitchableGroup> SorterOnSwitchableGroups
        {
            get
            {
                return _sorterTestOnSwitchableGroups.Values;
            }
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
