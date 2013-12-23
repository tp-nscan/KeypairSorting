using System;
using System.Linq;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.TestData;
using SorterEvo.Trackers;
using SorterEvo.Workflows;

namespace SorterEvo.Test.Trackers
{
    [TestClass]
    public class SorterCompPoolWorkflowTrackerFixture
    {
        [TestMethod]
        public void TestUpdateWithMergeAndTrack()
        {
            var tracker = SorterCompPoolWorkflowTracker.Make();

            var builder = SorterCompPoolWorkflowBuilder.Make
                (
                    workFlowGuid: Guid.NewGuid(),
                    repository: TestRepository.EntityRepository,
                    sorterGroupGuid: TestRepository.SorterLayerGuid,
                    sorterCompPoolParamsGuid: TestRepository.SorterCompPoolParamsGuid
                );

            var updatedBuilder = builder;

            for (var i = 0; i < 5000; i++)
            {
                var seeds = Rando.Fast(i * 17).ToIntEnumerator().Take(5).ToList();

                updatedBuilder = SorterCompPoolWorkflowBuilder.UpdateAndTrack(
                        builder: updatedBuilder,
                        seeds: seeds,
                        mergeWithPrev: true,
                        tracker: tracker
                    );

                System.Diagnostics.Debug.WriteLine(tracker.PoolReport);

                tracker = tracker.Trim(1000);
            }


            Assert.AreEqual(updatedBuilder.InputEntities.Count, 3);
            Assert.IsNotNull(updatedBuilder.Entity);
            Assert.AreEqual(updatedBuilder.Seeds.Count(), 25);
        }
    }
}
