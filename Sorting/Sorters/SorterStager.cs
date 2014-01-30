using System.Linq;
using Sorting.KeyPairs;

namespace Sorting.Sorters
{

    public interface ISorterStager
    {
        ISorterStage Current { get; }
        ISorterStage Previous { get; } 
    }

    public static class SorterStager
    {
        static readonly ISorterStager emptySorterStager = new SorterStagerImpl(SorterStage.Empty, SorterStage.Empty);

        public static ISorterStager Empty
        {
            get { return emptySorterStager; }
        }

        public static ISorterStager AppendKeyPair(this ISorterStager sorterStager, IKeyPair keyPair)
        {
            return 
                sorterStager.Current.KeyPairs.Any(kp => kp.Overlaps(keyPair))
                ? 
                new SorterStagerImpl(sorterStager.Current, SorterStage.Empty)
                : 
                new SorterStagerImpl(sorterStager.Current, sorterStager.Current.AppendKeyPair(keyPair));
        }
    }

    class SorterStagerImpl : ISorterStager
    {
        private readonly ISorterStage _current;
        private readonly ISorterStage _previous;

        public SorterStagerImpl(ISorterStage previous, ISorterStage current)
        {
            _previous = previous;
            _current = current;
        }

        public ISorterStage Current
        {
            get { return _current; }
        }

        public ISorterStage Previous
        {
            get { return _previous; }
        }
    }
}