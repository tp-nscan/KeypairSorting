using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.Test.Genomes;

namespace SorterEvo.Test.StateMonad
{
    [TestClass]
    public class LcgRandomFixture
    {
        [TestMethod]
        public void TestRand()
        {
            var seed = 20000;
            var a = LcgRandom.NextShort(seed);
            var b = LcgRandom.NextShort(a.Item2);
            var c = LcgRandom.NextShort(b.Item2);

            var res = a.Item1 + b.Item1 + c.Item1;
        }

        [TestMethod]
        public void TestRandStateless()
        {
            var result = from a in State.GetRandom()
                         from b in State.GetRandom()
                         from c in State.GetRandom()
                         select a + b + c;

            var seed = 0;
            var res = result.Computation(seed).Item2;

        }
    }
}
