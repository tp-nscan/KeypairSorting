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
        IEnumerable<ISorterOnSwitchableGroup> SorterOnSwitchableGroups { get; }
        ISorterOnSwitchableGroup SorterOnSwitchableGroup(Guid sorterId);
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
            var switchables = Switchable.AllSwitchablesForKeyCount(keyCount).ToSwitchableGroup(Guid.NewGuid(), keyCount);

            return new CompPoolImpl(
                    sortersList,
                    sortersList.AsParallel().Select(s => s.Sort(switchables))
                );
        }
    }

    class CompPoolImpl : ICompPool
    {
        public CompPoolImpl(IReadOnlyList<ISorter> sorters, IEnumerable<ISorterOnSwitchableGroup> sorterOnSwitchableGroups)
        {
            _sorters = sorters;
            _sorterOnSwitchableGroups = sorterOnSwitchableGroups.ToDictionary(s=>s.Sorter.Guid);
        }

        private readonly IReadOnlyList<ISorter> _sorters;
        public IReadOnlyList<ISorter> Sorters
        {
            get { return _sorters; }
        }

        private readonly IDictionary<Guid, ISorterOnSwitchableGroup> _sorterOnSwitchableGroups;
        public IEnumerable<ISorterOnSwitchableGroup> SorterOnSwitchableGroups
        {
            get { return _sorterOnSwitchableGroups.Values; }
        }

        public ISorterOnSwitchableGroup SorterOnSwitchableGroup(Guid sorterId)
        {
            return _sorterOnSwitchableGroups[sorterId];
        }
    }
}
