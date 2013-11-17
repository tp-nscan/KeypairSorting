using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.Genomes;

namespace SorterEvo.Test.Genomes
{
    [TestClass]
    public class GenomeToSwitchableGroupFixture
    {
        [TestMethod]
        public void TestToSwitchableGroupUint()
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

            var switchableGroup = switchableGroupGenome.ToSwitchableGroup();

            Assert.AreEqual(switchableGroup.KeyCount, cKeyCount);
            Assert.AreEqual(switchableGroup.SwitchableCount, cGroupSize);
            Assert.AreEqual(switchableGroup.SwitchableDataType, cSwitchableGroupGenomeType.ToSwitchableGroupDataType());
        }


        [TestMethod]
        public void TestToSwitchableGroupUlong()
        {
            const int cSeed = 1234;
            const int cGroupSize = 100;
            const int cKeyCount = 60;
            const SwitchableGroupGenomeType cSwitchableGroupGenomeType = SwitchableGroupGenomeType.ULong;

            var switchableGroupGenome = Rando.Fast(cSeed).ToSwitchableGroupGenome
                (
                    switchableGroupGenomeType: cSwitchableGroupGenomeType,
                    keyCount: cKeyCount,
                    groupSize: cGroupSize
                );

            var switchableGroup = switchableGroupGenome.ToSwitchableGroup();

            Assert.AreEqual(switchableGroup.KeyCount, cKeyCount);
            Assert.AreEqual(switchableGroup.SwitchableCount, cGroupSize);
            Assert.AreEqual(switchableGroup.SwitchableDataType, cSwitchableGroupGenomeType.ToSwitchableGroupDataType());
        }

        [TestMethod]
        public void TestToSwitchableGroupIntArray()
        {
            const int cSeed = 1234;
            const int cGroupSize = 100;
            const int cKeyCount = 60;
            const SwitchableGroupGenomeType cSwitchableGroupGenomeType = SwitchableGroupGenomeType.IntArray;

            var switchableGroupGenome = Rando.Fast(cSeed).ToSwitchableGroupGenome
                (
                    switchableGroupGenomeType: cSwitchableGroupGenomeType,
                    keyCount: cKeyCount,
                    groupSize: cGroupSize
                );

            var switchableGroup = switchableGroupGenome.ToSwitchableGroup();

            Assert.AreEqual(switchableGroup.KeyCount, cKeyCount);
            Assert.AreEqual(switchableGroup.SwitchableCount, cGroupSize);
            Assert.AreEqual(switchableGroup.SwitchableDataType, cSwitchableGroupGenomeType.ToSwitchableGroupDataType());
        }

        //[TestMethod]
        //public void TestToSwitchableGroupBitArray()
        //{
        //    const int cSeed = 1234;
        //    const int cGroupSize = 100;
        //    const int cKeyCount = 60;
        //    const SwitchableGroupGenomeType cSwitchableGroupGenomeType = SwitchableGroupGenomeType.ULong;

        //    var switchableGroupGenome = Rando.Fast(cSeed).ToSwitchableGroupGenome
        //        (
        //            switchableGroupGenomeType: cSwitchableGroupGenomeType,
        //            keyCount: cKeyCount,
        //            groupSize: cGroupSize
        //        );

        //    var switchableGroup = switchableGroupGenome.ToSwitchableGroup();

        //    Assert.AreEqual(switchableGroup.KeyCount, cKeyCount);
        //    Assert.AreEqual(switchableGroup.SwitchableCount, cGroupSize);
        //    Assert.AreEqual(switchableGroup.SwitchableDataType, cSwitchableGroupGenomeType.ToSwitchableGroupDataType());
        //}



    }
}
