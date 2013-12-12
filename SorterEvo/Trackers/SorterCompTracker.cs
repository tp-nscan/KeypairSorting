using Genomic.Trackers;
using SorterEvo.Workflows;

namespace SorterEvo.Trackers
{
    public interface ISorterPoolCompWorkflowTracker : ITracker<ISorterCompWorkflow>
    {
    }

    public static class SorterCompTracker
    {
        public static ISorterPoolCompWorkflowTracker Make()
        {
            return new SorterPoolCompWorkflowTrackerImpl();
        }
    }

    class SorterPoolCompWorkflowTrackerImpl : ISorterPoolCompWorkflowTracker
    {
        public ITracker<ISorterCompWorkflow> TrackItem(ISorterCompWorkflow itemToTrack)
        {
            System.Diagnostics.Debug.WriteLine("In TrackItem");
            return this;
        }
    }
}
