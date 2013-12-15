using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Vector;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.CompetePool.old
{
    public interface ISwitchableGroupEval
    {
        ISwitchableGroup SwitchableGroup { get; }
        double Fitness { get; }
    }

    public interface ISwitchableGroupOnSorterGroup : ISwitchableGroupEval
    {
        ISorterOnSwitchableGroup SorterOnSwitchableGroup(ISorter sorter);
    }

    public static class SwitchableGroupEval
    {
        public static ISwitchableGroupOnSorterGroup ToGlobalSwitchableGroupEval
        (
            this ISwitchableGroup switchableGroup,
            IEnumerable<ISorterOnSwitchableGroupSet> sorterEvalOnSwitchableGroupses
        )
        {
            var sorterEvalOnSwitchableGroupsList = sorterEvalOnSwitchableGroupses.ToList();
            return new SwitchableGroupOnSorterGroupImpl
                (
                    switchableGroup: switchableGroup,
                    sorterOnSwitchableGroups: sorterEvalOnSwitchableGroupsList
                                    .Select(t => t.SorterOnSwitchableGroup(switchableGroup)),
                    fitness:
                        sorterEvalOnSwitchableGroupsList.Sum(
                            t => t.SorterOnSwitchableGroup(switchableGroup).SwitchUseList.Project(t.SwitchUseList)
                            )
                );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="switchableGroup"></param>
        /// <param name="sorterOnSwitchableGroup"></param>
        /// <returns></returns>
        public static ISwitchableGroupOnSorterGroup ToLocalSwitchableGroupEval
        (
            this ISwitchableGroup switchableGroup,
            IEnumerable<ISorterOnSwitchableGroup> sorterOnSwitchableGroup
        )
        {
            var sorterTests = sorterOnSwitchableGroup.ToList();
            return new SwitchableGroupOnSorterGroupImpl
                (
                    switchableGroup: switchableGroup,
                    sorterOnSwitchableGroups: sorterTests,
                    //fitness: sorterTests.Sum(t => t.SwitchUseList.Project(globalSwitchUsage))
                    fitness: sorterTests.Sum(t=>t.SwitchesUsed)
                );
        }

    }

    internal class SwitchableGroupOnSorterGroupImpl : ISwitchableGroupOnSorterGroup
    {
        public SwitchableGroupOnSorterGroupImpl
        (
            ISwitchableGroup switchableGroup,
            IEnumerable<ISorterOnSwitchableGroup> sorterOnSwitchableGroups, 
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

        public ISorterOnSwitchableGroup SorterOnSwitchableGroup(ISorter sorter)
        {
            return _sorterTestOnSwitchableGroups[sorter.Guid];
        }

        private readonly Dictionary<Guid, ISorterOnSwitchableGroup> _sorterTestOnSwitchableGroups;
    }
}
