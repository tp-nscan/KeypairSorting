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
                sorterLayer: SorterEvoTestData.SorterLayer(),
                switchableGroupLayer: SorterEvoTestData.SwitchableGroupLayer(),
                sorterCompPoolParams: SorterEvoTestData.SorterPoolCompParams(),
                generation:0
                );
            Assert.IsNotNull(workflow);
        }

        [TestMethod]
        public void TestSteps()
        {
            var workflow = SorterCompWorkflow.Make(
                sorterLayer: SorterEvoTestData.SorterLayer(),
                switchableGroupLayer: SorterEvoTestData.SwitchableGroupLayer(),
                sorterCompPoolParams: SorterEvoTestData.SorterPoolCompParams(),
                generation:0
                );

            var seedList = SorterEvoTestData.Seeds.Take(5).ToList();
            var newWorkflow = workflow.Step(seedList[0]);
            Assert.AreEqual(newWorkflow.SorterPoolCompState, SorterCompState.RunCompetition);
            newWorkflow = newWorkflow.Step(seedList[1]);
            Assert.AreEqual(newWorkflow.SorterPoolCompState, SorterCompState.EvaluateResults);
        }

    }
}
