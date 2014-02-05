using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;
using Sorting.KeyPairs;
using Sorting.Sorters.StageGenerators;
using Sorting.Switchables;

namespace Sorting.Sorters
{
    public interface ISorter
    {
        Guid Guid { get; }
        int KeyCount { get; }
        int KeyPairCount { get; }
        IKeyPair KeyPair(int index);
        IReadOnlyList<IKeyPair> KeyPairs { get; }
    }

    public static class Sorter
    {
        public static ISorter ToSorter(this IEnumerable<IKeyPair> keyPairs, Guid guid, int keyCount)
        {
            return new SorterImpl(keyPairs, guid, keyCount);
        }

        public static ISorter ToSorter2(this IEnumerable<IKeyPair> keyPairs, Guid guid, int keyCount)
        {
            IList<IReadOnlyList<IKeyPair>> keyPairGroups = keyPairs
                    .Slice(40)
                    .ToList();


            return StagedSorter.Make
                (
                    guid: guid,
                    keyCount: keyCount,
                    sorterStages: Enumerable.Range(0, 160)
                                    .Select(
                                    i => keyPairGroups[i].ToReducedSorterStage(SwitchableGroups[i])
                                    ).ToList()
                );
        }

        private const int SwitchableGroupSize = 120;

        private static List<ISwitchableGroup<uint>> _switchableGroups;
        public static IReadOnlyList<ISwitchableGroup<uint>> SwitchableGroups
        {
            get
            {
                return _switchableGroups ?? (
                    _switchableGroups = Enumerable.Range(0, 160)
                        .Select(i => Rando.Fast(109 + i)
                            .ToSwitchableGroup<uint>(Guid.NewGuid(), 16, SwitchableGroupSize))
                        .ToList()
                );

            }
        }

        public static ISorter ToSorter(this IReadOnlyList<IKeyPair> keyPairs, IEnumerable<uint> keyPairChoices, int keyCount, Guid guid)
        {
            return keyPairs.PickMembers(keyPairChoices).ToSorter(guid, keyCount);
        }

        public static ISorter ToSorter(this IRando rando, int keyCount, int keyPairCount, Guid guid)
        {
            var keyPairSet = KeyPairRepository.KeyPairSet(keyCount);
            return rando.ToSorter(keyPairSet.KeyPairs, keyPairCount, keyCount, guid);
        }

        public static ISorter ToSorter2(this IRando rando, int keyCount, int keyPairCount, Guid guid)
        {
            var keyPairSet = KeyPairRepository.KeyPairSet(keyCount);
            return rando.ToSorter2(keyPairSet.KeyPairs, keyPairCount, keyCount, guid);
        }

        public static ISorter ToSorter(this IRando rando, IReadOnlyList<IKeyPair> keyPairs, int keyPairCount, int keyCount, Guid guid)
        {
            return rando.Pick(keyPairs).Take(keyPairCount).ToSorter(guid, keyCount);
        }

        public static ISorter ToSorter2(this IRando rando, IReadOnlyList<IKeyPair> keyPairs, int keyPairCount, int keyCount, Guid guid)
        {
            return rando.Pick(keyPairs).Take(keyPairCount).ToSorter2(guid, keyCount);
        }

        public static IEnumerable<ISorter> Mutate(this ISorter sorter, IRando rando, double mutationRate)
        {
            var randoK = rando.Spawn();
            var keyPairList = KeyPairRepository.KeyPairSet(sorter.KeyCount).KeyPairs;
            var newb = randoK.Pick(keyPairList).ToMoveNext();
            while (true)
            {
                yield return new SorterImpl
                    (
                        keyPairs: sorter.KeyPairs.MutateInsertDelete
                        (
                             rando.Spawn().ToBoolEnumerator(mutationRate),       
                             rando.Spawn().ToBoolEnumerator(mutationRate),
                             rando.Spawn().ToBoolEnumerator(mutationRate),
                             T => newb.Next(),
                             T => newb.Next(),
                             T => newb.Next()
                        ),
                        guid: randoK.NextGuid(),
                        keyCount: sorter.KeyCount
                    );
            }
        }
    }

    class SorterImpl : ISorter
    {
        public SorterImpl(IEnumerable<IKeyPair> keyPairs, Guid guid, int keyCount)
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

        private readonly IKeyPair[] _keyPairs;
        public IKeyPair KeyPair(int index)
        {
            return _keyPairs[index];
        }

        public IReadOnlyList<IKeyPair> KeyPairs
        {
            get { return _keyPairs; }
        }
    }
}
