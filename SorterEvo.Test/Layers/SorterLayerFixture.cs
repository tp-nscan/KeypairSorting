using System;
using System.Linq;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.Layers;
using SorterEvo.TestData;

namespace SorterEvo.Test.Layers
{
    [TestClass]
    public class SorterLayerFixture
    {
        [TestMethod]
        public void TestCreate()
        {
            var layer = SorterEvoTestData.SorterLayer();

            Assert.AreEqual(layer.Generation, 0);
            Assert.AreEqual(layer.Genomes.Count, SorterEvoTestData.SorterGenomeCount);
            Assert.AreEqual(layer.Genomes.First().KeyCount, SorterEvoTestData.KeyCount);
            Assert.AreEqual(layer.Genomes.First().KeyPairCount, SorterEvoTestData.KeyPairCount);
        }


        [TestMethod]
        public void TestMultiply()
        {
            var layer = SorterEvoTestData.SorterLayer();

            var randy = Rando.Fast(1233);
            
            var newLayer = layer.Multiply
                (
                    seed:SorterEvoTestData.Seeds.First(),
                    newGenomeCount: SorterEvoTestData.SorterExpandedGenomeCount,
                    mutationRate: SorterEvoTestData.SorterMutationRate,
                    insertionRate: SorterEvoTestData.SorterInsertionRate,
                    deletionRate: SorterEvoTestData.SorterDeletionRate
                );

            Assert.AreEqual(newLayer.Generation, 0);
            Assert.AreEqual(newLayer.Genomes.Count, SorterEvoTestData.SorterExpandedGenomeCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyCount, SorterEvoTestData.KeyCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyPairCount, SorterEvoTestData.KeyPairCount);
        }

        [TestMethod]
        public void TestNextGen()
        {
            var layer = SorterEvoTestData.SorterExpandedLayer();

            var randy = Rando.Fast(1233);

            var newLayer = layer.NextGen
                (
                    scores: layer.Genomes.Select(g => new Tuple<Guid, double>(g.Guid, randy.NextDouble())).ToList(),
                    seed: SorterEvoTestData.Seeds.First(),
                    newGenomeCount: SorterEvoTestData.SorterGenomeCount
                );

            Assert.AreEqual(newLayer.Generation, 1);
            Assert.AreEqual(newLayer.Genomes.Count, SorterEvoTestData.SorterGenomeCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyCount, SorterEvoTestData.KeyCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyPairCount, SorterEvoTestData.KeyPairCount);
        }
    }
}
