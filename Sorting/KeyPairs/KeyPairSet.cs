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

        public IKeyPair this[int index]
        {
            get { return _keyPairs[index]; }
        }

        readonly List<IKeyPair> _keyPairs;
        public IReadOnlyList<IKeyPair> KeyPairs
        {
            get { return _keyPairs; }
        }
    }
}
