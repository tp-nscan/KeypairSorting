using System;
using System.Linq;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.TestData;
using SorterEvo.Workflows;

namespace SorterEvo.Test.Workflows
{
    [TestClass]
    public class ScpWorkflowBuilderFixture
    {
        [TestMethod]
        public void TestMake()
        {
            var builder = ScpWorkflowBuilder.Make
                (
                    workFlowGuid: Guid.NewGuid(),
                    repository: TestRepository.EntityRepository,
                    sorterGroupGuid: TestRepository.SorterLayerGuid,
                    scpParamsGuid: TestRepository.ScpParamsGuid
                );

            Assert.AreEqual(builder.InputEntities.Count, 2);
            Assert.IsNotNull(builder.Entity);
        }

        [TestMethod]
        public void TestUpdateWithMerge()
        {
            var builder = ScpWorkflowBuilder.Make
                (
                    workFlowGuid: Guid.NewGuid(),
                    repository: TestRepository.EntityRepository,
                    sorterGroupGuid: TestRepository.SorterLayerGuid,
                    scpParamsGuid: TestRepository.ScpParamsGuid
                );


            var updatedBuilder = ScpWorkflowBuilder.Update(
                    builder: builder,
                    seeds: new[] { 123, 12345 },
                    mergeWithPrev: true
                );

            updatedBuilder = ScpWorkflowBuilder.Update(
                    builder: updatedBuilder,
                    seeds: new[] { 23, 2345, 444, 777 },
                    mergeWithPrev: true
                );

            Assert.AreEqual(updatedBuilder.InputEntities.Count, 2);
            Assert.IsNotNull(updatedBuilder.Entity);
            Assert.AreEqual(updatedBuilder.Seeds.Count(), 6);
        }

    }
}
