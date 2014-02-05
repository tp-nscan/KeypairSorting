using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SorterEvo.Json.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var pcs = Enumerable.Range(1, 10).ToList();

            var part = pcs.Take(5);

            Assert.AreEqual(part.Count(), 5);
        }
    }
}
