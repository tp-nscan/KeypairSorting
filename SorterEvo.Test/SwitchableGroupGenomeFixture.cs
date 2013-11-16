using System;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SorterEvo.Test
{
    [TestClass]
    public class SwitchableGroupGenomeFixture
    {
        [TestMethod]
        public void TestRandomSwitchableGroupGenomeUInt()
        {
            const int cSeed = 1234;
            const int cGroupSize = 100;
            const int cKeyCount = 16;
            const SwitchableGroupGenomeType cSwitchableGroupGenomeType = SwitchableGroupGenomeType.UInt;

            var switchableGroupGenome = Rando.Fast(cSeed).ToSwitchableGroupGenome
                (
                    switchableGroupGenomeType: cSwitchableGroupGenomeType,
                    keyCount: cKeyCount,
                    groupSize: cGroupSize
                );

            Assert.AreEqual(switchableGroupGenome.KeyCount, cKeyCount);
            Assert.AreEqual(switchableGroupGenome.Chromosome.Sequence.Count, cGroupSize);
            Assert.AreEqual(switchableGroupGenome.SwitchableGroupGenomeType, cSwitchableGroupGenomeType);

        }

        [TestMethod]
        public void TestRandomSwitchableGroupGenomeULong()
        {
            const int cSeed = 1234;
            const int cGroupSize = 100;
            const int cKeyCount = 16;
            const SwitchableGroupGenomeType cSwitchableGroupGenomeType = SwitchableGroupGenomeType.ULong;

            var switchableGroupGenome = Rando.Fast(cSeed).ToSwitchableGroupGenome
                (
                    switchableGroupGenomeType: cSwitchableGroupGenomeType,
                    keyCount: cKeyCount,
                    groupSize: cGroupSize
                );

            Assert.AreEqual(switchableGroupGenome.KeyCount, cKeyCount);
            Assert.AreEqual(switchableGroupGenome.Chromosome.Sequence.Count, cGroupSize*2);
            Assert.AreEqual(switchableGroupGenome.SwitchableGroupGenomeType, cSwitchableGroupGenomeType);

        }

        [TestMethod]
        public void TestRandomSwitchableGroupGenomeBitArray()
        {
            const int cSeed = 1234;
            const int cGroupSize = 100;
            const int cKeyCount = 16;
            const SwitchableGroupGenomeType cSwitchableGroupGenomeType = SwitchableGroupGenomeType.BitArray;

            var switchableGroupGenome = Rando.Fast(cSeed).ToSwitchableGroupGenome
                (
                    switchableGroupGenomeType: cSwitchableGroupGenomeType,
                    keyCount: cKeyCount,
                    groupSize: cGroupSize
                );

            Assert.AreEqual(switchableGroupGenome.KeyCount, cKeyCount);
            Assert.AreEqual(switchableGroupGenome.Chromosome.Sequence.Count, cGroupSize);
            Assert.AreEqual(switchableGroupGenome.SwitchableGroupGenomeType, cSwitchableGroupGenomeType);

        }

        [TestMethod]
        public void TestRandomSwitchableGroupGenomeIntArray()
        {
            const int cSeed = 1234;
            const int cGroupSize = 100;
            const int cKeyCount = 16;
            const SwitchableGroupGenomeType cSwitchableGroupGenomeType = SwitchableGroupGenomeType.IntArray;

            var switchableGroupGenome = Rando.Fast(cSeed).ToSwitchableGroupGenome
                (
                    switchableGroupGenomeType: cSwitchableGroupGenomeType,
                    keyCount: cKeyCount,
                    groupSize: cGroupSize
                );

            Assert.AreEqual(switchableGroupGenome.KeyCount, cKeyCount);
            Assert.AreEqual(switchableGroupGenome.Chromosome.Sequence.Count, cGroupSize);
            Assert.AreEqual(switchableGroupGenome.SwitchableGroupGenomeType, cSwitchableGroupGenomeType);

        }
        [TestMethod]
        public void TestToSwitchableGroup()
        {
        }
    }
}
