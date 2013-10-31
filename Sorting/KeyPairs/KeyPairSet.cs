using System.Collections.Generic;
using System.Linq;

namespace Sorting.KeyPairs
{
    public class KeyPairSet
    {
        public KeyPairSet(int keyCount)
        {
            _keyCount = keyCount;
            _keyPairs = KeyPairRepository.KeyPairsForKeyCount(KeyCount).ToList();
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        public KeyPair this[int index]
        {
            get { return _keyPairs[index]; }
        }

        readonly List<KeyPair> _keyPairs;
        public IReadOnlyList<KeyPair> KeyPairs
        {
            get { return _keyPairs; }
        }
    }
}
