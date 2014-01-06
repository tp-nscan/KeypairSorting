using MathUtils.Rand;
using SorterEvo.Genomes;

namespace SorterEvo.TestData
{
    public static class Genomes
    {
        public static int Seed = 15394;
        public static int KeyCount = 12;
        public static int KeyPairCount = 1000;

        public static ISorterGenome SorterGenome
        {
            get
            {
                return Rando.Fast(Seed).ToSorterGenome(KeyCount, KeyPairCount);
            }
        }


    }
}
