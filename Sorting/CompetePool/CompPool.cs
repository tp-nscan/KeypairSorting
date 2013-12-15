using System;
using System.Collections.Generic;
using System.Linq;
using Sorting.Sorters;
using Sorting.Switchables;
using Sorting.SwitchFunctionSets;

namespace Sorting.CompetePool
{
    public interface ICompPool
    {
        IEnumerable<ISorterOnSwitchableGroup> SorterOnSwitchableGroups { get; }
        ISorterOnSwitchableGroupSet SorterOnSwitchableGroupSet(Guid sorterId);
        IEnumerable<ISorterOnSwitchableGroupSet> SorterOnSwitchableGroupSets { get; }
    }

    public static class CompPool
    {
        public static ICompPool ToCompPoolParallel<T>
        (
            this IEnumerable<ISorter> sorters,
            IEnumerable<ISwitchableGroup<T>> switchableGroups
        )
        {
            var switchableGroupList = switchableGroups.ToList();
            KeyPairSwitchSet.Make<T>(switchableGroupList.First().KeyCount);
            return new CompPoolImpl(sorters.AsParallel().Select(t => t.MakeSorterOnSwitchableGroups(switchableGroupList)));
        }
    }

    class CompPoolImpl : ICompPool
    {
        public CompPoolImpl(IEnumerable<ISorterOnSwitchableGroupSet> sorterOnSwitchableGroupSets)
        {
            _sorterOnSwitchableGroupSets = sorterOnSwitchableGroupSets.ToDictionary(t => t.Sorter.Guid);
        }

        public IEnumerable<ISorterOnSwitchableGroup> SorterOnSwitchableGroups
        {
            get { return SorterOnSwitchableGroupSets.SelectMany(t=>t.SorterOnSwitchableGroups); }
        }

        public ISorterOnSwitchableGroupSet SorterOnSwitchableGroupSet(Guid sorterId)
        {
            return _sorterOnSwitchableGroupSets[sorterId];
        }

        private readonly IDictionary<Guid,ISorterOnSwitchableGroupSet> _sorterOnSwitchableGroupSets;
        public IEnumerable<ISorterOnSwitchableGroupSet> SorterOnSwitchableGroupSets
        {
            get { return _sorterOnSwitchableGroupSets.Values; }
        }
    }
}
