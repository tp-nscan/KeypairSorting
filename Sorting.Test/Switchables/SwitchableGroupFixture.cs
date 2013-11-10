using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.Switchables;

namespace Sorting.Test.Switchables
{
    [TestClass]
    public class SwitchableGroupFixture
    {
        [TestMethod]
        public void TestRandomMake()
        {
            const int keyCount = 16;
            const int switchableCount = 100;

            var switchableGroup = Rando.Fast(123).ToSwitchableGroup<uint>(Guid.NewGuid(), keyCount, switchableCount);

            Assert.AreEqual(switchableGroup.KeyCount, keyCount);
            Assert.AreEqual(switchableGroup.Switchables.Count, switchableCount);
        }

        [TestMethod]
        public void TestMutateUint()
        {
            const int keyCount = 16;
            const int switchableCount = 1000;
            const double mutationRate = 0.3;

            var switchableGroup = Rando.Fast(123).ToSwitchableGroup<uint>(Guid.NewGuid(), keyCount, switchableCount);

            var mutant = switchableGroup.Mutate(Rando.Fast(222), mutationRate, GuidExt.NewGuids()).First();

            var diffs = switchableGroup.Switchables.GetDifferentItems(mutant.Switchables).ToList();

            Assert.AreEqual(mutant.KeyCount, keyCount);
            Assert.AreEqual(mutant.Switchables.Count, switchableCount);
            Assert.IsTrue(diffs.Count < 350);
            Assert.IsTrue(diffs.Count > 250);
        }

        [TestMethod]
        public void TestMutateUlong()
        {
            const int keyCount = 16;
            const int switchableCount = 1000;
            const double mutationRate = 0.3;

            var switchableGroup = Rando.Fast(123).ToSwitchableGroup<ulong>(Guid.NewGuid(), keyCount, switchableCount);

            var mutant = switchableGroup.Mutate(Rando.Fast(222), mutationRate, GuidExt.NewGuids()).First();

            var diffs = switchableGroup.Switchables.GetDifferentItems(mutant.Switchables).ToList();

            Assert.AreEqual(mutant.KeyCount, keyCount);
            Assert.AreEqual(mutant.Switchables.Count, switchableCount);
            Assert.IsTrue(diffs.Count < 350);
            Assert.IsTrue(diffs.Count > 250);
        }

        [TestMethod]
        public void TestMutateUIntArray()
        {
            const int keyCount = 30;
            const int switchableCount = 1000;
            const double mutationRate = 0.3;

            var switchableGroup = Rando.Fast(123).ToSwitchableGroup<int[]>(Guid.NewGuid(), keyCount, switchableCount);

            var mutant = switchableGroup.Mutate(Rando.Fast(222), mutationRate, GuidExt.NewGuids()).First();

            var diffs = switchableGroup.Switchables.GetDifferentItems(mutant.Switchables).ToList();

            Assert.AreEqual(mutant.KeyCount, keyCount);
            Assert.AreEqual(mutant.Switchables.Count, switchableCount);
            Assert.IsTrue(diffs.Count < 350);
            Assert.IsTrue(diffs.Count > 250);
        }

        [TestMethod]
        public void TestMutateUBitArray()
        {
            const int keyCount = 27;
            const int switchableCount = 1000;
            const double mutationRate = 0.3;

            var switchableGroup = Rando.Fast(123).ToSwitchableGroup<bool[]>(Guid.NewGuid(), keyCount, switchableCount);

            var mutant = switchableGroup.Mutate(Rando.Fast(222), mutationRate, GuidExt.NewGuids()).First();

            var diffs = switchableGroup.Switchables.GetDifferentItems(mutant.Switchables).ToList();

            Assert.AreEqual(mutant.KeyCount, keyCount);
            Assert.AreEqual(mutant.Switchables.Count, switchableCount);
            Assert.IsTrue(diffs.Count < 350);
            Assert.IsTrue(diffs.Count > 250);
        }
    }
}
