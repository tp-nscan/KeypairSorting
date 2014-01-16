using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Chromosomes;
using Genomic.Genomes;
using MathUtils.Rand;
using Sorting.KeyPairs;

namespace SorterEvo.Genomes
{
    public interface ISorterGenome : ISimpleGenome<IChromosomeUint>
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
                chromosome: randy
                                .ToUintEnumerator(keyPairSetSize)
                                .Take(keyPairCount)
                                .ToList()
                                .ToChromosomeUint
                                (
                                    maxVal: keyPairSetSize
                                ),
                keyCount: keyCount,
                keyPairCount: keyPairCount
           );
        }

        public static ISorterGenome Make(
            Guid guid, 
            Guid parentGuid,
            IChromosomeUint chromosome, 
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

        public static Guid ProgenitorGuid(this ISorterGenome sorterGenome, IDictionary<Guid, ISorterGenome> genomeDictionary)
        {
            var guidRet = sorterGenome.Guid;
            ISorterGenome currentSorterGenome = sorterGenome;
            while (currentSorterGenome.ParentGuid != Guid.Empty)
            {
                currentSorterGenome = genomeDictionary[currentSorterGenome.ParentGuid];
            }

            return guidRet;
        }
    }

    class SorterGenomeImpl : SimpleGenomeImpl<IChromosomeUint>, ISorterGenome
    {
        public SorterGenomeImpl(
                Guid guid, 
                Guid parentGuid,
                IChromosomeUint chromosome, 
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
