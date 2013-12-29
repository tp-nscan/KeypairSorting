using System;
using MathUtils.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathUtils.Tests
{
    [TestClass]
    public class StringExtFixture
    {
        [TestMethod]
        public void TestTabChunker()
        {
            var stringIn = "ABC123ABC12";
            var chunked = stringIn.TabChunk(3);
            Assert.AreEqual(chunked, "ABC\t123\tABC\t12");
        }
    }
}
