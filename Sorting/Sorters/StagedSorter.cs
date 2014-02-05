using System;
using System.Collections.Generic;
using System.Linq;
using Sorting.KeyPairs;

namespace Sorting.Sorters
{
    public interface IStagedSorter : ISorter
    {
        ISorterStage SorterStage(int index);
        IReadOnlyList<ISorterStage> SorterStages { get; }
    }

    public static class StagedSorter
    {
        public static IStagedSorter Make(
                Guid guid,
                int keyCount,
                IReadOnlyList<ISorterStage> sorterStages
            )
        {
            return new StagedSorterImpl(guid, keyCount, sorterStages);
        }

        public static IStagedSorter ToStagedSorter(this ISorter sorter)
        {
            return new StagedSorterImpl
                (
                    guid: sorter.Guid,
                    keyCount: sorter.KeyCount,
                    sorterStages: sorter.KeyPairs.ToSorterStages(sorter.KeyCount)
                );
        }
    }

    class StagedSorterImpl : IStagedSorter
    {
        private readonly Guid _guid;
        private readonly int _keyCount;
        private readonly IReadOnlyList<ISorterStage> _sorterStages;

        public StagedSorterImpl
            (
                Guid guid, 
                int keyCount, 
                IReadOnlyList<ISorterStage> sorterStages
            )
        {
            _guid = guid;
            _keyCount = keyCount;
            _sorterStages = sorterStages;

            var tempList = new List<IKeyPair>();
            for (var i = 0; i < _sorterStages.Count; i++)
            {
                var curStage = _sorterStages[i];
                for (var j = 0; j < curStage.KeyPairs.Count; j++)
                {
                    tempList.Add(curStage.KeyPairs[j]);
                }
            }

            _keyPairs = tempList;
            _keyPairCount = SorterStages.Sum(s => s.KeyPairCount);
        }

        public Guid Guid
        {
            get { return _guid; }
        }

        public int KeyCount
        {
            get { return _keyCount; }
        }

        private readonly int _keyPairCount;
        public int KeyPairCount
        {
            get { return _keyPairCount; }
        }

        public IKeyPair KeyPair(int index)
        {
            return _keyPairs[index];
        }

        private IReadOnlyList<IKeyPair> _keyPairs = null;
        public IReadOnlyList<IKeyPair> KeyPairs
        {
            get { return _keyPairs ?? (_keyPairs = _sorterStages.SelectMany(ss=>ss.KeyPairs).ToList()); }
        }

        public ISorterStage SorterStage(int index)
        {
            return _sorterStages[index];
        }

        public IReadOnlyList<ISorterStage> SorterStages
        {
            get { return _sorterStages; }
        }
    }
}