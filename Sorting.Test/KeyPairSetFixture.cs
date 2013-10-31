using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.KeyPairs;

namespace Sorting.Test
{
    [TestClass]
    public class KeyPairSetFixture
    {
        [TestMethod]
        public void TestKeyPairsCount()
        {
            Assert.AreEqual
           (
                KeyPairRepository.KeyPairsForKeyCount(KeyPairRepository.MaxKeyCount).Count(),
                KeyPairRepository.KeyPairSetSizeForKeyCount(KeyPairRepository.MaxKeyCount)
           );
        }

        [TestMethod]
        public void TestKeyPairAssignment()
        {
            foreach (var keyPair in KeyPairRepository.KeyPairsForKeyCount(KeyPairRepository.MaxKeyCount))
            {
                Assert.AreEqual
                    (
                        keyPair.Index,
                        KeyPairRepository.KeyPairIndex(keyPair.LowKey, keyPair.HiKey)
                    );
            }
        }
    }
}
