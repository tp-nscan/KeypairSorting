﻿using System;
using System.Diagnostics;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.Sorters;

namespace Sorting.Test.Sorters
{
    [TestClass]
    public class SorterFixture
    {
        [TestMethod]
        public void TestSorterHash()
        {
            var stopwatch = new Stopwatch();
            const int keyCount = 16;
            const int keyPairCount = 600;
            const int sorterCount = 6000;
            const int switchableCount = 100;
            const int switchableGroupCount = 40;

            var sorters = Rando.Fast(1243).ToRandomEnumerator()
                .Select(t => t.ToSorter(keyCount, keyPairCount, Guid.NewGuid()))
                .Take(sorterCount)
                .ToList();

            stopwatch.Start();

            var hashes = sorters.Select(t => t.KeyPairs.ToHash(k => k.Index)).ToList();

            stopwatch.Stop();


            Debug.WriteLine("Time(ms): {0}", stopwatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void TestSorterSpeed()
        {
            var stopwatch = new Stopwatch();
            const int keyCount = 16;
            const int keyPairCount = 6400;
            const int sorterCount = 6000;
            const int switchableCount = 100;
            const int switchableGroupCount = 40;


            var rando = Rando.Fast(1243);

            //var sorter = Rando.Fast(1243).ToRandomEnumerator()
            //    .Select(t => t.ToSorter(keyCount, keyPairCount, Guid.NewGuid()))
            //    .First();

            stopwatch.Start();

            for (var i = 0; i < 10; i++)
            {
                var sorter = rando.Spawn().ToSorter2(keyCount, keyPairCount, Guid.NewGuid());
            }

            stopwatch.Stop();


            Debug.WriteLine("Time(ms): {0}", stopwatch.ElapsedMilliseconds);
        }




    }
}
