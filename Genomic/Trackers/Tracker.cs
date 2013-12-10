namespace Genomic.Trackers
{
    public interface ITracker<T>
    {
        ITracker<T> TrackItem(T itemToTrack);
    }
}
