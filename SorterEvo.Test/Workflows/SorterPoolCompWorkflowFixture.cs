using System;
using System.Linq;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.Layers;
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
                sorterLayer: SorterEvoTestData.SorterLayer(),
                switchableGroupLayer: SorterEvoTestData.SwitchableGroupLayer(),
                sorterCompPoolParams: SorterEvoTestData.SorterPoolCompParams()
                );
            Assert.IsNotNull(workflow);
        }

        [TestMethod]
        public void TestSteps()
        {
            var workflow = SorterCompWorkflow.Make(
                //tracker: SorterCompTracker.Make(),
                sorterLayer: SorterEvoTestData.SorterLayer(),
                switchableGroupLayer: SorterEvoTestData.SwitchableGroupLayer(),
                sorterCompPoolParams: SorterEvoTestData.SorterPoolCompParams()
                );

            var seedList = SorterEvoTestData.Seeds.Take(5).ToList();
            var newWorkflow = workflow.Step(seedList[0]);
            Assert.AreEqual(newWorkflow.SorterPoolCompState, SorterCompState.RunCompetition);
            newWorkflow = newWorkflow.Step(seedList[1]);
            Assert.AreEqual(newWorkflow.SorterPoolCompState, SorterCompState.EvaluateResults);
        }

    }
}
