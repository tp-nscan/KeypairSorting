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
            var layer = TestData.Layers.SorterLayer();

            Assert.AreEqual(layer.Generation, 0);
            Assert.AreEqual(layer.Genomes.Count, TestData.Layers.SorterGenomeCount);
            Assert.AreEqual(layer.Genomes.First().KeyCount, TestData.Layers.KeyCount);
            Assert.AreEqual(layer.Genomes.First().KeyPairCount, TestData.Layers.KeyPairCount);
        }


        [TestMethod]
        public void TestMultiply()
        {
            var layer = TestData.Layers.SorterLayer();

            var randy = Rando.Fast(1233);
            
            var newLayer = layer.Reproduce
                (
                    seed: TestData.Layers.Seeds.First(),
                    newGenomeCount: TestData.Layers.SorterExpandedGenomeCount,
                    mutationRate: TestData.Layers.SorterMutationRate,
                    insertionRate: TestData.Layers.SorterInsertionRate,
                    deletionRate: TestData.Layers.SorterDeletionRate
                );

            Assert.AreEqual(newLayer.Generation, 0);
            Assert.AreEqual(newLayer.Genomes.Count, TestData.Layers.SorterExpandedGenomeCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyCount, TestData.Layers.KeyCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyPairCount, TestData.Layers.KeyPairCount);
        }

        [TestMethod]
        public void TestNextGen()
        {
            var layer = TestData.Layers.SorterExpandedLayer();

            var randy = Rando.Fast(1233);

            var newLayer = layer.NextGen
                (
                    scores: layer.Genomes.Select(g => new Tuple<Guid, double>(g.Guid, randy.NextDouble())).ToList(),
                    seed: TestData.Layers.Seeds.First(),
                    newGenomeCount: TestData.Layers.SorterGenomeCount
                );

            Assert.AreEqual(newLayer.Generation, 1);
            Assert.AreEqual(newLayer.Genomes.Count, TestData.Layers.SorterGenomeCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyCount, TestData.Layers.KeyCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyPairCount, TestData.Layers.KeyPairCount);
        }
    }
}
