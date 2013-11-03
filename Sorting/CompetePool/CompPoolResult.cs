using System.Collections.Generic;
using System.Linq;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.CompetePool
{
    public interface ICompPoolResult
    {
        IReadOnlyList<ISwitchableGroup> SwitchableGroups { get; }
        IReadOnlyList<ISorter> SortersRankDesc { get; }
    }

    public static class CompPoolResult
    {
        public static ICompPoolResult ByUseCount
            (
                IEnumerable<ISwitchableGroup> switchableGroups,
                IEnumerable<ISorterTestOnCompPool> sorterTestOnCompPools
            )
        {
            return new CompPoolResultByUseCount(switchableGroups, sorterTestOnCompPools);
        }
    }

    class CompPoolResultByUseCount : ICompPoolResult
    {
        public CompPoolResultByUseCount
            (
                IEnumerable<ISwitchableGroup> switchableGroups,
                IEnumerable<ISorterTestOnCompPool> sorterTestOnCompPools
            )
        {
            _switchableGroups = switchableGroups.ToList();
            _sorterTestOnCompPools = sorterTestOnCompPools.ToList();
            _sorters = _sorterTestOnCompPools.Select(t => t.Sorter).ToList();
        }


        private readonly List<ISorterTestOnCompPool> _sorterTestOnCompPools;
        public IReadOnlyList<ISorterTestOnCompPool> SorterTestOnCompPools
        {
            get { return _sorterTestOnCompPools; }
        }


        private readonly List<ISorter> _sorters;

        private readonly List<ISwitchableGroup> _switchableGroups;
        private IReadOnlyList<ISorter> _sortersRankDesc;

        public IReadOnlyList<ISwitchableGroup> SwitchableGroups
        {
            get { return _switchableGroups; }
        }

        public IReadOnlyList<ISorter> SortersRankDesc
        {
            get { return _sortersRankDesc; }
        }

        public IReadOnlyList<ISorter> Sorters
        {
            get { return _sorters; }
        }
    }
}
