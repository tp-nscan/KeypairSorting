using MathUtils.Bits;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathUtils.Tests.Bits
{
    [TestClass]
    public class NumberConvertersFixture
    {
        [TestMethod]
        public void TestFlipBit()
        {
            const uint tV = 1 << 12;

            var res = tV.FilpBit(12);
            var res2 = res.FilpBit(12);
            Assert.IsTrue(res == 0);
            Assert.IsTrue(res2 == tV);
        }

        [TestMethod]
        public void TestHasBit()
        {
            const uint tV = 1 << 12;
            Assert.IsTrue(tV.BitValue(12));
            Assert.IsFalse(tV.BitValue(13));
        }

        [TestMethod]
        public void TestSwitchBits()
        {
            const uint tV = ((1 << 1) + 1) << 3;
        }

        [TestMethod]
        public void UshortConversions()
        {
            var randy = Rando.Fast(123);
            for (var i = 0; i < 100; i++)
            {
                var testIn = (ushort)randy.NextInt();
                var array = testIn.ToBits();
                var convertBack = array.ToUShort();

                Assert.AreEqual(testIn, convertBack);
                testIn *= 2;
            }
        }

        [TestMethod]
        public void UintConversions()
        {
            var randy = Rando.Fast(123);
            for (var i = 0; i < 100; i++)
            {
                var testIn = (uint)randy.NextInt();
                var array = testIn.ToBits();
                var convertBack = array.ToUInt();

                Assert.AreEqual(testIn, convertBack);
            }
        }


        [TestMethod]
        public void UlongConversions()
        {
            var randy = Rando.Fast(123);
            for (int i = 0; i < 100; i++)
            {
                var testIn = (ulong)randy.NextInt();
                testIn <<= 32;
                testIn += (ulong)randy.NextInt();

                var array = testIn.ToBits();
                var convertBack = array.ToULong();

                Assert.AreEqual(testIn, convertBack);
            }
        }
    }
}
