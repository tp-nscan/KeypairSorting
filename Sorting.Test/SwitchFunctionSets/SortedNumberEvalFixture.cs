using System.Linq;
using MathUtils.Bits;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.SwitchFunctionSets;

namespace Sorting.Test.SwitchFunctionSets
{
    [TestClass]
    public class SortedNumberEvalFixture
    {
        [TestMethod]
        public void TestSortedUshort()
        {
            for (var i = 0; i < 16; i++)
            {
                var bitArray = Enumerable.Repeat(false, i).Concat(Enumerable.Repeat(true, 16 - i)).ToArray();
                var testNum = bitArray.ToUShort();
                Assert.IsTrue(SortedNumberEval.IsSorted(testNum));
            }
        }

        [TestMethod]
        public void TestRandomUshortForSorted()
        {
            var randy = Rando.Fast(322);
            for (var i = 0; i < 10000; i++)
            {
                var testNum = (ushort)randy.NextInt();
                var testArray = testNum.ToBits();
                Assert.AreEqual(SortedNumberEval.IsSorted(testNum), testArray.IsSorted());
            }
        }

        [TestMethod]
        public void TestSortedUint()
        {
            for (var i = 0; i < 32; i++)
            {
                var bitArray = Enumerable.Repeat(false, i).Concat(Enumerable.Repeat(true, 32 - i)).ToArray();
                var testNum = bitArray.ToUInt();
                Assert.IsTrue(SortedNumberEval.IsSorted(testNum));
            }
        }

        [TestMethod]
        public void TestRandomUintForSorted()
        {
            var randy = Rando.Fast(322);
            for (var i = 0; i < 10000; i++)
            {
                var testNum = (uint)randy.NextInt();
                var testArray = testNum.ToBits();
                Assert.AreEqual(SortedNumberEval.IsSorted(testNum), testArray.IsSorted());
            }
        }

        [TestMethod]
        public void TestSortedUlong()
        {
            for (var i = 0; i < 64; i++)
            {
                var bitArray = Enumerable.Repeat(false, i).Concat(Enumerable.Repeat(true, 64 - i)).ToArray();
                var testNum = bitArray.ToULong();
                Assert.IsTrue(SortedNumberEval.IsSorted(testNum));
            }
        }

        [TestMethod]
        public void TestRandomUlongForSorted()
        {
            var randy = Rando.Fast(322);
            for (var i = 0; i < 10000; i++)
            {
                var testNum = (ulong)randy.NextInt();
                testNum <<= 32;
                testNum += (ulong)randy.NextInt();
                var testArray = testNum.ToBits();
                Assert.AreEqual(SortedNumberEval.IsSorted(testNum), testArray.IsSorted());
            }
        }

    }
}
