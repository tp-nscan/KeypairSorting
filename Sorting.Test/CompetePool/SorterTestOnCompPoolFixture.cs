using System;
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
    public class SorterTestOnCompPoolFixture
    {
        [TestMethod]
        public void TestSorterTestOnCompPool()
        {
            var stopwatch = new Stopwatch();
            const int keyCount = 16;
            const int keyPairCount = 600;
            const int switchableCount = 100;
            const int switchableGroupCount = 40;

            var sorter = Sorter.RandomSorter(212, keyCount, keyPairCount);
            var switchableGroups =
                Enumerable.Range(77, switchableGroupCount)
                    .Select(i => Rando.Fast(1234 + i).ToSwitchableGroup<uint>(Guid.NewGuid(), keyCount, switchableCount))
                    .ToList();

            stopwatch.Start();

            var sorterTestOnCompPool = sorter.MakeSorterTestOnCompPool(switchableGroups);

            stopwatch.Stop();

            var score = sorterTestOnCompPool.SwitchesUsed;

            Debug.WriteLine("Time(ms): {0}", stopwatch.ElapsedMilliseconds);
        }
    }
}
