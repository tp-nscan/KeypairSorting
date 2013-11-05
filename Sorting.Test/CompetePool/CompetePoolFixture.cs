using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.CompetePool;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.Test.CompetePool
{
    [TestClass]
    public class CompetePoolFixture
    {
        [TestMethod]
        public void TestCompPool()
        {
            var stopwatch = new Stopwatch();
            const int keyCount = 16;
            const int keyPairCount = 1000;
            const int sorterCount = 100;
            const int switchableCount = 100;
            const int switchableGroupCount = 100;

            var sorters = Rando.Fast(123).ToRandomEnumerator()
                            .Select(t => t.ToSorter(keyCount, keyPairCount, Guid.NewGuid()))
                            .Take(sorterCount)
                            .ToList();

            var switchableGroups =
                Enumerable.Range(77, switchableGroupCount)
                    .Select(i => Rando.Fast(1234 + i).ToSwitchableGroup<uint>(Guid.NewGuid(), keyCount, switchableCount))
                    .ToList();

            stopwatch.Start();

            var compPool = sorters.ToCompPool(switchableGroups);

            stopwatch.Stop();

            Debug.WriteLine("Time(ms): {0}", stopwatch.ElapsedMilliseconds);

        }

        [TestMethod]
        public void TestCompPoolEvo()
        {
            var stopwatch = new Stopwatch();
            const int keyCount = 16;
            const int keyPairCount = 1000;
            const int sorterCount = 20;
            const int switchableCount = 500;
            const int switchableGroupCount = 20;

            var rando = Rando.Fast(123);

            var sorters = rando.ToRandomEnumerator()
                            .Select(t => t.ToSorter(keyCount, keyPairCount, Guid.NewGuid()))
                            .Take(sorterCount)
                            .ToList();

            var switchableGroups =
                Enumerable.Range(77, switchableGroupCount)
                    .Select(i => Rando.Fast(1234 + i).ToSwitchableGroup<uint>(Guid.NewGuid(), keyCount, switchableCount))
                    .ToList();

            stopwatch.Start();

            var compPool = sorters.ToCompPool(switchableGroups);

            for (var i = 0; i < 100; i++)
            {
                compPool = compPool.SelectAndMutate<uint>(rando);
                Debug.WriteLine(compPool.ToReport(sorterCount, i.ToString(), "new"));
                var compPool2 = compPool.SorterEvals.Select(t => t.Sorter).ToCompPool(switchableGroups);
                Debug.WriteLine(compPool2.ToReport(sorterCount, i.ToString(), "orig"));
            }

            stopwatch.Stop();

            Debug.WriteLine("Time(ms): {0}", stopwatch.ElapsedMilliseconds);

        }
    }
}
