using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.TestData;
using SorterEvo.Workflows;

namespace SorterEvo.Test.Workflows
{
    [TestClass]
    public class SorterPoolCompWorkflowFixture
    {
        [TestMethod]
        public void TestMake()
        {
            var workflow = SorterCompWorkflow.Make(
                //tracker: null,
                sorterLayer: TestSorterEvo.SorterLayer(),
                switchableGroupLayer: TestSorterEvo.SwitchableGroupLayer(),
                sorterCompPoolParams: TestSorterEvo.SorterPoolCompParams()
                );
            Assert.IsNotNull(workflow);
        }

        [TestMethod]
        public void TestSteps()
        {
            var workflow = SorterCompWorkflow.Make(
                //tracker: SorterCompTracker.Make(),
                sorterLayer: TestSorterEvo.SorterLayer(),
                switchableGroupLayer: TestSorterEvo.SwitchableGroupLayer(),
                sorterCompPoolParams: TestSorterEvo.SorterPoolCompParams()
                );

            var seedList = TestSorterEvo.Seeds.Take(5).ToList();
            var newWorkflow = workflow.Step(seedList[0]);
            Assert.AreEqual(newWorkflow.SorterPoolCompState, SorterCompState.RunCompetition);
            newWorkflow = newWorkflow.Step(seedList[1]);
            Assert.AreEqual(newWorkflow.SorterPoolCompState, SorterCompState.EvaluateResults);
        }

    }
}
