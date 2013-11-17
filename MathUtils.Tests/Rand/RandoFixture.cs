using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathUtils.Tests.Rand
{
    [TestClass]
    public class RandoFixture
    {
        [TestMethod]
        public void TestMutator()
        {
            var origList = Enumerable.Repeat(0, 10000).ToList();
            var mutatedList = origList.Mutate(Rando.Fast(123).ToBoolEnumerator(0.1), t => 1).ToList();
            var sum = mutatedList.Sum(t => t);
            Assert.IsTrue(sum < 1200);
            Assert.IsTrue(sum > 800);
        }

        [TestMethod]
        public void TestUintWithLimit()
        {
            const uint maxVal = 100;
            var avg = Rando.Fast(8823).ToUintEnumerator(maxVal).Take(10000).Average(t => t);
            Assert.IsTrue(avg < (maxVal/2));
            Assert.IsTrue(avg > (maxVal / 2 - 1));
        }

        [TestMethod]
        public void TestUlongWithLimit()
        {
            const ulong maxVal = 100000000000;
            var avg = Rando.Fast(8823).ToUlongEnumerator(maxVal).Take(10000).Average(t => (long) t);
            Assert.IsTrue(avg < (maxVal * 0.53));
            Assert.IsTrue(avg > (maxVal * 0.47));
        }
    }
}
