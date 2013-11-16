using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathUtils.Tests.Collections
{
    [TestClass]
    public class EnumerableExtFixture
    {
        [TestMethod]
        public void TestSwap()
        {
            //var a1 = new MyClass() {Value = 1};
            //var a2 = new MyClass() {Value = 2};
            //Set.Swap(ref a1, ref a2);
            //Assert.AreEqual(a1.Value, 2);
            //Assert.AreEqual(a2.Value, 1);
        }

        [TestMethod]
        public void HasSameElementsAsTest()
        {
            //const int arraySize = 100;
            //var testPool = Enumerable.Range(0, arraySize).ToList();
            //var randomized = testPool.PickMembers(Rando.Fast(22).(arraySize)).ToList();
            //Assert.IsTrue(testPool.HasSameElementsAs(randomized));
        }


        [TestMethod]
        public void ChunkTest()
        {
            var aList = new List<string> { "a0", "a1", "a2", "a3", "a4", "a5", "a6", "a7" };

            var chunks = aList.Chunk(2).ToList();

            Assert.AreEqual(chunks.Count, 4);
            Assert.AreEqual(chunks[0].First(), "a0");
            Assert.AreEqual(chunks[0].Last(), "a1");
            Assert.AreEqual(chunks[3].First(), "a6");
            Assert.AreEqual(chunks[3].Last(), "a7");
        }

        [TestMethod]
        public void ChunkLimitTest()
        {
            var aList = new List<string> { "a0", "a1", "a2", "a3", "a4", "a5", "a6", "a7" };

            var chunks = aList.Chunk(3).Take(2).ToList();

            Assert.AreEqual(chunks.Count, 2);
            Assert.AreEqual(chunks[0].First(), "a0");
            Assert.AreEqual(chunks[0].Last(), "a2");
            Assert.AreEqual(chunks[1].First(), "a3");
            Assert.AreEqual(chunks[1].Last(), "a5");
        }

        [TestMethod]
        public void UintUlongEnumerTest()
        {
            var startUints = Rando.Fast(123).ToUintEnumerator()
                .Take(100).ToList();

            var convUlongs = startUints.ToUlongs().ToList();
            var endUints = convUlongs.ToUints().ToList();

            var diffs = startUints.GetDiffs(endUints).ToList();
            Assert.AreEqual(diffs.Count, 0);
        }

        [TestMethod]
        public void UlongUintEnumerTest()
        {
            var startUlongs = Rando.Fast(123).ToUlongEnumerator()
                .Take(100).ToList();

            var convUints = startUlongs.ToUints().ToList();
            var endUlongs = convUints.ToUlongs().ToList();

            var diffs = startUlongs.GetDiffs(endUlongs).ToList();
            Assert.AreEqual(diffs.Count, 0);
        }

    }
}

