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
            var layer = SorterEvoTestData.SwitchableGroupLayer();

            Assert.AreEqual(layer.Generation, 0);
            Assert.AreEqual(layer.Genomes.Count, SorterEvoTestData.SwitchableGroupGenomeCount);
            Assert.AreEqual(layer.Genomes.First().KeyCount, SorterEvoTestData.KeyCount);
            Assert.AreEqual(layer.Genomes.First().GroupCount, SorterEvoTestData.SwitchableGroupSize);
        }

        [TestMethod]
        public void TestMultiply()
        {
            var layer = SorterEvoTestData.SwitchableGroupLayer();

            var newLayer = layer.Multiply
                (
                    seed: SorterEvoTestData.Seeds.First(),
                    newGenomeCount: SorterEvoTestData.SwitchableGroupExpandedGenomeCount,
                    mutationRate: SorterEvoTestData.SwitchableMutationRate,
                    insertionRate: SorterEvoTestData.SwitchableInsertionRate,
                    deletionRate: SorterEvoTestData.SwitchableDeletionRate
                );

            Assert.AreEqual(newLayer.Generation, 0);
            Assert.AreEqual(newLayer.Genomes.Count, SorterEvoTestData.SwitchableGroupExpandedGenomeCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyCount, SorterEvoTestData.KeyCount);
        }

        [TestMethod]
        public void TestNextGen()
        {
            var layer = SorterEvoTestData.SwitchableGroupExpandedLayer();

            var randy = Rando.Fast(1233);

            var newLayer = layer.NextGen
                (
                    scores: layer.Genomes.Select(g => new Tuple<Guid, double>(g.Guid, randy.NextDouble())).ToList(),
                    seed: SorterEvoTestData.Seeds.First(),
                    newGenomeCount: SorterEvoTestData.SwitchableGroupGenomeCount
                );

            Assert.AreEqual(newLayer.Generation, 1);
            Assert.AreEqual(newLayer.Genomes.Count, SorterEvoTestData.SwitchableGroupGenomeCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyCount, SorterEvoTestData.KeyCount);
        }
    }
}
