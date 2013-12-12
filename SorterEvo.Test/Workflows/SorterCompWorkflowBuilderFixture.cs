using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.TestData;
using SorterEvo.Workflows;

namespace SorterEvo.Test.Workflows
{
    [TestClass]
    public class SorterCompWorkflowBuilderFixture
    {
        [TestMethod]
        public void TestMake()
        {
            var builder = SorterCompWorkflowBuilder.Make
                (
                    workFlowGuid: Guid.NewGuid(),
                    repository: TestRepository.EntityRepository,
                    switchableGroupGuid: TestRepository.SwitchableGroupLayerGuid,
                    sorterGroupGuid: TestRepository.SorterLayerGuid,
                    sorterPoolCompParamsGuid: TestRepository.SorterPoolCompParamsGuid
                );

            Assert.AreEqual(builder.InputEntities.Count, 3);
            Assert.IsNotNull(builder.Entity);
        }

        [TestMethod]
        public void TestUpdate()
        {
            var builder = SorterCompWorkflowBuilder.Make
                (
                    workFlowGuid: Guid.NewGuid(),
                    repository: TestRepository.EntityRepository,
                    switchableGroupGuid: TestRepository.SwitchableGroupLayerGuid,
                    sorterGroupGuid: TestRepository.SorterLayerGuid,
                    sorterPoolCompParamsGuid: TestRepository.SorterPoolCompParamsGuid
                );


            var updatedBuilder = SorterCompWorkflowBuilder.Update(
                builder: builder,
                seeds: new[] {123, 12345}
                );

            Assert.AreEqual(updatedBuilder.InputEntities.Count, 3);
            Assert.IsNotNull(updatedBuilder.Entity);
        }
    }
}
