using System;
using System.Linq;
using Genomic.Chromosomes;
using Genomic.Genes;
using Genomic.Genomes;
using MathUtils.Rand;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace SorterEvo.Genomes
{
    public interface ISorterGenome : ISimpleGenome<IChromosome<IGeneUintModN>>
    {
        int KeyCount { get; }
        int KeyPairCount { get; }
    }

    public static class SorterGenome
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

        public static ISorterGenome ToSorterGenome(
            this IRando randy, 
            int keyCount, 
            int keyPairCount)
        {
            return new SorterGenomeImpl
                (
                    randy: randy, 
                    keyCount: keyCount, 
                    keyPairCount: keyPairCount
                );
        }

        public static ISorterGenome Make(
            Guid guid, 
            Guid parentGuid, 
            IChromosome<IGeneUintModN> chromosome, 
            int keyCount, 
            int keyPairCount)
        {
            return new SorterGenomeImpl
                (
                    guid: guid, 
                    parentGuid:parentGuid, 
                    chromosome: chromosome, 
                    keyCount: keyCount, 
                    keyPairCount: keyPairCount
               );
        }
    }

    class SorterGenomeImpl : SimpleGenomeImpl<IChromosome<IGeneUintModN>>, ISorterGenome
    {
        public SorterGenomeImpl(IRando randy, int keyCount, int keyPairCount) 
            : base
            (
                guid: randy.NextGuid(),
                chromosome: randy.ToUintEnumerator((uint) KeyPairRepository.KeyPairSetSizeForKeyCount(keyCount))
                                 .Take(keyPairCount)
                                 .ToChromosomeUintN
                                   (
                                    guid: randy.NextGuid(), 
                                    maxVal: (uint) KeyPairRepository.KeyPairSetSizeForKeyCount(keyCount)
                                   ),
                parentGuid: Guid.Empty
            )
        {
            _keyCount = keyCount;
            _keyPairCount = keyPairCount;
        }

        public SorterGenomeImpl(
                Guid guid, 
                Guid parentGuid, 
                IChromosome<IGeneUintModN> chromosome, 
                int keyCount, 
                int keyPairCount
            ) 
            : base
            (
                guid: guid,
                chromosome: chromosome,
                parentGuid: parentGuid
            )
        {
            _keyCount = keyCount;
            _keyPairCount = keyPairCount;
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        private readonly int _keyPairCount;
        public int KeyPairCount
        {
            get { return _keyPairCount; }
        }
    }

}
