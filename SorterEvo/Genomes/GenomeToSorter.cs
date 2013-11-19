using Sorting.KeyPairs;
using Sorting.Sorters;

namespace SorterEvo.Genomes
{
    public static class GenomeToSorter
    {
        public static ISorter ToSorter(this ISorterGenome sorterGenome)
        {
            return KeyPairRepository.KeyPairSet(sorterGenome.KeyCount)
                .KeyPairs.ToSorter
                (
                    keyPairChoices: sorterGenome.Chromosome.Sequence,
                    keyCount: sorterGenome.KeyCount,
                    guid: sorterGenome.Guid
                );
        }
    }
}
