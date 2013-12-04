using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.TestData;
using SorterEvo.Trackers;
using SorterEvo.Workflows;

namespace SorterEvo.Test.Workflows
{
    [TestClass]
    public class SorterPoolCompWorkflowFixture
    {
        [TestMethod]
        public void TestMake()
        {
            var workflow = SorterPoolCompWorkflow.Make(
                tracker: null,
                sorterLayer: TestSorterEvo.SorterLayer(),
                switchableGroupLayer: TestSorterEvo.SwitchableGroupLayer(),
                sorterPoolCompParams: TestSorterEvo.SorterPoolCompParams()
                );
            Assert.IsNotNull(workflow);
        }

        [TestMethod]
        public void TestSteps()
        {
            var workflow = SorterPoolCompWorkflow.Make(
                tracker: SorterPoolCompWorkflowTracker.Make(),
                sorterLayer: TestSorterEvo.SorterLayer(),
                switchableGroupLayer: TestSorterEvo.SwitchableGroupLayer(),
                sorterPoolCompParams: TestSorterEvo.SorterPoolCompParams()
                );

            var seedList = TestSorterEvo.Seeds.Take(5).ToList();
            var newWorkflow = workflow.Step(seedList[0]);
            Assert.AreEqual(newWorkflow.SorterPoolCompState, SorterPoolCompState.RunCompetition);
            newWorkflow = newWorkflow.Step(seedList[1]);
            Assert.AreEqual(newWorkflow.SorterPoolCompState, SorterPoolCompState.EvaluateResults);
        }



    }
}
