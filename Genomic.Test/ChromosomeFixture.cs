using System;
using System.Linq;
using Genomic.Chromosomes;
using MathUtils.Collections;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genomic.Test
{
    [TestClass]
    public class ChromosomeFixture
    {
        [TestMethod]
        public void MakeChromosome()
        {
            const uint symbolCount = 32;
            const int sequenceLength = 1000;
            var chromo = Rando.Fast(123)
                .ToUintEnumerator(symbolCount)
                .Take(sequenceLength)
                .ToList()
                .ToChromosomeUint(symbolCount);

            Assert.AreEqual(chromo.Sequence.Count, sequenceLength);
        }

        [TestMethod]
        public void MutateChromosome()
        {
            const uint symbolCount = 32;
            const int sequenceLength = 10000;

            var chromo = Rando.Fast(123)
                .ToUintEnumerator(symbolCount)
                .Take(sequenceLength)
                .ToList()
                .ToChromosomeUint(symbolCount);

            const int seed = 1234;
            const double mutationRate = 0.1;
            const double insertionRate = 0.0;
            const double deletionRate = 0.0;

            var newChromo = chromo.StandardPropigate
                (
                    rando: Rando.Fast(seed),
                    mutationRate: mutationRate,
                    insertionRate: insertionRate,
                    deletionRate: deletionRate
                );

            var diffCount = newChromo.Sequence.GetDifferentItems(chromo.Sequence).Count();

            Assert.AreEqual(newChromo.Sequence.Count, sequenceLength);
            Assert.IsTrue(diffCount < sequenceLength * mutationRate * 1.1);
            Assert.IsTrue(diffCount > sequenceLength * mutationRate * 0.9);
        }

    }
}
