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

        public static ISorter ToSorter(this IReadOnlyList<KeyPair> keyPairs, IEnumerable<int> keyPairChoices, int keyCount, Guid guid)
        {
            return keyPairs.PickMembers(keyPairChoices).ToSorter(guid, keyCount);
        }

        public static ISorter ToSorter(this IRando rando, int keyCount, int keyPairCount, Guid guid)
        {
            var keyPairSet = KeyPairRepository.KeyPairSet(keyCount);
            return rando.ToSorter(keyPairSet.KeyPairs, keyPairCount, keyCount, guid);
        }

        public static ISorter ToSorter(this IRando rando, IReadOnlyList<KeyPair> keyPairs, int keyPairCount, int keyCount, Guid guid)
        {
            return rando.Pick(keyPairs).Take(keyPairCount).ToSorter(guid, keyCount);
        }

        public static IEnumerable<ISorter> Mutate(this ISorter sorter, IRando rando, double mutationRate)
        {
            var randoK = rando.Spawn();
            var keyPairList = KeyPairRepository.KeyPairSet(sorter.KeyCount).KeyPairs;
            var newb = randoK.Pick(keyPairList).ToMoveNext();
            while (true)
            {
                //yield return new SorterImpl
                //    (
                //        keyPairs: sorter.KeyPairs.Mutate(rando.ToBoolEnumerator(mutationRate), T => randoK.Pick(keyPairList).First()),
                //        guid: Guid.NewGuid(),
                //        keyCount: sorter.KeyCount
                //    );
                yield return new SorterImpl
                    (
                        keyPairs: sorter.KeyPairs.MutateInsertDelete
                        (
                             rando.Spawn().ToBoolEnumerator(mutationRate),       
                             rando.Spawn().ToBoolEnumerator(mutationRate),
                             rando.Spawn().ToBoolEnumerator(mutationRate),
                             T => newb.Next(),
                             T => newb.Next(),
                             () => newb.Next()
                        ),
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
