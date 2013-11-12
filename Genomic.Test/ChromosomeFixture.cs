using System;
using System.Linq;
using System.Runtime.InteropServices;
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
            var guid = Guid.NewGuid();
            const uint symbolCount = 32;
            const int sequenceLength = 1000;
            var chromo = Rando.Fast(123).ToUniformChromosome(guid, symbolCount, sequenceLength);
            Assert.AreEqual(chromo.Guid, guid);
            Assert.AreEqual(chromo.SymbolSet.MaxVal, symbolCount);
            Assert.AreEqual(chromo.Sequence.Count, sequenceLength);
        }

        [TestMethod]
        public void CopyChromosome()
        {
            var guid = Guid.NewGuid();
            const uint symbolCount = 32;
            const int sequenceLength = 10000;
            var chromo = Rando.Fast(123).ToUniformChromosome(guid, symbolCount, sequenceLength);

            var newGuid = Guid.NewGuid();
            const int seed = 1234;
            const double mutationRate = 0.1;
            const double insertionRate = 0.1;
            const double deletionRate = 0;

            var newChromo = chromo.Copy(newGuid, seed, mutationRate, mutationRate, mutationRate);

            var diffCount = newChromo.Sequence.GetDifferentItems(chromo.Sequence).Count();

            Assert.AreEqual(newChromo.Guid, newGuid);
            Assert.AreEqual(newChromo.SymbolSet.MaxVal, symbolCount);
            Assert.AreEqual(newChromo.Sequence.Count, sequenceLength);
            Assert.IsTrue(diffCount < sequenceLength*mutationRate*1.1);
            Assert.IsTrue(diffCount > sequenceLength * mutationRate * 0.9);
        }

    }
}
