using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.Genomes;

namespace SorterEvo.Test.Genomes
{
    [TestClass]
    public class GenomeToSorterFixture
    {
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
