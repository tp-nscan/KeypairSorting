using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genomic.Trackers
{
    public interface ITracker<T>
    {
        ITracker<T> TrackItem(T itemToTrack);
    }

    public static class Tracker
    {

    }

    class TrackerImpl<T> : ITracker<T>
    {
        public ITracker<T> TrackItem(T itemToTrack)
        {
            throw new NotImplementedException();
        }
    }

}
