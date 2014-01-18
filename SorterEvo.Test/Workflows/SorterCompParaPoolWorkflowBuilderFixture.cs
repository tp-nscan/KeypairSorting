using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.TestData;
using SorterEvo.Workflows;

namespace SorterEvo.Test.Workflows
{
    [TestClass]
    public class SorterCompParaPoolWorkflowBuilderFixture
    {
        [TestMethod]
        public void TestMake()
        {
            var builder = SorterCompParaPoolWorkflowBuilder.Make
                (
                    workFlowGuid: Guid.NewGuid(),
                    repository: TestRepository.EntityRepository,
                    switchableGroupGuid: TestRepository.SwitchableGroupLayerGuid,
                    sorterGroupGuid: TestRepository.SorterLayerGuid,
                    scpParamsGuid: TestRepository.SorterCompParaPoolParamsGuid
                );

            Assert.AreEqual(builder.InputEntities.Count, 3);
            Assert.IsNotNull(builder.Entity);
        }

        [TestMethod]
        public void TestUpdateWithMerge()
        {
            var builder = SorterCompParaPoolWorkflowBuilder.Make
                (
                    workFlowGuid: Guid.NewGuid(),
                    repository: TestRepository.EntityRepository,
                    switchableGroupGuid: TestRepository.SwitchableGroupLayerGuid,
                    sorterGroupGuid: TestRepository.SorterLayerGuid,
                    scpParamsGuid: TestRepository.SorterCompParaPoolParamsGuid
                );


            var updatedBuilder = SorterCompParaPoolWorkflowBuilder.Update(
                    builder: builder,
                    seeds: new[] {123, 12345},
                    mergeWithPrev: true
                );

            updatedBuilder = SorterCompParaPoolWorkflowBuilder.Update(
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
            var builder = SorterCompParaPoolWorkflowBuilder.Make
                (
                    workFlowGuid: Guid.NewGuid(),
                    repository: TestRepository.EntityRepository,
                    switchableGroupGuid: TestRepository.SwitchableGroupLayerGuid,
                    sorterGroupGuid: TestRepository.SorterLayerGuid,
                    scpParamsGuid: TestRepository.SorterCompParaPoolParamsGuid
                );


            var updatedBuilder = SorterCompParaPoolWorkflowBuilder.Update(
                    builder: builder,
                    seeds: new[] { 123, 12345 },
                    mergeWithPrev: false
                );

            updatedBuilder = SorterCompParaPoolWorkflowBuilder.Update(
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
