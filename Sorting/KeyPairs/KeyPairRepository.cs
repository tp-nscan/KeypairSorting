using System.Collections.Generic;
using System.Linq;

namespace Sorting.KeyPairs
{
    public static class KeyPairRepository
    {
        private static readonly List<KeyPairSet> _keyPairSets = Enumerable.Repeat<KeyPairSet>(null, 64).ToList();

        static KeyPairRepository()
        {
            KeyPairs = new KeyPair[KeyPairSetSizeForKeyCount(MaxKeyCount)];

            for (var hiKey = 1; hiKey < MaxKeyCount; hiKey++)
            {
                for (var lowKey = 0; lowKey < hiKey; lowKey++)
                {
                    var keyPairIndex = KeyPairIndex(lowKey, hiKey);
                    KeyPairs[keyPairIndex] = new KeyPair(keyPairIndex, lowKey, hiKey);
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

        private static readonly KeyPair[] KeyPairs;

        public static IEnumerable<KeyPair> KeyPairsForKeyCount(int keyCount)
        {
            for (var i = 0; i < KeyPairSetSizeForKeyCount(keyCount); i++)
            {
                yield return KeyPairs[i];
            }
        }

        public static KeyPairSet KeyPairSet(int keyCount)
        {
            return _keyPairSets[keyCount] ?? (_keyPairSets[keyCount] = new KeyPairSet(keyCount));
        }

        public static int KeyPairSetSizeForKeyCount(int keyCount)
        {
            return (keyCount * (keyCount - 1)) / 2;
        }

        public static int MaxKeyCount { get { return 64; } }
    }

}
