using System;
using System.Linq;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.Layers;
using SorterEvo.TestData;

namespace SorterEvo.Test.Layers
{
    [TestClass]
    public class SwitchableGroupLayerFixture
    {
        [TestMethod]
        public void TestCreate()
        {
            var layer = TestData.Layers.SwitchableGroupLayer();

            Assert.AreEqual(layer.Generation, 0);
            Assert.AreEqual(layer.Genomes.Count, TestData.Layers.SwitchableGroupGenomeCount);
            Assert.AreEqual(layer.Genomes.First().KeyCount, TestData.Layers.KeyCount);
            Assert.AreEqual(layer.Genomes.First().GroupCount, TestData.Layers.SwitchableGroupSize);
        }

        [TestMethod]
        public void TestMultiply()
        {
            var layer = TestData.Layers.SwitchableGroupLayer();

            var newLayer = layer.Reproduce
                (
                    seed: TestData.Layers.Seeds.First(),
                    newGenomeCount: TestData.Layers.SwitchableGroupExpandedGenomeCount,
                    mutationRate: TestData.Layers.SwitchableMutationRate,
                    insertionRate: TestData.Layers.SwitchableInsertionRate,
                    deletionRate: TestData.Layers.SwitchableDeletionRate
                );

            Assert.AreEqual(newLayer.Generation, 0);
            Assert.AreEqual(newLayer.Genomes.Count, TestData.Layers.SwitchableGroupExpandedGenomeCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyCount, TestData.Layers.KeyCount);
        }

        [TestMethod]
        public void TestNextGen()
        {
            var layer = TestData.Layers.SwitchableGroupExpandedLayer();

            var randy = Rando.Fast(1233);

            var newLayer = layer.NextGen
                (
                    scores: layer.Genomes.Select(g => new Tuple<Guid, double>(g.Guid, randy.NextDouble())).ToList(),
                    seed: TestData.Layers.Seeds.First(),
                    newGenomeCount: TestData.Layers.SwitchableGroupGenomeCount
                );

            Assert.AreEqual(newLayer.Generation, 1);
            Assert.AreEqual(newLayer.Genomes.Count, TestData.Layers.SwitchableGroupGenomeCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyCount, TestData.Layers.KeyCount);
        }


    }
}
