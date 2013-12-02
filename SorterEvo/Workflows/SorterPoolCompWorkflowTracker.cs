using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genomic.Trackers;

namespace SorterEvo.Workflows
{
    public interface ISorterPoolCompWorkflowTracker : ITracker<ISorterPoolCompWorkflow>
    {
    }

    public static class SorterPoolCompWorkflowTracker
    {
    }

    class SorterPoolCompWorkflowTrackerImpl : ITracker<ISorterPoolCompWorkflow>
    {
        public ITracker<ISorterPoolCompWorkflow> TrackItem(ISorterPoolCompWorkflow itemToTrack)
        {
            throw new NotImplementedException();
        }
    }
}
