using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.TestData;
using SorterEvo.Workflows;

namespace SorterEvo.Test.Workflows
{
    [TestClass]
    public class SorterCompPoolWorkflowFixture
    {
        [TestMethod]
        public void TestMake()
        {
            var workflow = SorterCompParaPoolWorkflow.Make(
                sorterLayer: SorterEvoTestData.SorterLayer(),
                switchableGroupLayer: SorterEvoTestData.SwitchableGroupLayer(),
                sorterCompParaPoolParams: SorterEvoTestData.SorterCompParaPoolParams(),
                generation:0
                );
            Assert.IsNotNull(workflow);
        }

        [TestMethod]
        public void TestSteps()
        {
            var workflow = SorterCompParaPoolWorkflow.Make(
                sorterLayer: SorterEvoTestData.SorterLayer(),
                switchableGroupLayer: SorterEvoTestData.SwitchableGroupLayer(),
                sorterCompParaPoolParams: SorterEvoTestData.SorterCompParaPoolParams(),
                generation:0
                );

            var seedList = SorterEvoTestData.Seeds.Take(5).ToList();
            var newWorkflow = workflow.Step(seedList[0]);
            Assert.AreEqual(newWorkflow.CompWorkflowState, CompWorkflowState.RunCompetition);
            newWorkflow = newWorkflow.Step(seedList[1]);
            Assert.AreEqual(newWorkflow.CompWorkflowState, CompWorkflowState.EvaluateResults);
        }

    }
}
