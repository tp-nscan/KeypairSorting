using System.Collections.Generic;
using System.Linq;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.CompetePool
{
    public class CompPool<T>
    {
        private readonly List<ISorter> _sorters = new List<ISorter>();
        private readonly List<ISwitchableGroup<T>> _switchableGroups = new List<ISwitchableGroup<T>>();

        public CompPool(IEnumerable<ISorter> sorters, IEnumerable<ISwitchableGroup<T>> switchableGroups)
        {
            _sorters = sorters.ToList();
            _switchableGroups = switchableGroups.ToList();
        }

        public IEnumerable<ISwitchableGroup<T>> SwitchableGroups
        {
            get { return _switchableGroups; }
        }

        public IEnumerable<ISorter> Sorters
        {
            get { return _sorters; }
        }
    }
}
