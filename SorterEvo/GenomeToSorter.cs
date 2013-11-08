using System;
using Evo.Genomes;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace SorterEvo
{
    public static class GenomeToSorter
    {
        public static ISorter ToSorter(this IChromosome chromosome, int keyCount)
        {
            return KeyPairRepository.KeyPairSet(keyCount).KeyPairs.ToSorter
                (
                    keyPairChoices: chromosome,
                    keyCount: keyCount,
                    guid: chromosome.Id
                );
        }
    }
}
