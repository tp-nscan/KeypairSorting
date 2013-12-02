using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.Genomes;

namespace SorterEvo.Test.Genomes
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
    }
}
