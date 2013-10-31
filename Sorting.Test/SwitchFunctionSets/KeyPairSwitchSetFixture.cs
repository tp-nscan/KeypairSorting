using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.SwitchFunctionSets;

namespace Sorting.Test.SwitchFunctionSets
{
    [TestClass]
    public class KeyPairSwitchSetFixture
    {
        [TestMethod]
        public void TestImplementationAssignment()
        {
            const int keyCount = 16;

            var impla = KeyPairSwitchSet.Make<ushort>(keyCount);
            Assert.IsTrue(impla is UshortSwitchSet);
            Assert.AreEqual(impla.KeyCount, keyCount);

            var implb = KeyPairSwitchSet.Make<uint>(keyCount);
            Assert.IsTrue(implb is UintSwitchSet);
            Assert.AreEqual(implb.KeyCount, keyCount);

            var imple = KeyPairSwitchSet.Make<ulong>(keyCount);
            Assert.IsTrue(imple is UlongSwitchSet);
            Assert.AreEqual(implb.KeyCount, keyCount);

            var implc = KeyPairSwitchSet.Make<bool[]>(keyCount);
            Assert.IsTrue(implc is BitArraySwitchSet);
            Assert.AreEqual(implc.KeyCount, keyCount);

            var impld = KeyPairSwitchSet.Make<int[]>(keyCount);
            Assert.IsTrue(impld is IntArraySwitchSet);
            Assert.AreEqual(impld.KeyCount, keyCount);


        }
    }
}
