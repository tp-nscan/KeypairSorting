using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;
using Sorting.KeyPairs;

namespace Sorting.Sorters
{
    public interface ISorter
    {
        Guid Guid { get; }
        int KeyCount { get; }
        int KeyPairCount { get; }
        KeyPair KeyPair(int index);
        IReadOnlyList<KeyPair> KeyPairs { get; }
    }

    public static class Sorter
    {
        public static ISorter ToSorter(this IEnumerable<KeyPair> keyPairs, Guid guid, int keyCount)
        {
            return new SorterImpl(keyPairs, guid, keyCount);
        }

        public static IEnumerable<ISorter> RandomSorters(this IRando rando, IReadOnlyList<KeyPair> keyPairs, int keyPairCount, int keyCount)
        {
            return rando.ToRandomEnumerator().Select(T => T.ToSorter(keyPairs, keyPairCount, keyCount));
        }

        public static ISorter ToSorter(this IRando rando, IReadOnlyList<KeyPair> keyPairs, int keyPairCount, int keyCount)
        {
            return rando.Pick(keyPairs).Take(keyPairCount).ToSorter(Guid.NewGuid(), keyCount);
        }

        public static ISorter ToSorter(this IReadOnlyList<KeyPair> keyPairs, IEnumerable<int> keyPairChoices, int keyCount)
        {
            return keyPairs.PickMembers(keyPairChoices).ToSorter(Guid.NewGuid(), keyCount);
        }

        public static ISorter RandomSorter(int seed, int keyCount, int keyPairCount)
        {
            var keyPairSet = KeyPairRepository.KeyPairSet(keyCount);
            return Rando.Fast(seed).ToSorter(keyPairSet.KeyPairs, keyPairCount, keyCount);
        }

        public static IEnumerable<ISorter> Mutate(this ISorter sorter, IRando rando, double mutationRate)
        {
            var randoK = rando.Spawn();
            var keyPairList = KeyPairRepository.KeyPairSet(16).KeyPairs;
            while (true)
            {
                yield return new SorterImpl
                    (
                        keyPairs: sorter.KeyPairs.Mutate(rando.ToBoolEnumerator(mutationRate), T => randoK.Pick(keyPairList).First()),
                        guid: Guid.NewGuid(),
                        keyCount: sorter.KeyCount
                    );
            }
        }
    }

    class SorterImpl : ISorter
    {
        public SorterImpl(IEnumerable<KeyPair> keyPairs, Guid guid, int keyCount)
        {
            _guid = guid;
            _keyCount = keyCount;
            _keyPairs = keyPairs.ToArray();
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        public int KeyPairCount
        {
            get { return _keyPairs.Length; }
        }

        private readonly KeyPair[] _keyPairs;
        public KeyPair KeyPair(int index)
        {
            return _keyPairs[index];
        }

        public IReadOnlyList<KeyPair> KeyPairs
        {
            get { return _keyPairs; }
        }
    }
}
