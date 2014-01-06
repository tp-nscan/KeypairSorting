using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Chromosomes;
using Genomic.Json.Chromosomes;
using MathUtils.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genomic.Json.Test.Chromosomes
{
    [TestClass]
    public class ChromosomeToJsonFixture
    {
        [TestMethod]
        public void ConvertChromosomeUintTest()
        {
            const uint maxVal = 25;
            const int sequenceLength = 100;

            IReadOnlyList<uint> sequence = Enumerable.Range(0, (int) maxVal).Select(t=>(uint)t)
                                            .Repeat().Take(sequenceLength).ToList();
                
            var chromosomeUint = ChromosomeUint.Make
                                (
                                    sequence: sequence,
                                    maxVal: maxVal
                                );

            var serialized = chromosomeUint.ToJsonString();

            var newChromosomeUint = serialized.ToChromosomeUint();

            Assert.AreEqual(newChromosomeUint.MaxVal, maxVal);
            Assert.IsTrue(newChromosomeUint.Sequence.IsSameAs(sequence));
        }
    }
}
