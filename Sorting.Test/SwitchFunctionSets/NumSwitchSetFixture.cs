using System.Diagnostics;
using System.Linq;
using MathUtils.Bits;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.SwitchFunctionSets;

namespace Sorting.Test.SwitchFunctionSets
{
    [TestClass]
    public class NumSwitchSetFixture
    {
        [TestMethod]
        public void TestUShortIsOrdered()
        {
            const int maxKeyCount = 16;
            for (var keyCount = 2; keyCount < maxKeyCount; keyCount++)
            {
                var ushortSwitchSet = KeyPairSwitchSet.Make<ushort>(keyCount);
                for (var i = 0; i < keyCount; i++)
                {
                    var bitArray = Enumerable.Repeat(false, i)
                                        .Concat(Enumerable.Repeat(true, keyCount - i))
                                            .ToArray();
                    var testNum = bitArray.ToUShort();
                    Assert.IsTrue(ushortSwitchSet.IsSorted(testNum));
                }
            }
        }


        [TestMethod]
        public void TestUIntIsOrdered()
        {
            const int maxKeyCount = 32;
            for (var keyCount = 2; keyCount < maxKeyCount; keyCount++)
            {
                var ushortSwitchSet = KeyPairSwitchSet.Make<uint>(keyCount);
                for (var i = 0; i < keyCount; i++)
                {
                    var bitArray = Enumerable.Repeat(false, i)
                                        .Concat(Enumerable.Repeat(true, keyCount - i))
                                            .ToArray();
                    var testNum = bitArray.ToUInt();
                    Assert.IsTrue(ushortSwitchSet.IsSorted(testNum));
                }
            }
        }


        [TestMethod]
        public void TestULongIsOrdered()
        {
            const int maxKeyCount = 64;
            for (var keyCount = 2; keyCount < maxKeyCount; keyCount++)
            {
                var ushortSwitchSet = KeyPairSwitchSet.Make<ulong>(keyCount);
                for (var i = 0; i < keyCount; i++)
                {
                    var bitArray = Enumerable.Repeat(false, i)
                                        .Concat(Enumerable.Repeat(true, keyCount - i))
                                            .ToArray();
                    var testNum = bitArray.ToULong();
                    Assert.IsTrue(ushortSwitchSet.IsSorted(testNum));
                }
            }
        }

        [TestMethod]
        public void TestRandomUshortForSorted()
        {
            const int maxKeyCount = 16;
            var randy = Rando.Fast(322);

            for (var keyCount = 2; keyCount < maxKeyCount + 1; keyCount++)
            {
                var switchSet = KeyPairSwitchSet.Make<ushort>(keyCount);
                for (var i = 0; i < 1000; i++)
                {
                    var testNum = (ushort)randy.NextInt();
                    testNum >>= (maxKeyCount - keyCount);
                    var testArray = testNum.ToBits().Take(keyCount).ToArray();
                    Assert.AreEqual(switchSet.IsSorted(testNum), testArray.IsSorted());
                }
            }
        }


        [TestMethod]
        public void TestRandomUintForSorted()
        {
            const int maxKeyCount = 32;
            var randy = Rando.Fast(322);

            for (var keyCount = 2; keyCount < maxKeyCount + 1; keyCount++)
            {
                var switchSet = KeyPairSwitchSet.Make<uint>(keyCount);
                for (var i = 0; i < 100; i++)
                {
                    var testNum = (uint)randy.NextInt();
                    testNum >>= (maxKeyCount - keyCount);
                    var testArray = testNum.ToBits().Take(keyCount).ToArray();
                    Assert.AreEqual(switchSet.IsSorted(testNum), testArray.IsSorted());
                }
            }
        }


        [TestMethod]
        public void TestRandomUlongForSorted()
        {
            const int maxKeyCount = 64;
            var randy = Rando.Fast(322);

            for (var keyCount = 2; keyCount < maxKeyCount + 1; keyCount++)
            {
                var switchSet = new UlongSwitchSet(keyCount);
                for (var i = 0; i < 1000; i++)
                {
                    var testNum = (ulong)randy.NextInt();
                    testNum <<= 32;
                    testNum += (ulong)randy.NextInt();

                    testNum >>= (maxKeyCount - keyCount);
                    var testArray = testNum.ToBits().Take(keyCount).ToArray();
                    Assert.AreEqual(switchSet.IsSorted(testNum), testArray.IsSorted());
                }
            }
        }

        [TestMethod]
        public void BenchUshortForSorted()
        {
            const int maxKeyCount = 16;
            var stopwatch = new Stopwatch();
            const int keyCount = 16;
            var switchSet = KeyPairSwitchSet.Make<ushort>(keyCount);

            var testResults = 0;

            stopwatch.Start();

            for (var j = 0; j < 1000; j++)
            {
                for (ushort i = 0; i < ushort.MaxValue; i++)
                {
                    var testNum = i;
                   // testNum >>= (maxKeyCount - keyCount);
                    testResults += switchSet.IsSorted(testNum) ? 1 : 0;
                }
            }

            stopwatch.Stop();

            Debug.WriteLine("Time(ms): {0}", stopwatch.ElapsedMilliseconds);
        }


        [TestMethod]
        public void BenchUintForSorted()
        {
            var stopwatch = new Stopwatch();
            const int keyCount = 16;
            var switchSet = KeyPairSwitchSet.Make<uint>(keyCount);

            var testResults = 0;

            stopwatch.Start();

            for (var j = 0; j < 1000; j++)
            {
                for (ushort i = 0; i < ushort.MaxValue; i++)
                {
                    var testNum = i;
                    //testNum >>= (maxKeyCount - keyCount);
                    testResults += switchSet.IsSorted(testNum) ? 1 : 0;
                }
            }

            stopwatch.Stop();

            Debug.WriteLine("Time(ms): {0}", stopwatch.ElapsedMilliseconds);
        }


        [TestMethod]
        public void TestCtor()
        {

            const int keyCount = 16;
            var switchSet = KeyPairSwitchSet.Make<ushort>(keyCount);

            var t = typeof(int[]);
            
            var dater = new[] {1, 2, 3};

        }
    }
}
