using System;
using System.Linq;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.Json.Genomes;
using SorterEvo.Layers;
using SorterEvo.TestData;

namespace SorterEvo.Test.Layers
{
    [TestClass]
    public class SorterLayerFixture
    {
        [TestMethod]
        public void TestMethod1()
        {
            var pcs = Enumerable.Range(1, 10).ToList();

            var part = pcs.Take(15).ToList();

            Assert.AreEqual(part.Count(), 10);
        }

        [TestMethod]
        public void TestCreate()
        {
            var layer = TestData.Layers.SorterLayer();

            Assert.AreEqual(layer.Generation, 0);
            Assert.AreEqual(layer.Genomes.Count, TestData.Layers.PopulationSize);
            Assert.AreEqual(layer.Genomes.First().KeyCount, TestData.Layers.KeyCount);
            Assert.AreEqual(layer.Genomes.First().KeyPairCount, TestData.Layers.KeyPairCount);
        }


        [TestMethod]
        public void TestMultiply()
        {
            var layer = TestData.Layers.SorterLayer();

            var randy = Rando.Fast(1233);
            
            var newLayer = layer.ReproducePreserveParents
                (
                    seed: TestData.Layers.Seeds.First(),
                    newGenomeCount: TestData.Layers.ChildCount,
                    mutationRate: TestData.Layers.SorterMutationRate,
                    insertionRate: TestData.Layers.SorterInsertionRate,
                    deletionRate: TestData.Layers.SorterDeletionRate
                );

            Assert.AreEqual(newLayer.Generation, 0);
            Assert.AreEqual(newLayer.Genomes.Count, TestData.Layers.ChildCount);
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
                    newGenomeCount: TestData.Layers.PopulationSize
                );

            Assert.AreEqual(newLayer.Generation, 1);
            Assert.AreEqual(newLayer.Genomes.Count, TestData.Layers.PopulationSize);
            Assert.AreEqual(newLayer.Genomes.First().KeyCount, TestData.Layers.KeyCount);
            Assert.AreEqual(newLayer.Genomes.First().KeyPairCount, TestData.Layers.KeyPairCount);
        }

        [TestMethod]
        public void TestRecommbinate()
        {
            var layer = TestData.Layers.SorterLayer();

            var randy = Rando.Fast(1233);

            foreach (var sorterGenome in layer.Genomes)
            {
                System.Diagnostics.Debug.WriteLine(sorterGenome.ToJsonString());
            }

            var newLayer = layer.Recombinate(0.0, 55);

            System.Diagnostics.Debug.WriteLine("--------------");

            foreach (var sorterGenome in newLayer.Genomes)
            {
                System.Diagnostics.Debug.WriteLine(sorterGenome.ToJsonString());
            }

        }
    }
}
