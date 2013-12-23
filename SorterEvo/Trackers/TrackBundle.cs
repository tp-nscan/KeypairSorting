using System.Collections.Generic;
using System.Linq;
using MathUtils.Rand;
using SorterEvo.Workflows;

namespace SorterEvo.Trackers
{
    public interface ITrackBundle
    {
        ISorterCompPoolParams SorterCompPoolParams { get; }
        ISorterCompPoolWorkflowTracker Tracker { get; }
        IEnumerable<int> Seeds { get; }
    }

    public static class TrackBundle
    {
        public static IEnumerable<ITrackBundle> Make
            (
                string baseName,
                int partitionCount,
                int repCount,
                int seed
            )
        {
            var seeds = Rando.Fast(seed).ToIntEnumerator().Take(repCount).ToList();
            for (var i = 0; i < repCount; i++)
            {
                foreach (var sorterCompPoolParams in SorterCompPoolParams.MakeStandards(baseName + "_r" + i, partitionCount))
                {
                    yield return new TrackBundleImpl(sorterCompPoolParams, seeds[i]);
                }
            }
        }
    }


    class TrackBundleImpl : ITrackBundle
    {
        public TrackBundleImpl(ISorterCompPoolParams sorterCompPoolParams, int rootSeed)
        {
            _sorterCompPoolParams = sorterCompPoolParams;
            _rootSeed = rootSeed;
            _rando = Rando.Fast(RootSeed);
        }


        private readonly ISorterCompPoolParams _sorterCompPoolParams;
        public ISorterCompPoolParams SorterCompPoolParams
        {
            get { return _sorterCompPoolParams; }
        }

        private readonly ISorterCompPoolWorkflowTracker _tracker = SorterCompPoolWorkflowTracker.Make();
        public ISorterCompPoolWorkflowTracker Tracker
        {
            get { return _tracker; }
        }

        private readonly IRando _rando;
        public IEnumerable<int> Seeds
        {
            get
            {
                return Rando.Fast(_rando.NextInt()).ToIntEnumerator().Take(5);
            }
        }

        private readonly int _rootSeed;
        private int RootSeed
        {
            get { return _rootSeed; }
        }
    }
}
