using Genomic.Trackers;
using SorterEvo.Workflows;

namespace SorterEvo.Trackers
{
    public interface ISorterPoolCompWorkflowTracker : ITracker<ISorterPoolCompWorkflow>
    {
    }

    public static class SorterPoolCompWorkflowTracker
    {
        public static ISorterPoolCompWorkflowTracker Make()
        {
            return new SorterPoolCompWorkflowTrackerImpl();
        }
    }

    class SorterPoolCompWorkflowTrackerImpl : ISorterPoolCompWorkflowTracker
    {
        public ITracker<ISorterPoolCompWorkflow> TrackItem(ISorterPoolCompWorkflow itemToTrack)
        {
            System.Diagnostics.Debug.WriteLine("In TrackItem");
            return this;
        }
    }
}
