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
            var layer = TestSorterEvo.SwitchableGroupLayer();

            Assert.AreEqual(layer.Generation, 0);
            Assert.AreEqual(layer.Genomes.Count, TestSorterEvo.SwitchableGroupGenomeCount);
            Assert.AreEqual(layer.Genomes.First().KeyCount, TestSorterEvo.KeyCount);
            Assert.AreEqual(layer.Genomes.First().GroupCount, TestSorterEvo.SwitchableGroupSize);
        }

        [TestMethod]
        public void TestMultiply()
        {
            var layer = TestSorterEvo.SwitchableGroupLayer();

            var newLayer = layer.Multiply
                (
                    seed: TestSorterEvo.Seeds.First(),
                    newGenomeCount: TestSorterEvo.SwitchableGroupExpandedGenomeCount,
                    mutationRate: TestSorterEvo.SwitchableMutationRate,
                    insertionRate: TestSorterEvo.SwitchableInsertionRate,
                    deletionRate: TestSorterEvo.SwitchableDeletionRate
                );

            Assert.AreEqual(newLayer.Generation, 1);
            Assert.AreEqual(newLayer.Genomes.Count, TestSorterEvo.SwitchableGroupExpandedGenomeCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyCount, TestSorterEvo.KeyCount);
        }

        [TestMethod]
        public void TestNextGen()
        {
            var layer = TestSorterEvo.SwitchableGroupLayer();

            var randy = Rando.Fast(1233);

            var newLayer = layer.NextGen
                (
                    scores: layer.Genomes.Select(g => new Tuple<Guid, double>(g.Guid, randy.NextDouble())).ToList(),
                    seed: TestSorterEvo.Seeds.First(),
                    newGenomeCount: TestSorterEvo.SwitchableGroupExpandedGenomeCount
                );

            Assert.AreEqual(newLayer.Generation, 1);
            Assert.AreEqual(newLayer.Genomes.Count, TestSorterEvo.SwitchableGroupExpandedGenomeCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyCount, TestSorterEvo.KeyCount);
        }
    }
}
