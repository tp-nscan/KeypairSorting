using System;
using System.Linq;
using Genomic.Chromosomes;
using Genomic.Genes;
using Sorting.Switchables;

namespace SorterEvo.Genomes
{
    public static class GenomeToSwitchableGroup
    {
        public static ISwitchableGroup ToSwitchableGroup(this ISwitchableGroupGenome genome)
        {
            switch (genome.SwitchableGroupGenomeType)
            {
                case SwitchableGroupGenomeType.UInt:

                    return ((IChromosome<IGeneUintModN>)genome.Chromosome)
                            .Blocks.Select(b => b.Val.ToSwitchableUint(genome.KeyCount))
                            .ToSwitchableGroup(genome.Guid, genome.KeyCount);

                case SwitchableGroupGenomeType.ULong:

                    return ((IChromosome<IGeneUlongModN>)genome.Chromosome)
                            .Blocks.Select(b => b.Val.ToSwitchableUlong(genome.KeyCount))
                            .ToSwitchableGroup(genome.Guid, genome.KeyCount);

                case SwitchableGroupGenomeType.BitArray:

                    return ((IChromosome<IGeneBits>)genome.Chromosome)
                            .Blocks.Select(b => b.Bits.ToSwitchableBitArray())
                            .ToSwitchableGroup(genome.Guid, genome.KeyCount);

                case SwitchableGroupGenomeType.IntArray:

                    return ((IChromosome<IGenePermutation>)genome.Chromosome)
                            .Blocks.Select(b => b.Items.ToSwitchableIntArray())
                            .ToSwitchableGroup(genome.Guid, genome.KeyCount);

                default:
                    throw new Exception(string.Format("SwitchableGroupGenomeType: {0} unkown", genome.SwitchableGroupGenomeType));
            }
        }

        //public static ISwitchableGroup<T> ToSwitchableGroup<T>(this IGenome<T> genome, int keyCount)
        //{
        //    var chromosome = genome.Chromosomes[0];


        //    return KeyPairRepository.KeyPairSet(keyCount).KeyPairs.ToSorter
        //        (
        //            keyPairChoices: chromosome.Genes,
        //            keyCount: keyCount,
        //            guid: genome.Guid
        //        );
        //}

    }
}
