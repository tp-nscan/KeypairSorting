using System;
using Genomic;
using MathUtils.Rand;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace SorterEvo
{
    public interface ISorterGenome : ISimpleGenome<IChromosome<GeneUintModN>>
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
            IChromosome<GeneUintModN> chromosome, 
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

    class SorterGenomeImpl : SimpleGenomeImpl<IChromosome<GeneUintModN>>, ISorterGenome
    {
        public SorterGenomeImpl(IRando randy, int keyCount, int keyPairCount) 
            : base
            (
                guid: randy.NextGuid(),
                chromosome: randy.ToUniformChromosome
                    (
                        guid: randy.NextGuid(),
                        symbolCount: (uint) KeyPairRepository.KeyPairSetSizeForKeyCount(keyCount),
                        sequenceLength: keyPairCount
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
                IChromosome<GeneUintModN> chromosome, 
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
