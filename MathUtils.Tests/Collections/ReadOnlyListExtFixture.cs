using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathUtils.Tests.Collections
{
    [TestClass]
    public class ReadOnlyListExtFixture
    {
        [TestMethod]
        public void IsTheSameAs()
        {
            var list1 = new[] {1, 2, 3, 3};
            var list2 = new[] {1, 2, 3};
            var list3 = new[] {3, 2, 3, 1};

            var res1 = list1.HasSameElementsRepeatsAllowed(list2);
            var res2 = list1.HasSameElementsRepeatsAllowed(list3);
            var res3 = list1.HasSameElementsRepeatsAllowed(list2);
        }

        [TestMethod]
        public void FisherYatesShuffleTest()
        {
            var stopwatch = new Stopwatch();
            const int arraySize = 1000;

            stopwatch.Start();
            var startingPool = Enumerable.Range(0, arraySize).ToList();

            var scrammy = 0;
            for (var i = 0; i < 1000; i++)
            {
                scrammy += startingPool.FisherYatesShuffle(Rando.Fast(i))[0];
            }

            stopwatch.Stop();
            Debug.WriteLine("Time(ms): {0}", stopwatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void TestInsert()
        {
            var stopwatch = new Stopwatch();
            var origList = Enumerable.Repeat(0, 1000).ToList();
            var rando = Rando.Fast(123);

            stopwatch.Start();


            for (var i = 0; i < 1000; i++)
            {
                var insertedList = origList.Insert(rando.ToBoolEnumerator(0.1), t => 1);
                var sum = insertedList.Sum(t => t);
                Assert.AreEqual(insertedList.Count, 1000);
                Assert.IsTrue(sum < 150);
                Assert.IsTrue(sum > 50);
            }

            stopwatch.Stop();

            Debug.WriteLine("Time(ms): {0}", stopwatch.ElapsedMilliseconds);

        }

        [TestMethod]
        public void TestDelete()
        {
            var stopwatch = new Stopwatch();
            var origList = Enumerable.Repeat(0, 1000).ToList();
            var rando = Rando.Fast(123);

            stopwatch.Start();


            for (var i = 0; i < 1000; i++)
            {
                var insertedList = origList.Delete(rando.ToBoolEnumerator(0.1), () => 1);
                var sum = insertedList.Sum(t => t);
                Assert.AreEqual(insertedList.Count, 1000);
                Assert.IsTrue(sum < 150);
                Assert.IsTrue(sum > 50);
            }

            stopwatch.Stop();

            Debug.WriteLine("Time(ms): {0}", stopwatch.ElapsedMilliseconds);

        }

        [TestMethod]
        public void TestMutateInsertDelete()
        {
            var stopwatch = new Stopwatch();
            var origList = Enumerable.Repeat(0, 1000).ToList();
            var randoMutate = Rando.Fast(123);
            var randoInsert = Rando.Fast(1234);
            var randoDelete = Rando.Fast(12345);

            stopwatch.Start();


            for (var i = 0; i < 1000; i++)
            {
                var insertedList = origList.MutateInsertDelete
                    (
                        doMutation:  randoMutate.ToBoolEnumerator(0.1),
                        doInsertion: randoInsert.ToBoolEnumerator(0.1),
                        doDeletion:  randoDelete.ToBoolEnumerator(0.1),
                        mutator:     x => 0,
                        inserter:    x => 1,
                        deleter:     () => 100000
                    );

                var sum = insertedList.Sum(t => t);
                Assert.AreEqual(insertedList.Count, 1000);
                //Assert.IsTrue(sum == 0);
                //Assert.IsTrue(sum < 150);
                //Assert.IsTrue(sum > 50);
            }

            stopwatch.Stop();

            Debug.WriteLine("Time(ms): {0}", stopwatch.ElapsedMilliseconds);

        }


        [TestMethod]
        public void TestMutateInsertDelete2()
        {
            var stopwatch = new Stopwatch();
            var origList = Enumerable.Repeat(0, 1000)
                                     .Select(i=> new Tuple<int, int, int>(0,0,0))
                                     .ToList();

            var randoMutate = Rando.Fast(123);
            var randoInsert = Rando.Fast(1234);
            var randoDelete = Rando.Fast(12345);

            stopwatch.Start();


            for (var i = 0; i < 1000; i++)
            {
                var insertedList = origList.MutateInsertDelete
                    (
                        doMutation: randoMutate.ToBoolEnumerator(0.1),
                        doInsertion: randoInsert.ToBoolEnumerator(0.1),
                        doDeletion: randoDelete.ToBoolEnumerator(0.1),
                        mutator: x => new Tuple<int, int, int>(1, x.Item2, x.Item3),
                        inserter: x => new Tuple<int, int, int>(x.Item1, 1, x.Item3), 
                        deleter: () => new Tuple<int, int, int>(0, 0, 1)
                    );

                //for (var j = 0; j < 1000; j++)
                //{
                //    var tupy = insertedList[j];
                //    System.Diagnostics.Debug.WriteLine("{0}{1}{2}", tupy.Item1, tupy.Item2, tupy.Item3);
                //}

                //System.Diagnostics.Debug.WriteLine("***************");
                //System.Diagnostics.Debug.WriteLine("***************");
                //System.Diagnostics.Debug.WriteLine("***************");
                //System.Diagnostics.Debug.WriteLine("***************");


                Assert.AreEqual(insertedList.Count, 1000);

                var mutationSum = insertedList.Sum(t => t.Item1);
                Assert.IsTrue(mutationSum < 150);
                Assert.IsTrue(mutationSum > 50);

                var insertionSum = insertedList.Sum(t => t.Item2);
                Assert.IsTrue(insertionSum < 150);
                Assert.IsTrue(insertionSum > 50);

                var deletionSum = insertedList.Sum(t => t.Item3);
                Assert.IsTrue(deletionSum < 50);
            }

            stopwatch.Stop();

            Debug.WriteLine("Time(ms): {0}", stopwatch.ElapsedMilliseconds);

        }

        [TestMethod]
        public void Recombine()
        {
            //var aList = new List<string> {"a0", "a1", "a2", "a3", "a4", "a5", "a6"};
            //var a2List = new List<string> {"a0", "a1", "a2", "a3", "a4", "a5", "a6"};
            //var bList = new List<string> { "b0", "b1", "b2", "b3", "b4", "b5", "b6" };
            //var swaps1 = new List<bool> { false, false, true, false, false, false, false};
            //var swaps2 = new List<bool> { true, false, false, false, false, false, false };

            //var aRes = new List<string> { "a0", "a1", "b2", "b3", "b4", "b5", "b6" };
            //var bRes = new List<string> { "b0", "b1", "a2", "a3", "a4", "a5", "a6" };

            //List<string> aOut;
            //List<string> bOut;

            //List.Recombine(aList, bList, swaps1, out aOut, out bOut);

            //Assert.IsTrue(aOut.SequenceEqual(aRes));
            //Assert.IsTrue(bOut.SequenceEqual(bRes));


            //List.Recombine(aList, bList, swaps2, out aOut, out bOut);
            //Assert.IsTrue(aOut.SequenceEqual(bList));
            //Assert.IsTrue(bOut.SequenceEqual(aList));
        }


    }
}
