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
        IEnumerable<ISorterEval> SorterOnSwitchableGroups { get; }
        ISorterEval SorterOnSwitchableGroup(Guid sorterId);
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
        public CompPoolImpl(IReadOnlyList<ISorter> sorters, IEnumerable<ISorterEval> sorterOnSwitchableGroups)
        {
            _sorters = sorters;
            _sorterOnSwitchableGroups = sorterOnSwitchableGroups.ToDictionary(s=>s.Sorter.Guid);
        }

        private readonly IReadOnlyList<ISorter> _sorters;
        public IReadOnlyList<ISorter> Sorters
        {
            get { return _sorters; }
        }

        private readonly IDictionary<Guid, ISorterEval> _sorterOnSwitchableGroups;
        public IEnumerable<ISorterEval> SorterOnSwitchableGroups
        {
            get { return _sorterOnSwitchableGroups.Values; }
        }

        public ISorterEval SorterOnSwitchableGroup(Guid sorterId)
        {
            return _sorterOnSwitchableGroups[sorterId];
        }
    }
}
