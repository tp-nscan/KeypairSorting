//using System.Collections.Generic;
//using System.Linq;
//using MathUtils.Rand;
//using SorterEvo.Workflows;

//namespace SorterEvo.Trackers
//{
//    public interface ITrackBundle
//    {
//        IScpParams ScpParams { get; }
//        IScpWorkflowTracker Tracker { get; }
//        IEnumerable<int> Seeds { get; }
//    }

//    public static class TrackBundle
//    {
//        public static IEnumerable<ITrackBundle> Make
//            (
//                string baseName,
//                int partitionCount,
//                int repCount,
//                int seed
//            )
//        {
//            var seeds = Rando.Fast(seed).ToIntEnumerator().Take(repCount).ToList();
//            for (var i = 0; i < repCount; i++)
//            {
//                foreach (var scpParams in ScpParams.MakeStandards(baseName + "_r" + i, partitionCount))
//                {
//                    yield return new TrackBundleImpl(scpParams, seeds[i]);
//                }
//            }
//        }
//    }


//    class TrackBundleImpl : ITrackBundle
//    {
//        public TrackBundleImpl(IScpParams scpParams, int rootSeed)
//        {
//            _scpParams = scpParams;
//            _rootSeed = rootSeed;
//            _rando = Rando.Fast(RootSeed);
//        }


//        private readonly IScpParams _scpParams;
//        public IScpParams ScpParams
//        {
//            get { return _scpParams; }
//        }

//        private readonly IScpWorkflowTracker _tracker = ScpWorkflowTracker.Make();
//        public IScpWorkflowTracker Tracker
//        {
//            get { return _tracker; }
//        }

//        private readonly IRando _rando;
//        public IEnumerable<int> Seeds
//        {
//            get
//            {
//                return Rando.Fast(_rando.NextInt()).ToIntEnumerator().Take(5);
//            }
//        }

//        private readonly int _rootSeed;
//        private int RootSeed
//        {
//            get { return _rootSeed; }
//        }
//    }
//}
