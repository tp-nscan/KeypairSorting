using System;
using System.Linq;
using Evo.Genomes;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evo.Test.Genomes
{
    [TestClass]
    public class SymbolSetFixture
    {
        [TestMethod]
        public void MakeFlagsUint()
        {
            var symbolSet = SymbolSet.Make<uint>(new UIntSymbols.Initializer(10));
            var randy = Rando.Fast(123);
            var avg = Enumerable.Range(0, 2000).Select(i => symbolSet.Choose(randy, (uint) i)).Average(t=>t);
            Assert.IsTrue(avg  > 400);
            Assert.IsTrue(avg < 600);
        }

        [TestMethod]
        public void MakeFlagsUlong()
        {
            var symbolSet = SymbolSet.Make<ulong>(new ULongSymbols.Initializer(50));
            var randy = Rando.Fast(123);
            var avg = Enumerable.Range(0, 1000).Select(i => symbolSet.Choose(randy, (ulong)i)).Average(t => (double)t);
            var mid = Math.Pow(2, 49);

            Assert.IsTrue(avg > 0.9 * mid);
            Assert.IsTrue(avg < 1.1 * mid);
        }
    }
}
