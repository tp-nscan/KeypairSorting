using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathUtils.Collections;
using MathUtils.Rand;
using Sorting.Sorters;
using Sorting.Switchables;
using Sorting.SwitchFunctionSets;

namespace Sorting.CompetePool
{
    public interface ICompPool
    {
        IReadOnlyList<ISwitchableGroupEval> SwitchableGroupEval { get; }
        IReadOnlyList<ISorterEval> SorterEvals { get; }
    }

    public static class CompPool
    {
    //    public static ICompPool ToCompPool
    //(
    //    this IEnumerable<ISorter> sorters,
    //    IEnumerable<ISwitchableGroup<uint>> switchableGroups
    //)
    //    {
    //        var switchableGroupList = switchableGroups.ToList();
    //        var sorterEvals = sorters.Select(t => t.MakeSorterTestOnCompPool(switchableGroupList))
    //                           .ToList();

    //        //var switchableGroupEvals = switchableGroupList
    //        //    .Select(t => t.ToLocalSwitchableGroupEval(sorterEvals.Select(s=>s.SorterOnSwitchableGroup(t))));
    //        var switchableGroupEvals = switchableGroupList
    //                .Select(t => t.ToGlobalSwitchableGroupEval(sorterEvals));

    //        return new CompPoolImpl(switchableGroupEvals, sorterEvals);
    //    }

    //    public static ICompPool ToCompPoolParallel
    //    (
    //        this IEnumerable<ISorter> sorters,
    //        IEnumerable<ISwitchableGroup<uint>> switchableGroups
    //    )
    //    {
    //        var switchableGroupList = switchableGroups.ToList();
    //        KeyPairSwitchSet.Make<uint>(switchableGroupList.First().KeyCount);
    //        var sorterEvals = sorters.AsParallel().Select(t => t.MakeSorterTestOnCompPool(switchableGroupList))
    //                           .ToList();

    //        //var switchableGroupEvals = switchableGroupList
    //        //    .Select(t => t.ToLocalSwitchableGroupEval(sorterEvals.Select(s=>s.SorterOnSwitchableGroup(t))));
    //        var switchableGroupEvals = switchableGroupList
    //                .Select(t => t.ToGlobalSwitchableGroupEval(sorterEvals));

    //        return new CompPoolImpl(switchableGroupEvals, sorterEvals);
    //    }

    //    public static ICompPool ToCompPool
    //        (
    //            this IEnumerable<ISorter> sorters,
    //            IEnumerable<ISwitchableGroup<ulong>> switchableGroups
    //        )
    //    {
    //        var switchableGroupList = switchableGroups.ToList();
    //        var sorterEvals = sorters.Select(t => t.MakeSorterTestOnCompPool(switchableGroupList))
    //                           .ToList();

    //        //var switchableGroupEvals = switchableGroupList
    //        //    .Select(t => t.ToLocalSwitchableGroupEval(sorterEvals.Select(s=>s.SorterOnSwitchableGroup(t))));
    //        var switchableGroupEvals = switchableGroupList
    //                .Select(t => t.ToGlobalSwitchableGroupEval(sorterEvals));

    //        return new CompPoolImpl(switchableGroupEvals, sorterEvals);
    //    }

    //    public static ICompPool ToCompPoolParallel
    //        (
    //            this IEnumerable<ISorter> sorters,
    //            IEnumerable<ISwitchableGroup<ulong>> switchableGroups
    //        )
    //    {
    //        var switchableGroupList = switchableGroups.ToList();
    //        KeyPairSwitchSet.Make<uint>(switchableGroupList.First().KeyCount);
    //        var sorterEvals = sorters.AsParallel().Select(t => t.MakeSorterTestOnCompPool(switchableGroupList))
    //                           .ToList();

    //        //var switchableGroupEvals = switchableGroupList
    //        //    .Select(t => t.ToLocalSwitchableGroupEval(sorterEvals.Select(s=>s.SorterOnSwitchableGroup(t))));
    //        var switchableGroupEvals = switchableGroupList
    //                .Select(t => t.ToGlobalSwitchableGroupEval(sorterEvals));

    //        return new CompPoolImpl(switchableGroupEvals, sorterEvals);
    //    }

    //    public static ICompPool ToCompPool
    //(
    //    this IEnumerable<ISorter> sorters,
    //    IEnumerable<ISwitchableGroup<bool[]>> switchableGroups
    //)
    //    {
    //        var switchableGroupList = switchableGroups.ToList();
    //        var sorterEvals = sorters.Select(t => t.MakeSorterTestOnCompPool(switchableGroupList))
    //                           .ToList();

    //        //var switchableGroupEvals = switchableGroupList
    //        //    .Select(t => t.ToLocalSwitchableGroupEval(sorterEvals.Select(s=>s.SorterOnSwitchableGroup(t))));
    //        var switchableGroupEvals = switchableGroupList
    //                .Select(t => t.ToGlobalSwitchableGroupEval(sorterEvals));

    //        return new CompPoolImpl(switchableGroupEvals, sorterEvals);
    //    }

    //    public static ICompPool ToCompPoolParallel
    //        (
    //            this IEnumerable<ISorter> sorters,
    //            IEnumerable<ISwitchableGroup<bool[]>> switchableGroups
    //        )
    //    {
    //        var switchableGroupList = switchableGroups.ToList();
    //        KeyPairSwitchSet.Make<uint>(switchableGroupList.First().KeyCount);
    //        var sorterEvals = sorters.AsParallel().Select(t => t.MakeSorterTestOnCompPool(switchableGroupList))
    //                           .ToList();

    //        //var switchableGroupEvals = switchableGroupList
    //        //    .Select(t => t.ToLocalSwitchableGroupEval(sorterEvals.Select(s=>s.SorterOnSwitchableGroup(t))));
    //        var switchableGroupEvals = switchableGroupList
    //                .Select(t => t.ToGlobalSwitchableGroupEval(sorterEvals));

    //        return new CompPoolImpl(switchableGroupEvals, sorterEvals);
    //    }

    //    public static ICompPool ToCompPool
    //(
    //    this IEnumerable<ISorter> sorters,
    //    IEnumerable<ISwitchableGroup<int[]>> switchableGroups
    //)
    //    {
    //        var switchableGroupList = switchableGroups.ToList();
    //        var sorterEvals = sorters.Select(t => t.MakeSorterTestOnCompPool(switchableGroupList))
    //                           .ToList();

    //        //var switchableGroupEvals = switchableGroupList
    //        //    .Select(t => t.ToLocalSwitchableGroupEval(sorterEvals.Select(s=>s.SorterOnSwitchableGroup(t))));
    //        var switchableGroupEvals = switchableGroupList
    //                .Select(t => t.ToGlobalSwitchableGroupEval(sorterEvals));

    //        return new CompPoolImpl(switchableGroupEvals, sorterEvals);
    //    }

    //    public static ICompPool ToCompPoolParallel
    //        (
    //            this IEnumerable<ISorter> sorters,
    //            IEnumerable<ISwitchableGroup<int[]>> switchableGroups
    //        )
    //    {
    //        var switchableGroupList = switchableGroups.ToList();
    //        KeyPairSwitchSet.Make<uint>(switchableGroupList.First().KeyCount);
    //        var sorterEvals = sorters.AsParallel().Select(t => t.MakeSorterTestOnCompPool(switchableGroupList))
    //                           .ToList();

    //        //var switchableGroupEvals = switchableGroupList
    //        //    .Select(t => t.ToLocalSwitchableGroupEval(sorterEvals.Select(s=>s.SorterOnSwitchableGroup(t))));
    //        var switchableGroupEvals = switchableGroupList
    //                .Select(t => t.ToGlobalSwitchableGroupEval(sorterEvals));

    //        return new CompPoolImpl(switchableGroupEvals, sorterEvals);
    //    }
        public static ICompPool ToCompPool<T>
            (
                this IEnumerable<ISorter> sorters,
                IEnumerable<ISwitchableGroup<T>> switchableGroups
            )
        {
            var switchableGroupList = switchableGroups.ToList();
            var sorterEvals = sorters.Select(t => t.MakeSorterTestOnCompPool(switchableGroupList))
                               .ToList();

            //var switchableGroupEvals = switchableGroupList
            //    .Select(t => t.ToLocalSwitchableGroupEval(sorterEvals.Select(s=>s.SorterOnSwitchableGroup(t))));
            var switchableGroupEvals = switchableGroupList
                    .Select(t => t.ToGlobalSwitchableGroupEval(sorterEvals));

            return new CompPoolImpl(switchableGroupEvals, sorterEvals);
        }

        public static ICompPool ToCompPoolParallel<T>
        (
            this IEnumerable<ISorter> sorters,
            IEnumerable<ISwitchableGroup<T>> switchableGroups
        )
        {
            var switchableGroupList = switchableGroups.ToList();
            KeyPairSwitchSet.Make<T>(switchableGroupList.First().KeyCount);
            var sorterEvals = sorters.AsParallel().Select(t => t.MakeSorterTestOnCompPool(switchableGroupList))
                               .ToList();

            //var switchableGroupEvals = switchableGroupList
            //    .Select(t => t.ToLocalSwitchableGroupEval(sorterEvals.Select(s=>s.SorterOnSwitchableGroup(t))));
            var switchableGroupEvals = switchableGroupList
                    .Select(t => t.ToGlobalSwitchableGroupEval(sorterEvals));

            return new CompPoolImpl(switchableGroupEvals, sorterEvals);
        }

        public static ICompPool SelectAndMutate<T>(this ICompPool originalPool, IRando rando)
        {
            const int selectionRatio = 4;
            const double sorterMutationRate = 0.02;
            const double switchableMutationRate = 0.25;
            const double insertionRate = 0.02;
            const double deletionRate = 0.02;

            var bestSorters = originalPool.SorterEvals.GetBestSorters(selectionRatio).ToList();
            var bestSwitchables = originalPool.SwitchableGroupEval.GetBestSwitchableGroups<T>(selectionRatio).ToList();

            var newSorters =
                bestSorters.Select(s => s.ToEnumerable().Concat(s.Mutate(rando, sorterMutationRate).Take(selectionRatio - 1)))
                .SelectMany(t => t);

            var newSwitchables =
                bestSwitchables.Select(
                    s => s.ToEnumerable().Concat(s.Mutate(rando, switchableMutationRate, GuidExt.NewGuids()).Take(selectionRatio - 1)))
                    .SelectMany(t => t);

            return newSorters.ToCompPool(newSwitchables);
            //return newSorters.ToCompPool(originalPool.SwitchableGroupEval.Select(t=>t.SwitchableGroupImpl).Cast<ISwitchableGroup<T>>());
        }

        public static string ToReport(this ICompPool compPool, int numRecords, string label1, string label2)
        {
            var bestSorters = compPool.SorterEvals.OrderBy(t => t.SwitchesUsed).Take(numRecords).ToList();
            var bestSwitches = compPool.SwitchableGroupEval.OrderByDescending(t => t.Fitness).Take(numRecords).ToList();

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

    class CompPoolImpl : ICompPool
    {
        private readonly IReadOnlyList<ISwitchableGroupEval> _switchableGroupEval;
        private readonly IReadOnlyList<ISorterEval> _sorterEvals;

        public CompPoolImpl
        (
            IEnumerable<ISwitchableGroupEval> switchableGroupEvals,
            IEnumerable<ISorterEval> sorterEvals
        )
        {
            _switchableGroupEval = switchableGroupEvals.ToList();
            _sorterEvals = sorterEvals.ToList();
        }

        public IReadOnlyList<ISwitchableGroupEval> SwitchableGroupEval
        {
            get { return _switchableGroupEval; }
        }

        public IReadOnlyList<ISorterEval> SorterEvals
        {
            get { return _sorterEvals; }
        }
    }
}
