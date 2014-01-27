using System;
using System.Linq;
using Genomic.Chromosomes;
using MathUtils.Collections;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

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

        [TestMethod]
        public void RecombineChromosomes()
        {
            const uint symbolCount = 32;
            const double recombinationRate = 0.1;
            const int sequenceLength = 1000;


            var chromoA = Enumerable.Repeat((uint)1, sequenceLength)
                .ToList()
                .ToChromosomeUint(symbolCount);

            var chromoB = Enumerable.Repeat((uint)2, sequenceLength)
                .ToList()
                .ToChromosomeUint(symbolCount);


            //var recombinants = chromoA.Recombine(chromoB, Rando.Fast(222), recombinationRate);

            //System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(recombinants.Item1.Sequence));
            //System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(recombinants.Item2.Sequence));

            Assert.AreEqual(chromoA.Sequence.Count, sequenceLength);
        }

        [TestMethod]
        public void Recombi()
        {
            const uint symbolCount = 32;
            const double recombinationRate = 0.2;
            const int sequenceLength = 50;


            var chromoA = Enumerable.Repeat((uint)1, sequenceLength + 10)
                .ToList();

            var chromoB = Enumerable.Repeat((uint) 2, sequenceLength)
                .ToList();


            var recombinants = Chromosome.StandardRecombinator<uint>(Rando.Fast(123), recombinationRate)
                .Invoke(chromoA, chromoB);

            //var recombinants = chromoA.Recombine(chromoB, Rando.Fast(222), recombinationRate);

            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(recombinants.Item1));
            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(recombinants.Item2));

        }

    }
}
