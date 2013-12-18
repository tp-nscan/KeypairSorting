using System;
using System.Linq;
using MathUtils;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.TestData;
using SorterEvo.Trackers;
using SorterEvo.Workflows;
using Sorting.CompetePool;

namespace SorterEvo.Test.Trackers
{
    [TestClass]
    public class SorterPoolCompWorkflowTrackerFixture
    {
        [TestMethod]
        public void TestMake()
        {
            Assert.IsNotNull(SorterCompTracker.Make());
        }

        [TestMethod]
        public void TestUpdateWithMergeAndTrack()
        {
            var tracker = SorterCompTracker.Make();
            var builder = SorterCompWorkflowBuilder.Make
                (
                    workFlowGuid: Guid.NewGuid(),
                    repository: TestRepository.EntityRepository,
                    switchableGroupGuid: TestRepository.SwitchableGroupLayerGuid,
                    sorterGroupGuid: TestRepository.SorterLayerGuid,
                    sorterPoolCompParamsGuid: TestRepository.SorterPoolCompParamsGuid
                );


            var updatedBuilder = SorterCompWorkflowBuilder.UpdateAndTrack(
                    builder: builder,
                    seeds: new[] {123, 12345},
                    mergeWithPrev: true,
                    tracker: tracker
                );



            updatedBuilder = SorterCompWorkflowBuilder.UpdateAndTrack(
                    builder: updatedBuilder,
                    seeds: new[] { 23, 2345, 444, 777, 234, 345, 346, 347, 348, 349, 350, 351, 352, 353, 354, 355, 356, 357, 358, 259, 360, 361, 362 },
                    mergeWithPrev: true,
                    tracker: tracker
                );

            Assert.AreEqual(updatedBuilder.InputEntities.Count, 3);
            Assert.IsNotNull(updatedBuilder.Entity);
            Assert.AreEqual(updatedBuilder.Seeds.Count(), 25);
        }

        [TestMethod]
        public void TestUpdateWithMergeAndTrack2()
        {
            ISorterPoolCompWorkflowTracker tracker = SorterCompTracker.Make();

            var builder = SorterCompWorkflowBuilder.Make
                (
                    workFlowGuid: Guid.NewGuid(),
                    repository: TestRepository.EntityRepository,
                    switchableGroupGuid: TestRepository.SwitchableGroupLayerGuid,
                    sorterGroupGuid: TestRepository.SorterLayerGuid,
                    sorterPoolCompParamsGuid: TestRepository.SorterPoolCompParamsGuid
                );


            ISorterCompWorkflowBuilder updatedBuilder = builder;

            for (var i = 0; i < 1000; i++)
            {
                var seeds = Rando.Fast(i*17).ToIntEnumerator().Take(5).ToList();

                updatedBuilder = SorterCompWorkflowBuilder.UpdateAndTrack(
                        builder: updatedBuilder,
                        seeds: seeds,
                        mergeWithPrev: true,
                        tracker: tracker
                    );

                var bsetResult =
                    tracker.SorterPoolStats
                           .GenomeStatses
                           .OrderBy(t => t.ReferenceResult.Cast<ISorterOnSwitchableGroup>().SwitchesUsed)
                           .First().ReferenceResult
                           .Cast<ISorterOnSwitchableGroup>();

                var averageScore = tracker.SorterPoolStats
                                          .GenomeStatses.Average(t => t.ReferenceResult.Cast<ISorterOnSwitchableGroup>().SwitchesUsed);

                System.Diagnostics.Debug.WriteLine("{0}\t{1}\t{2}\t{3}", bsetResult.SwitchesUsed, bsetResult.Success, averageScore, i);
                System.Diagnostics.Debug.WriteLine(tracker.PoolReport);

                tracker = tracker.Trim(1000);
            }


            Assert.AreEqual(updatedBuilder.InputEntities.Count, 3);
            Assert.IsNotNull(updatedBuilder.Entity);
            Assert.AreEqual(updatedBuilder.Seeds.Count(), 25);
        }
    }
}

