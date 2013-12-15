using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathUtils.Tests.Rand
{
    [TestClass]
    public class RandSortFixture
    {
        public IList<Tuple<int, double>> NumTuples()
        {
            var rando = Rando.Fast(123);
            return Enumerable.Range(0, 5).Repeat().Take(25)
                .Select(i => new Tuple<int, double>(i, rando.NextDouble()))
                .ToList();
        }

        [TestMethod]
        public void TestMethod1()
        {
            var preSort = NumTuples();
            var testResult = preSort
                .SubSortShuffle(t => t.Item1, 1234)
                .ToList();
        }
    }
}
