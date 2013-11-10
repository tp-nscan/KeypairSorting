namespace SorterEvo
{
    public interface ISorterGenome
    {
        int KeyCount { get; }
        int KeyPairCount { get; }
    }

    public static class SorterGenome
    {
        public static ISorterGenome Make()
        {
            return new SorterGenomeImpl();
        }
    }

    class SorterGenomeImpl : ISorterGenome
    {
        private int _keyCount;
        private int _keyPairCount;

        public int KeyCount
        {
            get { return _keyCount; }
        }

        public int KeyPairCount
        {
            get { return _keyPairCount; }
        }
    }

}
