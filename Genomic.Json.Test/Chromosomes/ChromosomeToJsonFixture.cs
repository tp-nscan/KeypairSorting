using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Chromosomes;
using Genomic.Json.Chromosomes;
using MathUtils.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

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
            Guid guid = Guid.NewGuid();

            IReadOnlyList<uint> sequence = Enumerable.Range(0, (int) maxVal).Select(t=>(uint)t)
                                            .Repeat().Take(sequenceLength).ToList();
                
            var chromosomeUint = ChromosomeUint.Make
                                (
                                    guid: guid,
                                    sequence: sequence,
                                    maxVal: maxVal
                                );

            var serialized = JsonConvert.SerializeObject(ChromosomeUintToJson.ToJsonAdapter(chromosomeUint), Formatting.Indented);
            var deserialized = JsonConvert.DeserializeObject<ChromosomeUintToJson>(serialized);
            var newChromosomeUint = ChromosomeUintToJson.ToChromosomeUint(deserialized);

            Assert.AreEqual(newChromosomeUint.Guid, guid);
            Assert.AreEqual(newChromosomeUint.MaxVal, maxVal);
            Assert.IsTrue(newChromosomeUint.Sequence.IsSameAs(sequence));
        }
    }
}
