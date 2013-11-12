using System.Runtime.InteropServices.ComTypes;
using Evo.Genomes;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace SorterEvo
{
    public static class GenomeToSorter
    {
        public static ISorter ToSorter<T>(this IGenome<T> genome, int keyCount)
        {
            var chromosome = (IChromosome<int>) genome.Chromosomes[0];
            return KeyPairRepository.KeyPairSet(keyCount).KeyPairs.ToSorter
                (
                    keyPairChoices: chromosome.Genes,
                    keyCount: keyCount,
                    guid: genome.Guid
                );
        }

    }
}
