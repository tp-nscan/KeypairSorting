using System;
using System.Collections.Generic;
using MathUtils.Collections;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathUtils.Tests.Collections
{
    [TestClass]
    public class GuidFixture
    {
        [TestMethod]
        public void TestMakeGuidHash()
        {
            var result = Guid.NewGuid().ToHash();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestGuidHashErgodictiy()
        {
            var guidsAndKeys = new Dictionary<Guid, int>();
            var curGuid = Guid.NewGuid();

            for (var i = 0; i < 100000; i++)
            {
                var curSeed = curGuid.ToHash();
                guidsAndKeys[curGuid] = curSeed;
                var randy = Rando.Fast(curSeed);
                curGuid = randy.NextGuid();
                guidsAndKeys[curGuid] = curSeed;
                curGuid = randy.NextGuid();
                guidsAndKeys[curGuid] = curSeed;
                curGuid = randy.NextGuid();
                guidsAndKeys[curGuid] = curSeed;
                curGuid = randy.NextGuid();
                guidsAndKeys[curGuid] = curSeed;
                curGuid = randy.NextGuid();
                guidsAndKeys[curGuid] = curSeed;
                curGuid = randy.NextGuid();
                guidsAndKeys[curGuid] = curSeed;
                curGuid = randy.NextGuid();
            }
        }
    }
}
