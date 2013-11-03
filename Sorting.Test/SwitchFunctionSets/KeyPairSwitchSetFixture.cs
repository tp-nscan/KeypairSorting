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
            Assert.AreEqual(impla.KeyCount, keyCount);

            var implb = KeyPairSwitchSet.Make<uint>(keyCount);
            Assert.AreEqual(implb.KeyCount, keyCount);

            var imple = KeyPairSwitchSet.Make<ulong>(keyCount);
            Assert.AreEqual(implb.KeyCount, keyCount);

            var implc = KeyPairSwitchSet.Make<bool[]>(keyCount);
            Assert.AreEqual(implc.KeyCount, keyCount);

            var impld = KeyPairSwitchSet.Make<int[]>(keyCount);
            Assert.AreEqual(impld.KeyCount, keyCount);

        }
    }
}
