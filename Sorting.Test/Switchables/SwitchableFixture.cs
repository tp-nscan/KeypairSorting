using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Rand;
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
            Assert.AreEqual(Switchable.AllSwitchablesForKeyCount(keyCountSize).ToArray().Length, Math.Pow(2, keyCountSize));
        }


        [TestMethod]
        public void SwitchableConstructions()
        {
            const int switchCount = 100;
            var switchablesUint = Rando.Fast(123).MakeSwitchables<uint>(16).Take(switchCount).ToList();
            Assert.AreEqual(switchablesUint.Count(), switchCount);

            var switchablesUlong = Rando.Fast(123).MakeSwitchables<ulong>(16).Take(switchCount).ToList();
            Assert.AreEqual(switchablesUlong.Count(), switchCount);

            var switchablesBa = Rando.Fast(123).MakeSwitchables<bool[]>(16).Take(switchCount).ToList();
            Assert.AreEqual(switchablesBa.Count(), switchCount);

            var switchablesIa = Rando.Fast(123).MakeSwitchables<int[]>(16).Take(switchCount).ToList();
            Assert.AreEqual(switchablesIa.Count(), switchCount);

        }


    }
}
