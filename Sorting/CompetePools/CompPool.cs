using System;
using System.Collections.Generic;
using System.Linq;
using Sorting.Sorters;
using Sorting.Switchables;
using Sorting.SwitchFunctionSets;

namespace Sorting.CompetePools
{
    public interface ICompPool
    {
        int KeyCount { get; }
        IReadOnlyList<ISorter> Sorters { get; }
        IEnumerable<ISorterEval> SorterEvals { get; }
        ISorterEval SorterEval(Guid sorterEvalId);
        bool ContainsSorterEval(Guid sorterEvalId);
    }

    public static class CompPool
    {
        public static ICompPool MakeEmpty(int keyCount)
        {
            return new CompPoolImpl(Enumerable.Empty<ISorterEval>(), keyCount);
        }

        //public static ICompPool ToCompPoolParallel(
        //    this IEnumerable<ISorter> sorters
        //)
        //{
        //    var sortersList = sorters.ToList();
        //    var keyCount = sortersList[0].KeyCount;
        //    KeyPairSwitchSet.Make<uint>(keyCount);
        //    var switchables = Switchable.AllSwitchablesForKeyCount(keyCount).ToSwitchableGroup
        //        (
        //            guid: SwitchableGroup.GuidOfAllSwitchableGroupsForKeyCount(keyCount), 
        //            keyCount: keyCount
        //        );

        //    return new CompPoolImpl(
        //            sorterEvals: sortersList.AsParallel().Select(s => s.Sort(switchables)),
        //            keyCount: keyCount
        //        );
        //}

        public static ICompPool AddSorterEvalsParallel(this ICompPool compPool, 
            IEnumerable<ISorter> sorters)
        {
            KeyPairSwitchSet.Make<uint>(compPool.KeyCount);
            var switchables = Switchable.AllSwitchablesForKeyCount(compPool.KeyCount).ToSwitchableGroup
            (
                guid: SwitchableGroup.GuidOfAllSwitchableGroupsForKeyCount(compPool.KeyCount),
                keyCount: compPool.KeyCount
            );

            return new CompPoolImpl(
                sorterEvals: compPool.SorterEvals.Concat(sorters.AsParallel().Select(s => s.Sort(switchables))),
                keyCount: compPool.KeyCount
            );
        }

        public static ICompPool TrimEvalsToTheseIds(this ICompPool compPool,
             IEnumerable<Guid> sorterIds)
        {
            var idDict = sorterIds.ToDictionary(id => id);
            return new CompPoolImpl(
                sorterEvals: compPool.SorterEvals.Where(ev => idDict.ContainsKey(ev.Sorter.Guid)),
                keyCount: compPool.KeyCount
            );
        }

    }

    class CompPoolImpl : ICompPool
    {
        public CompPoolImpl(IEnumerable<ISorterEval> sorterEvals, int keyCount)
        {
            _keyCount = keyCount;
            _sorterEvals = sorterEvals.ToDictionary(s=>s.Sorter.Guid);
            _sorters = _sorterEvals.Values.Select(e => e.Sorter).ToList();
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        private readonly IReadOnlyList<ISorter> _sorters;
        public IReadOnlyList<ISorter> Sorters
        {
            get { return _sorters ; }
        }

        private readonly IDictionary<Guid, ISorterEval> _sorterEvals;
        public IEnumerable<ISorterEval> SorterEvals
        {
            get { return _sorterEvals.Values; }
        }

        public ISorterEval SorterEval(Guid sorterEvalId)
        {
            return _sorterEvals[sorterEvalId];
        }

        public bool ContainsSorterEval(Guid sorterEvalId)
        {
            return _sorterEvals.ContainsKey(sorterEvalId);
        }
    }
}
