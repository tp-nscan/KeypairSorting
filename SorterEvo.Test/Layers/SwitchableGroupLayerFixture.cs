using System;
using System.Linq;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.Layers;

namespace SorterEvo.Test.Layers
{
    [TestClass]
    public class SwitchableGroupLayerFixture
    {
        [TestMethod]
        public void TestCreate()
        {
            const int cSeed = 1234;
            const int cGenomes = 100;
            const int cKeys = 16;
            const int cKeyPairs = 1000;

            var layer = SorterLayer.Create(
                seed: cSeed,
                genomeCount: cGenomes,
                keyCount: cKeys,
                keyPairCount: cKeyPairs
                );

            Assert.AreEqual(layer.Generation, 0);
            Assert.AreEqual(layer.Genomes.Count, cGenomes);
            Assert.AreEqual(layer.Genomes.First().KeyCount, cKeys);
            Assert.AreEqual(layer.Genomes.First().KeyPairCount, cKeyPairs);
        }

        [TestMethod]
        public void TestUpdate()
        {
            const int cSeed = 1234;
            const int cGenomes = 100;
            const int cKeys = 16;
            const int cKeyPairs = 1000;

            var layer = SorterLayer.Create(
                seed: cSeed,
                genomeCount: cGenomes,
                keyCount: cKeys,
                keyPairCount: cKeyPairs
                );

            var randy = Rando.Fast(1233);

            var newLayer = layer.Update
                (
                    scores: layer.Genomes.Select(g => new Tuple<Guid, double>(g.Guid, randy.NextDouble())).ToList(),
                    selectionRatio: 4,
                    mutationRate: 0.1,
                    insertionRate: 0.1,
                    deletionRate: 0.1
                );

            Assert.AreEqual(newLayer.Generation, 1);
            Assert.AreEqual(newLayer.Genomes.Count, cGenomes);
            Assert.AreEqual(newLayer.Genomes.First().KeyCount, cKeys);
            Assert.AreEqual(newLayer.Genomes.First().KeyPairCount, cKeyPairs);
        }
    }
}
