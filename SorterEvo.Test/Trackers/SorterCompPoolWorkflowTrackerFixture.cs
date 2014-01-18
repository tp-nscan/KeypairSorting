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
    public class ScpWorkflowTrackerFixture
    {
        [TestMethod]
        public void TestUpdateWithMergeAndTrack()
        {
            var tracker = ScpWorkflowTracker.Make();

            var builder = ScpWorkflowBuilder.Make
                (
                    workFlowGuid: Guid.NewGuid(),
                    repository: TestRepository.EntityRepository,
                    sorterGroupGuid: TestRepository.SorterLayerGuid,
                    scpParamsGuid: TestRepository.ScpParamsGuid
                );

            var updatedBuilder = builder;

            for (var i = 0; i < 5000; i++)
            {
                var seeds = Rando.Fast(i * 17).ToIntEnumerator().Take(5).ToList();

                updatedBuilder = ScpWorkflowBuilder.UpdateAndTrack(
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
