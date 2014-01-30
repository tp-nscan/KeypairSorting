using System;
using System.Linq;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Sorting.Sorters;

namespace Sorting.Test.Sorters
{
    [TestClass]
    public class StagedSorterFixture
    {
        [TestMethod]
        public void TestAStagedSorter()
        {
            const int keyCount = 14;
            const int keyPairCount = 60;

            var sorter = Rando.Fast(1243).ToSorter(keyCount, keyPairCount, Guid.NewGuid());

            var stagedSorter = sorter.ToStagedSorter();

            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(sorter.KeyPairs.Select(kp => kp.Index).ToList()));

            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(stagedSorter.KeyPairs.Select(kp => kp.Index).ToList()));

            Assert.AreEqual(stagedSorter.KeyPairCount, sorter.KeyPairCount);
            
        }

    }
}
