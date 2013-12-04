//using System;
//using System.Linq;
//using Genomic.Genomes;
//using Genomic.Layers;
//using Genomic.Trackers;
//using MathUtils.Rand;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using SorterEvo.Genomes;
//using SorterEvo.Layers;
//using SorterEvo.TestData;

//namespace SorterEvo.Test.Layers
//{
//    [TestClass]
//    public class SwitchableGroupGenomeEvoFixture
//    {
//        [TestMethod]
//        public void TestMethod1()
//        {
//            const double mutationRate = 0.3;
//            const double deletionRate = 0.3;
//            const double insertionRate = 0.3;
//            const int cSelectionRatio = 2;

//            var layer = TestSorterEvo.SwitchableGroupLayer();

//            var randy = Rando.Fast(1233);

//            var genomePoolEvo = GenomePoolHistory.Make<ISwitchableGroupGenome>();

//            genomePoolEvo.AddGenomeEvos(layer.Genomes.Select(g => GenomeTracker.Make(genome: g, referenceScore: 0.0)));

//            var genomeLayerResults =
//                layer.Genomes.Select(g => GenomeEval.Make(g, 0, randy.NextDouble())).ToList();

//            genomePoolEvo.AddGenomeLayerResults(genomeLayerResults);

//            var newLayer = layer.Update
//                (
//                    scores: genomeLayerResults.Select(g => new Tuple<Guid, double>(g.Genome.Guid, g.Score)).ToList(),
//                    seed:TestSorterEvo.Seeds.First(),
//                    selectionRatio: cSelectionRatio,
//                    mutationRate: mutationRate,
//                    insertionRate: insertionRate,
//                    deletionRate: deletionRate
//                );

//            var genomeLayerResults2 =
//                    newLayer.Genomes.Select(g => GenomeEval.Make(g, 1, randy.NextDouble())).ToList();

//            var genomeLayerResults2P5 =
//                    genomePoolEvo.AddGenomeLayerResults(genomeLayerResults2).ToList();


//            var newGenomes2 = genomeLayerResults2P5.Select(r => r.Genome).ToList();
//            genomePoolEvo.AddGenomeEvos(newGenomes2.Select(g => GenomeTracker.Make(g, 0.0)));
//            genomePoolEvo.AddGenomeLayerResults(genomeLayerResults2P5);

//            var newLayer2 = newLayer.Update
//            (
//                scores: genomeLayerResults2.Select(g => new Tuple<Guid, double>(g.Genome.Guid, g.Score)).ToList(),
//                seed:TestSorterEvo.Seeds.First(),
//                selectionRatio: cSelectionRatio,
//                mutationRate: mutationRate,
//                insertionRate: insertionRate,
//                deletionRate: deletionRate
//            );

//            var genomeLayerResults3 =
//                    newLayer2.Genomes.Select(g => GenomeEval.Make(g, 2, randy.NextDouble())).ToList();

//            var genomeLayerResults3P5 =
//                    genomePoolEvo.AddGenomeLayerResults(genomeLayerResults3).ToList();

//            var newGenomes3 = genomeLayerResults3P5.Select(r => r.Genome).ToList();
//            genomePoolEvo.AddGenomeEvos(newGenomes3.Select(g => GenomeTracker.Make(g, 0.0)));
//            genomePoolEvo.AddGenomeLayerResults(genomeLayerResults3P5);

//            foreach (var genomeEvo in genomePoolEvo.GenomeEvos)
//            {
//                System.Diagnostics.Debug.WriteLine("{0} {1}", genomeEvo.Genome.Guid, genomeEvo.GenomeEvals.Count());
//            }

//            Assert.AreEqual(newLayer.Generation, 1);
//            Assert.AreEqual(newLayer.Genomes.Count, TestSorterEvo.GenomeCount);
//            Assert.AreEqual(newLayer.Genomes.First().KeyCount, TestSorterEvo.KeyCount);
//            Assert.AreEqual(newLayer.Genomes.First().GroupCount, TestSorterEvo.SwitchableGroupSize);
//        }
//    }
//}
