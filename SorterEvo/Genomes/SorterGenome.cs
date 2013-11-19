using System;
using System.Linq;
using Genomic.Chromosomes;
using Genomic.Genes;
using Genomic.Genomes;
using MathUtils.Rand;
using Sorting.KeyPairs;

namespace SorterEvo.Genomes
{
    public interface ISorterGenome : ISimpleGenome<IChromosome<IGeneUintModN>>
    {
        int KeyCount { get; }
        int KeyPairCount { get; }
    }

    public static class SorterGenome
    {
        public static ISorterGenome ToSorterGenome(
            this IRando randy, 
            int keyCount, 
            int keyPairCount)
        {
            var keyPairSetSize = (uint) KeyPairRepository.KeyPairSetSizeForKeyCount(keyCount);
            return new SorterGenomeImpl
            (
                guid: randy.NextGuid(),
                parentGuid: Guid.Empty,
                chromosome: randy.ToUintEnumerator(keyPairSetSize)
                                 .Take(keyPairCount)
                                 .ToChromosomeUintN
                                   (
                                        guid: randy.NextGuid(),
                                        maxVal: keyPairSetSize
                                   ),
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
