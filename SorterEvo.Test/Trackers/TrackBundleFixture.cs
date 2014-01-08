//using System;
//using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using SorterEvo.Trackers;

//namespace SorterEvo.Test.Trackers
//{
//    [TestClass]
//    public class TrackBundleFixture
//    {
//        [TestMethod]
//        public void TestTrackBundle()
//        {
//            const int partitionCount = 3;
//            const int repCount = 2;

//            var trackBundle = TrackBundle.Make("UnitTest", partitionCount, repCount, 1234)
//                               .ToList();

//            Assert.AreEqual(trackBundle.Count, partitionCount*repCount);

//        }

//        [TestMethod]
//        public void TestTrackBundleUse()
//        {
//            const int partitionCount = 3;
//            const int repCount = 2;

//            var trackBundle = TrackBundle.Make("UnitTest", partitionCount, repCount, 1234)
//                               .ToList();

//            Assert.AreEqual(trackBundle.Count, partitionCount * repCount);

//        }
//    }
//}
