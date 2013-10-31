using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.KeyPairs;
using Sorting.Sorters;
using Sorting.Switchables;
using Sorting.SwitchFunctionSets;

namespace Sorting.Test.Sorters
{
    [TestClass]
    public class SortingFunctionsFixture
    {

        [TestMethod]
        public void TestASorter()
        {
            var stopwatch = new Stopwatch();
            const int keyCount = 16;
            const int keyPairCount = 2000;
            const int switchableCount = 1000;

            var switchSet = new UintSwitchSet(keyCount);
            var sorter = Sorter.RandomSorter(212, keyCount, keyPairCount);
            var switchableSortingRuns = Rando.Fast(1234).ToUintSwitchables(keyCount)
                                            .Take(switchableCount)
                                            .ToList();

            stopwatch.Start();

            var result = sorter.Sort(switchSet, switchableSortingRuns);

            stopwatch.Stop();

            var score = result.UseCount;

            Debug.WriteLine("Time(ms): {0}", stopwatch.ElapsedMilliseconds);
        }


        //[TestMethod]
        //public void TestASorter()
        //{
        //    var stopwatch = new Stopwatch();
        //    const int keyCount = 16;
        //    const int keyPairCount = 200;
        //    const int switchableCount = 100;

        //    var switchSet = new UintSwitchSet(keyCount);
        //    var sorter = Sorter.RandomSorter(212, keyCount, keyPairCount);
        //    var switchableSortingRuns = Rando.Fast(1234).ToUintSwitchables(keyCount).Take(switchableCount)
        //                                        .Select(t => t.ToSwitchableSortingRun()).ToList();
        //    var switchUseList = Enumerable.Repeat(0, keyPairCount).ToList();

        //    stopwatch.Start();

        //    var result = sorter.SortMany(switchSet, switchableSortingRuns, switchUseList);

        //    stopwatch.Stop();

        //    var score = switchUseList.Count(t => t > 0);

        //    Debug.WriteLine("Time(ms): {0}", stopwatch.ElapsedMilliseconds);
        //}

        //[TestMethod]
        //public void TestASorter2()
        //{
        //    var stopwatch = new Stopwatch();
        //    const int keyCount = 16;
        //    const int keyPairCount = 200;
        //    const int switchableCount = 1000;

        //    var switchSet = new UintSwitchSet(keyCount);
        //    var sorter = Sorter.RandomSorter(212, keyCount, keyPairCount);
        //    var sorter2 = Sorter.RandomSorter(2132, keyCount, keyPairCount);
        //    var switchables = Rando.Fast(1234).ToUintSwitchables(keyCount).Take(switchableCount).ToList();

        //    stopwatch.Start();

        //    var results = SortingFunctions.TestWith
        //        (
        //            sorter: sorter,
        //            switchables: switchables
        //        );


        //    var results2 = SortingFunctions.TestWith
        //        (
        //            sorter: sorter2,
        //            switchables: switchables
        //        );

        //    stopwatch.Stop();

        //    var switchUseCount = results.SwitchUseList.Count(t => t > 0);
        //    var sucesses = results.SortingResults.Count(t => switchSet.IsSorted(t.Result));
        //    System.Diagnostics.Debug.WriteLine("switches used:{0} sucesses: {1}", switchUseCount, sucesses);
        //    Debug.WriteLine("Time(ms): {0}", stopwatch.ElapsedMilliseconds);
        //}

        //[TestMethod]
        //public void TestASorter3()
        //{
        //    const int keyCount = 16;
        //    const int keyPairCount = 800;
        //    const int switchableCount = 1000;

        //    var switchSet = new UintSwitchSet(keyCount);
        //    var switchables = Rando.Fast(1234).ToUintSwitchables(keyCount).Take(switchableCount).ToList();

        //    foreach (var sorter in Rando.Fast(1223).RandomSorters(KeyPairRepository.KeyPairsForKeyCount(keyCount).ToList(), keyPairCount, keyCount))
        //    {
        //        var results = SortingFunctions.TestWith
        //            (
        //                sorter: sorter,
        //                switchables: switchables
        //            );


        //        var switchUseCount = results.SwitchUseList.Count(t => t > 0);
        //        var sucesses = results.SortingResults.Count(t => switchSet.IsSorted(t.Result));
        //        System.Diagnostics.Debug.WriteLine("switches used:{0} sucesses: {1}", switchUseCount, sucesses);
                
        //    }

        //}

        //[TestMethod]
        //public void TestASorter4()
        //{
        //    const int keyCount = 16;
        //    const int keyPairCount = 700;
        //    int switchableCount = 1000;

        //    var switchSet = new UintSwitchSet(keyCount);
        //    //var switchables = Rando.Fast(1234).ToUintSwitchables(keyCount).Take(switchableCount).ToList();
        //    var switchables = Switchable.AllUintEnumerablesForKeyCount(16).ToList();
        //    switchableCount = switchables.Count();

        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();

        //    var results =
        //    Rando.Fast(1223).RandomSorters(KeyPairRepository.KeyPairsForKeyCount(keyCount).ToList(), keyPairCount, keyCount)
        //        .Select(s => s.TestWith(switchables))
        //        .Take(100)
        //        .ToList();

        //    stopwatch.Stop();

        //    var summary =    results
        //                    .Where(t => t.SortingResults.Count(q => switchSet.IsSorted(q.Result)) == switchableCount)
        //                    .Select(t => new Tuple<int>(t.SwitchUseList.Count(q => q > 0)))
        //                    .ToList();

        //    System.Diagnostics.Debug.WriteLine("{0}\t{1}\t{2}\t{3}", stopwatch.ElapsedMilliseconds, summary.Count(), keyPairCount, switchableCount);

        //    foreach (var tuple in summary.OrderBy(q => q.Item1))
        //    {
        //        System.Diagnostics.Debug.WriteLine("{0}", tuple.Item1);
        //    }

        //}


    }
}
