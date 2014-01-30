using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Permissions;
using Sorting.KeyPairs;

namespace Sorting.Sorters
{
    public interface ISorterStage
    {
        int KeyCount { get; }
        int KeyPairCount { get; }
        IKeyPair KeyPair(int index);
        IReadOnlyList<IKeyPair> KeyPairs { get; }
        ISorterStage AppendKeyPair(IKeyPair keyPair);
    }

    public static class SorterStage
    {
        static readonly ISorterStage emptySorterStage = new SorterStageImpl(0, Enumerable.Empty<IKeyPair>().ToList());

        public static ISorterStage Empty
        {
            get { return emptySorterStage; }
        }

        public static ISorterStage Make(int keyCount, IReadOnlyList<IKeyPair> keyPairs)
        {
            return new SorterStageImpl(keyCount, keyPairs);
        }

        public static IReadOnlyList<ISorterStage> ToSorterStages(this IReadOnlyList<IKeyPair> keyPairs, int keyCount)
        {
            var retSorterStages = new List<ISorterStage>();
            var sorterStager = SorterStager.Empty;

            for (var i = 0; i < keyPairs.Count; i++)
            {
                sorterStager = sorterStager.AppendKeyPair(keyPairs[i]);
                if (sorterStager.Current == Empty)
                {
                    retSorterStages.Add(sorterStager.Previous);
                    sorterStager = SorterStager.Empty.AppendKeyPair(keyPairs[i]);
                }
            }
            if (sorterStager.Current != Empty)
            {
                retSorterStages.Add(sorterStager.Current);
            }
            return retSorterStages;
        }

    }

    class SorterStageImpl : ISorterStage
    {
        private readonly int _keyCount;
        private readonly IImmutableList<IKeyPair> _keyPairs 
            = ImmutableList<IKeyPair>.Empty;

        public SorterStageImpl(int keyCount, IReadOnlyList<IKeyPair> keyPairs)
        {
            _keyCount = keyCount;
            _keyPairs =  _keyPairs.AddRange(keyPairs.OrderBy(kp=>kp.Index));
        }

        public int KeyCount
        {
            get { return _keyCount; }
        }

        public int KeyPairCount
        {
            get { return _keyPairs.Count; }
        }

        public IKeyPair KeyPair(int index)
        {
            return _keyPairs[index];
        }

        public IReadOnlyList<IKeyPair> KeyPairs
        {
            get { return _keyPairs; }
        }

        public ISorterStage AppendKeyPair(IKeyPair keyPair)
        {
            return new SorterStageImpl(KeyCount, _keyPairs.Add(keyPair));
        }
    }
}
