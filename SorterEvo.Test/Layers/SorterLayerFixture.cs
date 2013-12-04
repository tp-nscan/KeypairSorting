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
            var layer = TestSorterEvo.SorterLayer();

            Assert.AreEqual(layer.Generation, 0);
            Assert.AreEqual(layer.Genomes.Count, TestSorterEvo.GenomeCount);
            Assert.AreEqual(layer.Genomes.First().KeyCount, TestSorterEvo.KeyCount);
            Assert.AreEqual(layer.Genomes.First().KeyPairCount, TestSorterEvo.KeyPairCount);
        }


        [TestMethod]
        public void TestMultiply()
        {
            var layer = TestSorterEvo.SorterLayer();

            var randy = Rando.Fast(1233);
            
            var newLayer = layer.Multiply
                (
                    seed:TestSorterEvo.Seeds.First(),
                    newGenomeCount: TestSorterEvo.SorterLayerExpandedGenomeCount,
                    mutationRate: TestSorterEvo.SorterMutationRate,
                    insertionRate: TestSorterEvo.SorterInsertionRate,
                    deletionRate: TestSorterEvo.SorterDeletionRate
                );

            Assert.AreEqual(newLayer.Generation, 1);
            Assert.AreEqual(newLayer.Genomes.Count, TestSorterEvo.SorterLayerExpandedGenomeCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyCount, TestSorterEvo.KeyCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyPairCount, TestSorterEvo.KeyPairCount);
        }

        [TestMethod]
        public void TestNextGen()
        {
            var layer = TestSorterEvo.SorterLayer();

            var randy = Rando.Fast(1233);

            var newLayer = layer.NextGen
                (
                    scores: layer.Genomes.Select(g => new Tuple<Guid, double>(g.Guid, randy.NextDouble())).ToList(),
                    seed: TestSorterEvo.Seeds.First(),
                    newGenomeCount: TestSorterEvo.SorterLayerExpandedGenomeCount
                );

            Assert.AreEqual(newLayer.Generation, 1);
            Assert.AreEqual(newLayer.Genomes.Count, TestSorterEvo.SorterLayerExpandedGenomeCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyCount, TestSorterEvo.KeyCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyPairCount, TestSorterEvo.KeyPairCount);
        }
    }
}
