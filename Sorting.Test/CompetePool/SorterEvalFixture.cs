using System;
using System.Diagnostics;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.CompetePools;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.Test.CompetePool
{
    [TestClass]
    public class SorterEvalFixture
    {
        [TestMethod]
        public void TestAStagedSorter()
        {
            var stopwatch = new Stopwatch();
            const int keyCount = 14;
            const int keyPairCount = 600;
            const int switchableCount = 2000;

            var sorter = Rando.Fast(1243).ToSorter(keyCount, keyPairCount, Guid.NewGuid());
            var switchableGroup = Rando.Fast(1234).ToSwitchableGroup<uint>(Guid.NewGuid(), keyCount, switchableCount);

            stopwatch.Start();

            var sorterTestOnSwitchableGroup = sorter.Sort(switchableGroup);

            stopwatch.Stop();

            var reducedSorter = sorterTestOnSwitchableGroup.Reduce(Guid.NewGuid());
        }
    }
}
