using System.Linq;
using Genomic.Genes;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genomic.Test
{
    [TestClass]
    public class ChromosomeBlockFixture
    {
        [TestMethod]
        public void TestModNBlockUlong()
        {
            const int maxBits = 60;
            foreach (var value in Rando.Fast(123).ToUlongEnumerator(((ulong)1) << (maxBits - 1)).Take(50))
            {
                var longBlock = GeneUlongModN.Make(value, maxBits);
                var serialized = longBlock.ToIntStream.ToArray();
                var longBack = GeneUlongModN.Make(serialized, maxBits);
                Assert.AreEqual(longBack.Val, value);
            }
        }
    }
}