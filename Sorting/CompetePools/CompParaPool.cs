using System;
using System.Collections.Generic;
using System.Linq;
using Sorting.Sorters;
using Sorting.Switchables;
using Sorting.SwitchFunctionSets;

namespace Sorting.CompetePools
{
    public interface ICompParaPool
    {
        IReadOnlyList<ISorter> Sorters { get; }
        IReadOnlyList<ISwitchableGroup> SwitchableGroups { get; }
        IEnumerable<ISorterOnSwitchableGroup> SorterOnSwitchableGroups { get; }
        ISorterOnSwitchableGroupSet SorterOnSwitchableGroupSet(Guid sorterId);
        IEnumerable<ISorterOnSwitchableGroupSet> SorterOnSwitchableGroupSets { get; }
    }

    public static class CompParaPool
    {
        public static ICompParaPool ToCompParaPoolParallel<T>
        (
            this IEnumerable<ISorter> sorters,
            IEnumerable<ISwitchableGroup<T>> switchableGroups
        )
        {
            var switchableGroupList = switchableGroups.ToList();
            var sortersList = sorters.ToList();
            KeyPairSwitchSet.Make<T>(switchableGroupList.First().KeyCount);
            return new CompParaPoolImpl(
                sortersList,
                switchableGroupList,
                sortersList.AsParallel().Select(t => t.MakeSorterOnSwitchableGroups(switchableGroupList)));
        }
    }

    class CompParaPoolImpl : ICompParaPool
    {
        public CompParaPoolImpl
            (
            
                IEnumerable<ISorter> sorters,
                IEnumerable<ISwitchableGroup> switchableGroups,
                IEnumerable<ISorterOnSwitchableGroupSet> sorterOnSwitchableGroupSets
            )
        {
            _sorters = sorters.ToList();
            _switchableGroups = switchableGroups.ToList();
            _sorterOnSwitchableGroupSets = sorterOnSwitchableGroupSets.ToDictionary(t => t.Sorter.Guid);
        }

        private readonly IReadOnlyList<ISorter> _sorters;
        public IReadOnlyList<ISorter> Sorters
        {
            get { return _sorters; }
        }

        private readonly IReadOnlyList<ISwitchableGroup> _switchableGroups;
        public IReadOnlyList<ISwitchableGroup> SwitchableGroups
        {
            get { return _switchableGroups; }
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
