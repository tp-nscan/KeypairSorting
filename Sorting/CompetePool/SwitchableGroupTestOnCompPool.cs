using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.CompetePool
{
    public interface ISwitchableGroupTestCompPool
    {
        ISwitchableGroup SwitchableGroup { get; }
        double Fitness { get; set; }
        SorterTestOnSwitchableGroup SorterTestOnSwitchableGroup(ISorter sorter);
    }

    public static class SwitchableGroupTestOnCompPool
    {
        public static ISwitchableGroupTestCompPool MakeISwitchableGroupTestCompPool
            (
                ISwitchableGroup switchableGroup,
                IEnumerable<SorterTestOnSwitchableGroup> sorterTestOnSwitchableGroups
            )
        {
            return new SwitchableGroupTestCompPoolImpl
                (
                    switchableGroup: null,
                    sorterTestOnSwitchableGroups: null
                );
        }
    }

    internal class SwitchableGroupTestCompPoolImpl : ISwitchableGroupTestCompPool
    {
        public SwitchableGroupTestCompPoolImpl
        (
            ISwitchableGroup switchableGroup, 
            IEnumerable<SorterTestOnSwitchableGroup> sorterTestOnSwitchableGroups
        )
        {
            _switchableGroup = switchableGroup;
            _sorterTestOnSwitchableGroups = sorterTestOnSwitchableGroups.ToDictionary(t => t.Sorter.Guid);
        }

        private readonly ISwitchableGroup _switchableGroup;
        public ISwitchableGroup SwitchableGroup
        {
            get { return _switchableGroup; }
        }

        public double Fitness { get; set; }

        public SorterTestOnSwitchableGroup SorterTestOnSwitchableGroup(ISorter sorter)
        {
            return _sorterTestOnSwitchableGroups[sorter.Guid];
        }

        private readonly Dictionary<Guid, SorterTestOnSwitchableGroup> _sorterTestOnSwitchableGroups;

    }
}
