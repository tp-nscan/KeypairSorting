using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sorting.Sorters;
using Sorting.Switchables;
using Sorting.SwitchFunctionSets;

namespace Sorting.CompetePool.old
{
    public interface ICompPoolOld
    {
        IReadOnlyList<ISwitchableGroupEval> SwitchableGroupEval { get; }
        IReadOnlyList<ISorterOnSwitchableGroupSet> SorterOnSwitchableGroupSets { get; }
    }

    public static class CompPoolOld
    {
        public static ICompPoolOld ToCompPool<T>
            (
                this IEnumerable<ISorter> sorters,
                IEnumerable<ISwitchableGroup<T>> switchableGroups
            )
        {
            var switchableGroupList = switchableGroups.ToList();
            var sorterGroupEvalOnSwitchableGroups = sorters.Select(t => t.MakeSorterOnSwitchableGroups(switchableGroupList))
                               .ToList();

            //var switchableGroupEvals = switchableGroupList
            //    .Select(t => t.ToLocalSwitchableGroupEval(sorterGroupEvalOnSwitchableGroups.Select(s=>s.SorterOnSwitchableGroup(t))));
            var switchableGroupEvals = switchableGroupList
                    .Select(t => t.ToGlobalSwitchableGroupEval(sorterGroupEvalOnSwitchableGroups));

            return new CompPoolOldImpl(switchableGroupEvals, sorterGroupEvalOnSwitchableGroups);
        }

        public static ICompPoolOld ToCompPoolParallel<T>
        (
            this IEnumerable<ISorter> sorters,
            IEnumerable<ISwitchableGroup<T>> switchableGroups
        )
        {
            var switchableGroupList = switchableGroups.ToList();
            KeyPairSwitchSet.Make<T>(switchableGroupList.First().KeyCount);
            var sorterGroupEvalOnSwitchableGroups = sorters.AsParallel().Select(t => t.MakeSorterOnSwitchableGroups(switchableGroupList))
                               .ToList();

            //var switchableGroupEvals = switchableGroupList
            //    .Select(t => t.ToLocalSwitchableGroupEval(sorterGroupEvalOnSwitchableGroups.Select(s=>s.SorterOnSwitchableGroup(t))));
            var switchableGroupEvals = switchableGroupList
                    .Select(t => t.ToGlobalSwitchableGroupEval(sorterGroupEvalOnSwitchableGroups));

            return new CompPoolOldImpl(switchableGroupEvals, sorterGroupEvalOnSwitchableGroups);
        }

        public static string ToReport(this ICompPoolOld compPoolOld, int numRecords, string label1, string label2)
        {
            var bestSorters = compPoolOld.SorterOnSwitchableGroupSets.OrderBy(t => t.SwitchesUsed).Take(numRecords).ToList();
            var bestSwitches = compPoolOld.SwitchableGroupEval.OrderByDescending(t => t.Fitness).Take(numRecords).ToList();

            var sb = new StringBuilder();
            sb.AppendLine("SorterHash\tSwitchCount\tFitness\t");

            for (var i = 0; i < numRecords; i++)
            {
                sb.AppendLine
                    (
                        String.Format
                        (
                            "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t", 
                            bestSorters[i].Sorter.Guid, 
                            bestSorters[i].SwitchesUsed, 
                            bestSwitches[i].Fitness.ToString("0.0"),
                            bestSwitches[i].SwitchableGroup.Guid,
                            label1, i, label2
                        )
                    );
            }

            return sb.ToString();
        }
    }

    class CompPoolOldImpl : ICompPoolOld
    {
        private readonly IReadOnlyList<ISwitchableGroupEval> _switchableGroupEval;
        private readonly IReadOnlyList<ISorterOnSwitchableGroupSet> _sorterOnSwitchableGroupSets;

        public CompPoolOldImpl
        (
            IEnumerable<ISwitchableGroupEval> switchableGroupEvals,
            IEnumerable<ISorterOnSwitchableGroupSet> sorterEvals
        )
        {
            _switchableGroupEval = switchableGroupEvals.ToList();
            _sorterOnSwitchableGroupSets = sorterEvals.ToList();
        }

        public IReadOnlyList<ISwitchableGroupEval> SwitchableGroupEval
        {
            get { return _switchableGroupEval; }
        }

        public IReadOnlyList<ISorterOnSwitchableGroupSet> SorterOnSwitchableGroupSets
        {
            get { return _sorterOnSwitchableGroupSets; }
        }
    }
}
