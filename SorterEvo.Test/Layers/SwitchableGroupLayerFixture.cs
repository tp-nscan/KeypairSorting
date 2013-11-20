using System;
using System.Linq;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.Genomes;
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
            const int cGroupCount = 1000;
            const SwitchableGroupGenomeType switchableGroupGenomeType = SwitchableGroupGenomeType.UInt;

            var layer = SwitchableGroupLayer.Create(
                    seed: cSeed,
                    switchableGroupGenomeType: switchableGroupGenomeType,
                    genomeCount: cGenomes,
                    keyCount: cKeys,
                    groupSize: cGroupCount
                );

            Assert.AreEqual(layer.Generation, 0);
            Assert.AreEqual(layer.Genomes.Count, cGenomes);
            Assert.AreEqual(layer.Genomes.First().KeyCount, cKeys);
            Assert.AreEqual(layer.Genomes.First().GroupCount, cGroupCount);
        }

        [TestMethod]
        public void TestUpdate()
        {
            const int cSeed = 1234;
            const int cGenomes = 100;
            const int cKeys = 16;
            const int cKeyPairs = 1000;
            const double mutationRate = 0.3;
            const double deletionRate = 0.3;
            const double insertionRate = 0.3;
            const int cSelectionRatio = 4;
            const int cGroupCount = 1000;
            const SwitchableGroupGenomeType switchableGroupGenomeType = SwitchableGroupGenomeType.UInt;

            var layer = SwitchableGroupLayer.Create(
                    seed: cSeed,
                    switchableGroupGenomeType: switchableGroupGenomeType,
                    genomeCount: cGenomes,
                    keyCount: cKeys,
                    groupSize: cGroupCount
                );



            var randy = Rando.Fast(1233);

            var newLayer = layer.Update

                (
                    scores: layer.Genomes.Select(g => new Tuple<Guid, double>(g.Guid, randy.NextDouble())).ToList(),
                    selectionRatio: cSelectionRatio,
                    mutationRate: mutationRate,
                    insertionRate: insertionRate,
                    deletionRate: deletionRate
                );
                
            Assert.AreEqual(newLayer.Generation, 1);
            Assert.AreEqual(newLayer.Genomes.Count, cGenomes);
            Assert.AreEqual(newLayer.Genomes.First().KeyCount, cKeys);
            Assert.AreEqual(newLayer.Genomes.First().GroupCount, cKeyPairs);
        }

        [TestMethod]
        public void TestMultipleUpdate()
        {
            const int cSeed = 1234;
            const int cGenomes = 100;
            const int cKeys = 16;
            const int cKeyPairs = 1000;
            const double mutationRate = 0.3;
            const double deletionRate = 0.3;
            const double insertionRate = 0.3;
            const int cSelectionRatio = 4;
            const int cGroupCount = 1000;
            const SwitchableGroupGenomeType switchableGroupGenomeType = SwitchableGroupGenomeType.UInt;

            var layer = SwitchableGroupLayer.Create(
                    seed: cSeed,
                    switchableGroupGenomeType: switchableGroupGenomeType,
                    genomeCount: cGenomes,
                    keyCount: cKeys,
                    groupSize: cGroupCount
                );


           
            var randy = Rando.Fast(1233);

            var newLayer = layer.Update
                (
                    scores: layer.Genomes.Select(g => new Tuple<Guid, double>(g.Guid, randy.NextDouble())).ToList(),
                    selectionRatio: cSelectionRatio,
                    mutationRate: mutationRate,
                    insertionRate: insertionRate,
                    deletionRate: deletionRate
                );


            var retainedGenomes = layer.Genomes.Count(t => newLayer.GetGenome(t.Guid) != null);

            var newLayer2 = newLayer.Update
            (
                scores: newLayer.Genomes.Select(g => new Tuple<Guid, double>(g.Guid, randy.NextDouble())).ToList(),
                selectionRatio: cSelectionRatio,
                mutationRate: mutationRate,
                insertionRate: insertionRate,
                deletionRate: deletionRate
            );


            var retainedGenomes2 = layer.Genomes.Count(t => newLayer2.GetGenome(t.Guid) != null);

            Assert.AreEqual(newLayer.Generation, 1);
            Assert.AreEqual(newLayer.Genomes.Count, cGenomes);
            Assert.AreEqual(newLayer.Genomes.First().KeyCount, cKeys);
            Assert.AreEqual(newLayer.Genomes.First().GroupCount, cKeyPairs);
        }

    }
}
