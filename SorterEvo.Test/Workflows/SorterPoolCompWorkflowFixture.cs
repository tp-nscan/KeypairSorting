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
            var workflow = SorterPoolCompWorkflow.Make(
                tracker: null,
                sorterLayer: TestSorterEvo.SorterLayer(),
                switchableGroupLayer: TestSorterEvo.SwitchableGroupLayer(),
                sorterPoolCompParams: TestSorterEvo.SorterPoolCompParams()
                );
            Assert.IsNotNull(workflow);
        }

        [TestMethod]
        public void TestStep()
        {
            var workflow = SorterPoolCompWorkflow.Make(
                tracker: null,
                sorterLayer: TestSorterEvo.SorterLayer(),
                switchableGroupLayer: TestSorterEvo.SwitchableGroupLayer(),
                sorterPoolCompParams: TestSorterEvo.SorterPoolCompParams()
                );

            workflow.Step(TestSorterEvo.Seeds.First());
            Assert.AreEqual(workflow.SorterPoolCompState, SorterPoolCompState.RunCompetition);
        }


    }
}
