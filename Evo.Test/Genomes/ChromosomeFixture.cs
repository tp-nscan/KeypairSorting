using System;
using Evo.GenomeBuilders;
using Evo.Genomes;
using Evo.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evo.Test.Genomes
{
    [TestClass]
    public class ChromosomeFixture
    {
        [TestMethod]
        public void TestGenChromosome()
        {
            const int chromosomeLength = 700;
            const int symbolCount = 50;
            const int seed = 1277;
            var id = Guid.NewGuid();

            var chromo = ChromosomeBuildInfo.GenInfo(id, seed, chromosomeLength, symbolCount)
                                            .ToChromosome<int>();
            Assert.AreEqual(chromo.GeneCount, chromosomeLength);
            Assert.AreEqual(chromo.SymbolSet.Count, symbolCount);
            Assert.AreEqual(chromo.Id, id);
        }

        [TestMethod]
        public void TestCopyChromosome()
        {
            const int chromosomeLength = 700;
            const int symbolCount = 50;
            const int seed = 1277;
            var parentId = Guid.NewGuid();
            var childId = Guid.NewGuid();

            var parentChromo = ChromosomeBuildInfo.GenInfo(parentId, seed, chromosomeLength, symbolCount)
                .ToChromosome<int>();
            var chromRepository = ChromosomeRepository.WithTestData();
            chromRepository.AddChromosome(parentChromo);

            const double mutationRate = 0.1;
            const double insertionRate = 0.0;
            const double deletionRate = 0.0;

            var childChromo = ChromosomeBuildInfo.CopyInfo(childId, 987, new[] { parentChromo.Id }, deletionRate,
                mutationRate, insertionRate)
                .ToChromosome<int>(chromRepository);

            Assert.AreEqual(childChromo.GeneCount, chromosomeLength);
            Assert.AreEqual(childChromo.SymbolSet.Count, symbolCount);
            Assert.AreEqual(childChromo.Id, childId);
        }

    }
}
