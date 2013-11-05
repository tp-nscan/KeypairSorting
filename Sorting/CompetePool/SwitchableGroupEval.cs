using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Vector;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.CompetePool
{
    public interface ISwitchableGroupEval
    {
        ISwitchableGroup SwitchableGroup { get; }
        double Fitness { get; }
    }

    public interface ISwitchableGroupOnCompPool : ISwitchableGroupEval
    {
        SorterOnSwitchableGroup SorterTestOnSwitchableGroup(ISorter sorter);
    }

    public static class SwitchableGroupEval
    {
        public static ISwitchableGroupOnCompPool ToGlobalSwitchableGroupEval
        (
            this ISwitchableGroup switchableGroup,
            IEnumerable<ISorterEvalOnCompPool> sorterEvalOnCompPools
        )
        {
            var sorterTestList = sorterEvalOnCompPools.ToList();
            return new SwitchableGroupOnCompPoolImpl
                (
                switchableGroup: switchableGroup,
                sorterOnSwitchableGroups: sorterTestList.Select(t => t.SorterOnSwitchableGroup(switchableGroup)),
                fitness:
                    sorterTestList.Sum(
                        t => t.SorterOnSwitchableGroup(switchableGroup).SwitchUseList.Project(t.SwitchUseList)
                        )
                );
        }

        public static ISwitchableGroupOnCompPool ToLocalSwitchableGroupEval
        (
            this ISwitchableGroup switchableGroup,
            IEnumerable<SorterOnSwitchableGroup> sorterTestOnSwitchableGroups
        )
        {
            var sorterTests = sorterTestOnSwitchableGroups.ToList();
            return new SwitchableGroupOnCompPoolImpl
                (
                    switchableGroup: switchableGroup,
                    sorterOnSwitchableGroups: sorterTests,
                    //fitness: sorterTests.Sum(t => t.SwitchUseList.Project(globalSwitchUsage))
                    fitness: sorterTests.Sum(t=>t.SwitchesUsed)
                );
        }

        public static IEnumerable<ISwitchableGroup<T>> GetBestSwitchableGroups<T>
            (
                this IReadOnlyList<ISwitchableGroupEval> switchableGroups, 
                int fraction
            )
        {
            return switchableGroups.OrderByDescending(t => t.Fitness)
                                   .Select(e => e.SwitchableGroup)
                                   .Cast<ISwitchableGroup<T>>()
                                   .Take(switchableGroups.Count / fraction);
        }
    }

    internal class SwitchableGroupOnCompPoolImpl : ISwitchableGroupOnCompPool
    {
        public SwitchableGroupOnCompPoolImpl
        (
            ISwitchableGroup switchableGroup, 
            IEnumerable<SorterOnSwitchableGroup> sorterOnSwitchableGroups, 
            double fitness
        )
        {
            _switchableGroup = switchableGroup;
            _fitness = fitness;
            _sorterTestOnSwitchableGroups = sorterOnSwitchableGroups.ToDictionary(t => t.Sorter.Guid);
        }

        private readonly ISwitchableGroup _switchableGroup;
        public ISwitchableGroup SwitchableGroup
        {
            get { return _switchableGroup; }
        }

        private readonly double _fitness;
        public double Fitness
        {
            get { return _fitness; }
        }

        public SorterOnSwitchableGroup SorterTestOnSwitchableGroup(ISorter sorter)
        {
            return _sorterTestOnSwitchableGroups[sorter.Guid];
        }

        private readonly Dictionary<Guid, SorterOnSwitchableGroup> _sorterTestOnSwitchableGroups;
    }
}
