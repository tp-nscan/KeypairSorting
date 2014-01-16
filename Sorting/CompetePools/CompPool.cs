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
        IReadOnlyList<ISorter> Sorters { get; }
        IEnumerable<ISorterEval> SorterEvals { get; }
        ISorterEval SorterEval(Guid sorterId);
    }

    public static class CompPool
    {
        public static ICompPool ToCompPoolParallel
        (
            this IEnumerable<ISorter> sorters
        )
        {
            var sortersList = sorters.ToList();
            var keyCount = sortersList[0].KeyCount;
            KeyPairSwitchSet.Make<uint>(keyCount);
            var switchables = Switchable.AllSwitchablesForKeyCount(keyCount).ToSwitchableGroup
                (
                    guid: SwitchableGroup.GuidOfAllSwitchableGroupsForKeyCount(keyCount), 
                    keyCount: keyCount
                );

            return new CompPoolImpl(
                    sortersList,
                    sortersList.AsParallel().Select(s => s.Sort(switchables))
                );
        }
    }

    class CompPoolImpl : ICompPool
    {
        public CompPoolImpl(IReadOnlyList<ISorter> sorters, IEnumerable<ISorterEval> sorterEvals)
        {
            _sorters = sorters;
            _sorterEvals = sorterEvals.ToDictionary(s=>s.Sorter.Guid);
        }

        private readonly IReadOnlyList<ISorter> _sorters;
        public IReadOnlyList<ISorter> Sorters
        {
            get { return _sorters; }
        }

        private readonly IDictionary<Guid, ISorterEval> _sorterEvals;
        public IEnumerable<ISorterEval> SorterEvals
        {
            get { return _sorterEvals.Values; }
        }

        public ISorterEval SorterEval(Guid sorterId)
        {
            return _sorterEvals[sorterId];
        }
    }
}
