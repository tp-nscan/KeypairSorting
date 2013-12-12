using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.Trackers;

namespace SorterEvo.Test.Workflows
{
    [TestClass]
    public class SorterPoolCompWorkflowTrackerFixture
    {
        [TestMethod]
        public void TestMake()
        {
            Assert.IsNotNull(SorterCompTracker.Make());
        }
    }
}
