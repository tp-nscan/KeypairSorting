using System;
using System.Diagnostics;
using System.Linq;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.CompetePools;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.Test.CompetePool
{
    [TestClass]
    public class CompParaPoolFixture
    {
        [TestMethod]
        public void TestCompParaPool()
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

            var compParaPool = sorters.ToCompParaPoolParallel(switchableGroups);

            stopwatch.Stop();

            Debug.WriteLine("Time(ms): {0}", stopwatch.ElapsedMilliseconds);

        }
    }
}
