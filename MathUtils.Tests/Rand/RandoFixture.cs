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
        public void TestUints()
        {
            var rando = Rando.Fast(33);
            for (var i = 0; i < 550; i++)
            {
                var res = rando.NextUint();
                if (res > 2132955153)
                    System.Diagnostics.Debug.WriteLine(res);
            }
        }

        [TestMethod]
        public void TestRandomFlags()
        {
            var randomFlags = Rando.Fast(33).ToRandomUlongFlags(50).Take(5000).OrderByDescending(T => T).ToList();

            foreach (var randomFlag in randomFlags)
            {
                System.Diagnostics.Debug.WriteLine(randomFlag);
            }
        }

        [TestMethod]
        public void TestMutator()
        {
            var origList = Enumerable.Repeat(0, 10000).ToList();
            var mutatedList = origList.Mutate(Rando.Fast(123).ToBoolEnumerator(0.1), t => 1).ToList();
            var sum = mutatedList.Sum(t => t);
            Assert.IsTrue(sum < 1200);
            Assert.IsTrue(sum > 800);
        }


    }
}
