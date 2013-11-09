using System;
using System.Linq;
using Evo.GenomeBuilders;
using Evo.Genomes;
using Evo.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evo.Test.Genomes
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGenGenome()
        {
            const int chromosomeLength = 700;
            const int symbolCount = 50;
            const int seed = 1277;
            var genomeId = Guid.NewGuid();
            var chromosomeId = Guid.NewGuid();
            var chromRepository = ChromosomeRepository.WithTestData();

            var genome = GenomeBuildInfo.SingleChromosomeGen
                (
                    genomeId: genomeId,
                    chromosomeId: chromosomeId,
                    seed: seed,
                    chromosomeLength: chromosomeLength,
                    symbolCount: symbolCount
                ).ToGenome(chromRepository);

            Assert.AreEqual(genome.Guid, genomeId);
            Assert.AreEqual(genome.GenomeBuildInfo.TargetId, genomeId);
            Assert.AreEqual(genome.Chromosomes.First().Id, chromosomeId);
            Assert.AreEqual(genome.Chromosomes.First().GeneCount, chromosomeLength);
        }

        [TestMethod]
        public void TestCopyGenome()
        {
            const int chromosomeLength = 700;
            const int symbolCount = 50;
            const int parentSeed = 1277;
            const int childSeed = 12777;
            var parentGenomeId = Guid.NewGuid();
            var parentChromosomeId = Guid.NewGuid();
            var childGenomeId = Guid.NewGuid();
            var childChromosomeId = Guid.NewGuid();

            var chromRepository = ChromosomeRepository.WithTestData();

            var parentGenome = GenomeBuildInfo.SingleChromosomeGen
            (
                genomeId: parentGenomeId,
                chromosomeId: parentChromosomeId,
                seed: parentSeed,
                chromosomeLength: chromosomeLength,
                symbolCount: symbolCount
            ).ToGenome(chromRepository);

            chromRepository.AddChromosome(parentGenome.Chromosomes.First());

            const double mutationRate = 0.1;
            const double insertionRate = 0.0;
            const double deletionRate = 0.0;

            var childGenome = GenomeBuildInfo.SingleChromosomeCopy
                (
                    targetGenomeId:childGenomeId,
                    targetChromosomeId:childChromosomeId,
                    parentGenomeId:parentGenomeId,
                    parentChromosomeId:parentChromosomeId,
                    seed:childSeed,
                    deletionRate: deletionRate,
                    mutationRate: mutationRate,
                    insertionRate: insertionRate
                ).ToGenome(chromRepository);

            var parentChromosome = (IChromosome<int>) parentGenome.Chromosomes[0];
            var childChromosome = (IChromosome<int>)childGenome.Chromosomes[0];

            var diffs = parentChromosome.Genes.Where
                (
                    (t, i) => parentChromosome.Genes[i] != childChromosome.Genes[i]
                ).ToList();

            Assert.AreEqual(childGenome.Guid, childGenomeId);
            Assert.IsTrue(diffs.Count < 100);
            Assert.IsTrue(diffs.Count > 50);
        }
    }
}
