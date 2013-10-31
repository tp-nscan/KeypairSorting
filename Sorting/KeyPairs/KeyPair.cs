namespace Sorting.KeyPairs
{
    public class KeyPair
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
