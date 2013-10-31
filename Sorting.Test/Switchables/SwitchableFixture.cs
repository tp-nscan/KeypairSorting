using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.Switchables;

namespace Sorting.Test.Switchables
{
    [TestClass]
    public class SwitchableFixture
    {
        [TestMethod]
        public void AllSwitchablesForKeyCount()
        {
            const int keyCountSize = 8;
            Assert.AreEqual(Switchable.AllUintEnumerablesForKeyCount(keyCountSize).ToArray().Length, Math.Pow(2, keyCountSize));
        }
    }
}
