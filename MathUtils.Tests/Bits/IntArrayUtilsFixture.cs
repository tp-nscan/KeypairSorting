using System;
using System.Linq;
using MathUtils.Bits;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathUtils.Tests.Bits
{
    [TestClass]
    public class IntArrayUtilsFixture
    {
        [TestMethod]
        public void TestCompareDict()
        {
        }

        [TestMethod]
        public void TestIsSorted()
        {
            const int arrayLength = 10;
            var sortedArray = Enumerable.Range(0, arrayLength).ToArray();

            Assert.IsTrue(sortedArray.IsSorted());

            sortedArray = Enumerable.Range(1, arrayLength).ToArray();

            Assert.IsFalse(sortedArray.IsSorted());
        }


    }
}
