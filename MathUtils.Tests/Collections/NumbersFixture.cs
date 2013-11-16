using MathUtils.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathUtils.Tests.Collections
{
    [TestClass]
    public class NumbersFixture
    {
        [TestMethod]
        public void TestHiBitUlong()
        {
            for (var shift = 0; shift < 64; shift++)
            {
                var testVal = ((ulong)1) << shift;
                Assert.AreEqual(testVal.HiBit(), shift);
            }
        }



    }
}
