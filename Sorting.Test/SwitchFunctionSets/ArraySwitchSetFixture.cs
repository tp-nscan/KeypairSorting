using System.Linq;
using MathUtils.Bits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.KeyPairs;
using Sorting.SwitchFunctionSets;

namespace Sorting.Test.SwitchFunctionSets
{
    [TestClass]
    public class ArraySwitchSetFixture
    {
        [TestMethod]
        public void TestBitArraySwitchSetFunctionsAreNotNull()
        {
            var keyCount = 7;
            var bitArraySwitchSet = KeyPairSwitchSet.Make<bool[]>(keyCount);

            var keyPairs = KeyPairRepository.KeyPairsForKeyCount(keyCount).ToList();
            foreach (var keyPair in keyPairs)
            {
                Assert.IsNotNull(bitArraySwitchSet.SwitchFunction(keyPair));
            }

            keyCount = 16;
            bitArraySwitchSet = KeyPairSwitchSet.Make<bool[]>(keyCount);

            keyPairs = KeyPairRepository.KeyPairsForKeyCount(keyCount).ToList();
            foreach (var keyPair in keyPairs)
            {
                Assert.IsNotNull(bitArraySwitchSet.SwitchFunction(keyPair));
            }

            keyCount = 33;
            bitArraySwitchSet = KeyPairSwitchSet.Make<bool[]>(keyCount);

            keyPairs = KeyPairRepository.KeyPairsForKeyCount(keyCount).ToList();
            foreach (var keyPair in keyPairs)
            {
                Assert.IsNotNull(bitArraySwitchSet.SwitchFunction(keyPair));
            }
        }

        [TestMethod]
        public void TestBitArraySwitchSetFunctionsSwitchTheRightIndexes()
        {
            var keyCount = 64;
            var bitArraySwitchSet = KeyPairSwitchSet.Make<bool[]>(keyCount);

            var keyPairs = KeyPairRepository.KeyPairsForKeyCount(keyCount).ToList();
            foreach (var keyPair in keyPairs)
            {
                var preSwitch = BitArrayUtils.SetTrueBitInArray(keyPair.LowKey, keyCount);
                var postSwitch = BitArrayUtils.SetTrueBitInArray(keyPair.HiKey, keyCount);

                var switchResult = bitArraySwitchSet.SwitchFunction(keyPair)(preSwitch);
                Assert.IsTrue(switchResult.Item2);
                Assert.AreEqual(0, postSwitch.CompareDict(switchResult.Item1));

                Assert.IsNotNull(bitArraySwitchSet.SwitchFunction(keyPair));
            }

        }

        [TestMethod]
        public void TestIntArraySwitchSetFunctionsAreNotNull()
        {
            var keyCount = 7;
            var intArraySwitchSet = KeyPairSwitchSet.Make<int[]>(keyCount);

            var keyPairs = KeyPairRepository.KeyPairsForKeyCount(keyCount).ToList();
            foreach (var keyPair in keyPairs)
            {
                Assert.IsNotNull(intArraySwitchSet.SwitchFunction(keyPair));
            }

            keyCount = 16;
            intArraySwitchSet = KeyPairSwitchSet.Make<int[]>(keyCount);

            keyPairs = KeyPairRepository.KeyPairsForKeyCount(keyCount).ToList();
            foreach (var keyPair in keyPairs)
            {
                Assert.IsNotNull(intArraySwitchSet.SwitchFunction(keyPair));
            }

            keyCount = 33;
            intArraySwitchSet = KeyPairSwitchSet.Make<int[]>(keyCount);

            keyPairs = KeyPairRepository.KeyPairsForKeyCount(keyCount).ToList();
            foreach (var keyPair in keyPairs)
            {
                Assert.IsNotNull(intArraySwitchSet.SwitchFunction(keyPair));
            }
        }



        [TestMethod]
        public void TestIntArraySwitchSetFunctionsSwitchTheRightIndexes()
        {
            var keyCount = 7;
            var intArraySwitchSet = KeyPairSwitchSet.Make<int[]>(keyCount);

            var keyPairs = KeyPairRepository.KeyPairsForKeyCount(keyCount).ToList();
            foreach (var keyPair in keyPairs)
            {
                var preSwitch = new int[keyCount];
                preSwitch[keyPair.LowKey] = 33;
                var postSwitch = new int[keyCount];
                postSwitch[keyPair.HiKey] = 33;

                var switchResult = intArraySwitchSet.SwitchFunction(keyPair)(preSwitch);
                Assert.IsTrue(switchResult.Item2);
                Assert.AreEqual(0, postSwitch.CompareDict(switchResult.Item1));

                Assert.IsNotNull(intArraySwitchSet.SwitchFunction(keyPair));
            }
        }

        [TestMethod]
        public void TestIntArraySwitchSetFunctionIsSorted()
        {
            const int keyCount = 7;
            var intArraySwitchSet = KeyPairSwitchSet.Make<int[]>(keyCount);

            var sortedArray = Enumerable.Range(0, keyCount).ToArray();

            Assert.IsTrue(intArraySwitchSet.IsSorted(sortedArray));

            sortedArray = Enumerable.Range(1, keyCount).ToArray();

            Assert.IsFalse(intArraySwitchSet.IsSorted(sortedArray));
        }

    }
}
