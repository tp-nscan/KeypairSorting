using System;
using System.Linq;
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
        public void TestUpdateWithMerge()
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
                    seeds: new[] {123, 12345},
                    mergeWithPrev: true
                );

            updatedBuilder = SorterCompWorkflowBuilder.Update(
                    builder: updatedBuilder,
                    seeds: new[] { 23, 2345, 444, 777 },
                    mergeWithPrev: true
                );

            Assert.AreEqual(updatedBuilder.InputEntities.Count, 3);
            Assert.IsNotNull(updatedBuilder.Entity);
            Assert.AreEqual(updatedBuilder.Seeds.Count(), 6);
        }

        [TestMethod]
        public void TestUpdateWithoutMerge()
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
                    seeds: new[] { 123, 12345 },
                    mergeWithPrev: false
                );

            updatedBuilder = SorterCompWorkflowBuilder.Update(
                builder: updatedBuilder,
                seeds: new[] { 23, 2345 },
                mergeWithPrev: false
            );

            Assert.AreEqual(updatedBuilder.InputEntities.Count, 1);
            Assert.IsNotNull(updatedBuilder.Entity);
            Assert.AreEqual(updatedBuilder.Seeds.Count(), 2);
        }

    }
}
