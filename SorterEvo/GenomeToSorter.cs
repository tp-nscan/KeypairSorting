using System;
using Evo.Genomes;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace SorterEvo
{
    public static class GenomeToSorter
    {
        public static ISorter ToSorter(this IGenome genome, int keyCount)
        {
            var chromosome = genome.Chromosomes[0];
            return KeyPairRepository.KeyPairSet(keyCount).KeyPairs.ToSorter
                (
                    keyPairChoices: chromosome,
                    keyCount: keyCount,
                    guid: genome.Guid
                );
        }

    }
}
