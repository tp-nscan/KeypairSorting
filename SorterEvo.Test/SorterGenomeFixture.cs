using System;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.KeyPairs;

namespace SorterEvo.Test
{
    [TestClass]
    public class SorterGenomeFixture
    {
        [TestMethod]
        public void TestRandomSorterGenome()
        {
            const int keyCount = 16;
            const int keyPairCount = 300;
            const int seed = 1222;

            var sorterGenome = Rando.Fast(seed).ToSorterGenome(keyCount, keyPairCount);

            Assert.IsTrue(sorterGenome.Chromosome.Sequence.Count == keyPairCount);
        }

        [TestMethod]
        public void TestSorterGenomeToSorter()
        {
            const int keyCount = 16;
            const int keyPairCount = 300;
            const int seed = 1222;

            var sorterGenome = Rando.Fast(seed).ToSorterGenome(keyCount, keyPairCount);

            var sorter = sorterGenome.ToSorter();

            Assert.IsTrue(sorter.KeyCount == keyCount);
            Assert.IsTrue(sorter.KeyPairs.Count == keyPairCount);
        }


    }
}
