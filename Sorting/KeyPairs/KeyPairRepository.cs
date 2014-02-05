using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Rand;

namespace Sorting.KeyPairs
{
    public interface IKeyPair
    {
        int LowKey { get; }
        int HiKey { get; }
        int Index { get; }
    }

    public static class KeyPairRepository
    {
        private static readonly List<KeyPairSet> keyPairSets = Enumerable.Repeat<KeyPairSet>(null, 64).ToList();

        public static IEnumerable<IKeyPair> RandomKeyPairs(int keyCount, int numKeyPairs, int seed)
        {
            var rando = Rando.Fast(seed);
            for (var i = 0; i < numKeyPairs; i++)
            {
                yield return AtIndex(rando.NextInt(KeyPairSetSizeForKeyCount(keyCount)));
            }
        }

        static KeyPairRepository()
        {
            keyPairs = new IKeyPair[KeyPairSetSizeForKeyCount(MaxKeyCount)];

            for (var hiKey = 1; hiKey < MaxKeyCount; hiKey++)
            {
                for (var lowKey = 0; lowKey < hiKey; lowKey++)
                {
                    var keyPairIndex = KeyPairIndex(lowKey, hiKey);
                    keyPairs[keyPairIndex] = new KeyPair(keyPairIndex, lowKey, hiKey);
                }
            }
        }

        public static int KeyPairIndex(int lowKey, int hiKey)
        {
            if (hiKey == 1)
            {
                return 0;
            }

            return KeyPairSetSizeForKeyCount(hiKey) + lowKey;
        }

        private static readonly IKeyPair[] keyPairs;
        public static IEnumerable<IKeyPair> KeyPairsForKeyCount(int keyCount)
        {
            for (var i = 0; i < KeyPairSetSizeForKeyCount(keyCount); i++)
            {
                yield return keyPairs[i];
            }
        }

        public static IKeyPair AtIndex(int dex)
        {
            return keyPairs[dex]; 
        }

        public static IKeyPair ForKeys(int lowKey, int highKey)
        {
            return AtIndex(KeyPairIndex(lowKey, highKey));
        }

        public static KeyPairSet KeyPairSet(int keyCount)
        {
            return keyPairSets[keyCount] ?? (keyPairSets[keyCount] = new KeyPairSet(keyCount));
        }

        public static int KeyPairSetSizeForKeyCount(int keyCount)
        {
            return (keyCount * (keyCount - 1)) / 2;
        }

        public static bool Overlaps(this IKeyPair lhs, IKeyPair rhs)
        {
            return (lhs.LowKey == rhs.LowKey)
                   ||
                   (lhs.LowKey == rhs.HiKey)
                   ||
                   (lhs.HiKey == rhs.LowKey)
                   ||
                   (lhs.HiKey == rhs.HiKey);
        }

        public static int MaxKeyCount { get { return 64; } }

        class KeyPair : IKeyPair
        {
            public KeyPair(int index, int lowKey, int hiKey)
            {
                _index = index;
                _lowKey = lowKey;
                _hiKey = hiKey;
            }

            private readonly int _lowKey;
            public int LowKey
            {
                get { return _lowKey; }
            }

            private readonly int _hiKey;
            public int HiKey
            {
                get { return _hiKey; }
            }

            private readonly int _index;
            public int Index
            {
                get { return _index; }
            }
        }
    }

}
