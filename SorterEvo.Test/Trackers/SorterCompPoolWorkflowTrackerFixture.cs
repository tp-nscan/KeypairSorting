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
        public void TestUpdateWithMergeAndTrack2()
        {
            var tracker = SorterCompPoolWorkflowTracker.Make();

            var builder = SorterCompPoolWorkflowBuilder.Make
                (
                    workFlowGuid: Guid.NewGuid(),
                    repository: TestRepository.EntityRepository,
                    sorterGroupGuid: TestRepository.SorterLayerGuid,
                    sorterCompPoolParamsGuid: TestRepository.SorterCompPoolParamsGuid
                );



            ISorterCompPoolWorkflowBuilder updatedBuilder = builder;

            for (var i = 0; i < 1000; i++)
            {
                var seeds = Rando.Fast(i * 17).ToIntEnumerator().Take(5).ToList();

                updatedBuilder = SorterCompPoolWorkflowBuilder.UpdateAndTrack(
                        builder: updatedBuilder,
                        seeds: seeds,
                        mergeWithPrev: true,
                        tracker: tracker
                    );

                //    var bsetResult =
                //        tracker.SorterPoolStats
                //               .GenomeStatses
                //               .OrderBy(t => t.ReferenceResult.Cast<ISorterOnSwitchableGroup>().SwitchesUsed)
                //               .First().ReferenceResult
                //               .Cast<ISorterOnSwitchableGroup>();

                //    var averageScore = tracker.SorterPoolStats
                //                              .GenomeStatses.Average(t => t.ReferenceResult.Cast<ISorterOnSwitchableGroup>().SwitchesUsed);

                //    System.Diagnostics.Debug.WriteLine("{0}\t{1}\t{2}\t{3}", bsetResult.SwitchesUsed, bsetResult.Success, averageScore, i);
                System.Diagnostics.Debug.WriteLine(tracker.PoolReport);

                tracker = tracker.Trim(1000);
            }


            Assert.AreEqual(updatedBuilder.InputEntities.Count, 3);
            Assert.IsNotNull(updatedBuilder.Entity);
            Assert.AreEqual(updatedBuilder.Seeds.Count(), 25);
        }
    }
}
