using System;
using System.Linq;
using Genomic.Chromosomes;
using Genomic.Genes;
using Genomic.Genomes;
using MathUtils.Collections;
using MathUtils.Rand;
using Sorting.Switchables;

namespace SorterEvo.Genomes
{
    public interface ISwitchableGroupGenome : ISimpleGenome<IChromosome>
    {
        int GroupCount { get; }
        int KeyCount { get; }
        SwitchableGroupGenomeType SwitchableGroupGenomeType { get; }
    }

    public static class SwitchableGroupGenome 
    {
        public static ISwitchableGroupGenome ToSwitchableGroupGenome(
            this IRando rando, 
            SwitchableGroupGenomeType switchableGroupGenomeType,
            int keyCount,
            int groupSize
            )
        {
            switch (switchableGroupGenomeType)
            {
                case SwitchableGroupGenomeType.UInt:
                    return new SwitchableGroupGenomeImpl
                        (
                            guid: rando.NextGuid(),
                            parentGuid: Guid.Empty,
                            keyCount: keyCount,
                            chromosome: rando.ToUintEnumerator(((uint)1) << (keyCount - 1))
                                             .Take(groupSize)
                                             .ToList()
                                             .ToChromosomeUint(((uint)1) << keyCount),
                            groupCount: groupSize
                        );

                case SwitchableGroupGenomeType.ULong:
                    return new SwitchableGroupGenomeImpl
                        (
                            guid: rando.NextGuid(),
                            parentGuid: Guid.Empty,
                            keyCount: keyCount,
                            chromosome: rando.ToUlongEnumerator(((ulong)1) << (keyCount - 1))
                                             .Take(groupSize)
                                             .ToUints()
                                             .ToList()
                                             .ToChromosomeUlongN(((ulong)1) << keyCount),
                            groupCount: groupSize
                        );

                case SwitchableGroupGenomeType.BitArray:
                    return new SwitchableGroupGenomeImpl
                        (
                            guid: rando.NextGuid(),
                            parentGuid: Guid.Empty,
                            keyCount: keyCount,
                            chromosome: rando.ToUintEnumerator(2)
                                             .Take(groupSize * keyCount)
                                             .ToList()
                                             .ToChromosomeBits(rando.NextGuid(), keyCount),
                            groupCount: groupSize
                        );

                case SwitchableGroupGenomeType.IntArray:

                    return new SwitchableGroupGenomeImpl
                        (
                            guid: rando.NextGuid(),
                            parentGuid: Guid.Empty,
                            keyCount: keyCount,
                            chromosome: Enumerable
                                .Range(0, groupSize)
                                .SelectMany(i => Enumerable
                                                    .Range(0, keyCount)
                                                    .Select(t=>(uint)t)
                                                    .ToList()
                                                    .FisherYatesShuffle(rando)
                                            )
                                .ToList()
                                .ToChromosomePermutation(rando.NextGuid(), keyCount, 0.2),

                            groupCount: groupSize
                        );

                default:
                    throw new Exception();
            }
        }

        public static ISwitchableGroupGenome ToSwitchableGroupGenome(this ISwitchableGroup switchableGroup)
        {
            if (switchableGroup.SwitchableDataType == typeof(uint))
            {
                var symbolCount = 2 ^ (uint)switchableGroup.KeyCount;
                return new SwitchableGroupGenomeImpl
                    (
                        guid: switchableGroup.Guid,
                        parentGuid: Guid.Empty,
                        keyCount: switchableGroup.KeyCount,
                        chromosome: ((ISwitchableGroup<uint>)switchableGroup)
                                            .Switchables
                                            .Select(t => t.Item)
                                            .ToList()
                                            .ToChromosomeUint(symbolCount),
                        groupCount: switchableGroup.SwitchableCount
                    );
            }

            if (switchableGroup.SwitchableDataType == typeof(ulong))
            {
                var symbolCount = 2 ^ (ulong) switchableGroup.KeyCount;
                return new SwitchableGroupGenomeImpl
                    (
                        guid: switchableGroup.Guid,
                        parentGuid: Guid.Empty,
                        keyCount: switchableGroup.KeyCount,
                        chromosome: ((ISwitchableGroup<uint>)switchableGroup)
                                            .Switchables
                                            .Select(t => t.Item)
                                            .ToList()
                                            .ToChromosomeUlongN(symbolCount),
                        groupCount: switchableGroup.SwitchableCount
                    );
            }

            if (switchableGroup.SwitchableDataType == typeof(int[]))
            {
                var symbolCount = switchableGroup.KeyCount;
                var mixingRate = 0.3;
                return new SwitchableGroupGenomeImpl
                    (
                        guid: switchableGroup.Guid,
                        parentGuid: Guid.Empty,
                        keyCount: switchableGroup.KeyCount,
                        chromosome: ((ISwitchableGroup<int[]>)switchableGroup)
                                            .Switchables
                                            .SelectMany(t => t.Item.Cast<uint>())
                                            .ToList()
                                            .ToChromosomePermutation(Guid.NewGuid(), symbolCount, mixingRate),
                        groupCount: switchableGroup.SwitchableCount
                    );
            }

            if (switchableGroup.SwitchableDataType == typeof(bool[]))
            {
                const int symbolCount = 2;
                return new SwitchableGroupGenomeImpl
                    (
                        guid: switchableGroup.Guid,
                        parentGuid: Guid.Empty,
                        keyCount: switchableGroup.KeyCount,
                        chromosome: ((ISwitchableGroup<bool[]>)switchableGroup)
                                            .Switchables
                                            .SelectMany(t => t.Item.Cast<uint>())
                                            .ToList()
                                            .ToChromosomeBits(Guid.NewGuid(), symbolCount),
                        groupCount: switchableGroup.SwitchableCount
                    );
            }

            throw new Exception(string.Format("data type {0} not  handled", switchableGroup.SwitchableDataType.Name));
        }


        public static IChromosome CopyChromosome
            (
                this ISwitchableGroupGenome switchableGroupGenome,
                IRando randy,
                double mutationRate,
                double insertionRate,
                double deletionRate
            )
        {
            switch (switchableGroupGenome.SwitchableGroupGenomeType)
            {
                case SwitchableGroupGenomeType.UInt:
                    var chrom = (IChromosome<IGeneUintModN>)switchableGroupGenome.Chromosome;
                    return chrom.StandardPropigate(randy, mutationRate, insertionRate, deletionRate);
                case SwitchableGroupGenomeType.ULong:
                    var chrom2 = (IChromosome<IGeneUlongModN>)switchableGroupGenome.Chromosome;
                    return chrom2.StandardPropigate(randy, mutationRate, insertionRate, deletionRate);
                case SwitchableGroupGenomeType.BitArray:
                    var chrom3 = (IChromosome<IGeneBits>)switchableGroupGenome.Chromosome;
                    return chrom3.StandardPropigate(randy, mutationRate, insertionRate, deletionRate);
                case SwitchableGroupGenomeType.IntArray:
                    var chrom4 = (IChromosome<IGenePermutation>)switchableGroupGenome.Chromosome;
                    return chrom4.StandardPropigate(randy, mutationRate, insertionRate, deletionRate);
                default:
                    throw new Exception("SwitchableGroupGenomeType not handled");
            }
        }

        public static ISwitchableGroupGenome Make
            (
                        Guid guid,
                        Guid parentGuid,
                        int keyCount,
                        IChromosome chromosome,
                        SwitchableGroupGenomeType switchableGroupGenomeType,
                        int groupCount
            )
        {
            switch (switchableGroupGenomeType)
            {
                case SwitchableGroupGenomeType.UInt:
                    return new SwitchableGroupGenomeImpl(guid, parentGuid, keyCount, (IChromosomeUint)chromosome, groupCount);
                case SwitchableGroupGenomeType.ULong:
                    return new SwitchableGroupGenomeImpl(guid, parentGuid, keyCount, (IChromosomeUlongN)chromosome, groupCount);
                case SwitchableGroupGenomeType.BitArray:
                    return new SwitchableGroupGenomeImpl(guid, parentGuid, keyCount, (IChromosomeBits)chromosome, groupCount);
                case SwitchableGroupGenomeType.IntArray:
                    return new SwitchableGroupGenomeImpl(guid, parentGuid, keyCount, (IChromosomePermutation)chromosome, groupCount);
                default:
                    throw new Exception("SwitchableGroupGenomeType not handled");
            }
        }
    }

    class SwitchableGroupGenomeImpl : SimpleGenomeImpl<IChromosome>, ISwitchableGroupGenome
    {
        public SwitchableGroupGenomeImpl
        (
            Guid guid,
            Guid parentGuid,
            int keyCount,
            IChromosomeUint chromosome,
            int groupCount
        )
            : base
            (
                guid: guid,
                chromosome: chromosome,
                parentGuid: parentGuid
            )
        {
            _keyCount = keyCount;
            _switchableGroupGenomeType = SwitchableGroupGenomeType.UInt;
            _groupCount = groupCount;
        }

        public SwitchableGroupGenomeImpl
        (
            Guid guid,
            Guid parentGuid,
            int keyCount,
            IChromosomeUlongN chromosome,
            int groupCount
        )
            : base
            (
                guid: guid,
                chromosome: chromosome,
                parentGuid: parentGuid
            )
        {
            _keyCount = keyCount;
            _switchableGroupGenomeType = SwitchableGroupGenomeType.ULong;
            _groupCount = groupCount;
        }

        public SwitchableGroupGenomeImpl
            (
                Guid guid,
                Guid parentGuid,
                int keyCount,
                IChromosomeBits chromosome,
                int groupCount
            )
            : base
            (
                guid: guid,
                chromosome: chromosome,
                parentGuid: parentGuid
            )
        {
            _keyCount = keyCount;
            _switchableGroupGenomeType = SwitchableGroupGenomeType.BitArray;
            _groupCount = groupCount;
        }

        public SwitchableGroupGenomeImpl
        (
            Guid guid,
            Guid parentGuid,
            int keyCount,
            IChromosomePermutation chromosome,
            int groupCount
        )
            : base
            (
                guid: guid,
                chromosome: chromosome,
                parentGuid: parentGuid
            )
        {
            _keyCount = keyCount;
            _switchableGroupGenomeType = SwitchableGroupGenomeType.IntArray;
            _groupCount = groupCount;
        }

        //public SwitchableGroupGenomeImpl
        //    (
        //        Guid guid,
        //        Guid parentGuid,
        //        int keyCount,
        //        IChromosome chromosome,
        //        SwitchableGroupGenomeType switchableGroupGenomeType, 
        //        int groupCount
        //    )
        //    : base
        //    (
        //        guid: guid,
        //        chromosome: chromosome,
        //        parentGuid: parentGuid
        //    )
        //{
        //    _keyCount = keyCount;
        //    _switchableGroupGenomeType = switchableGroupGenomeType;
        //    _groupCount = groupCount;
        //}

        private readonly int _groupCount;
        public int GroupCount
        {
            get { return _groupCount; }
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        private readonly SwitchableGroupGenomeType _switchableGroupGenomeType;
        public SwitchableGroupGenomeType SwitchableGroupGenomeType
        {
            get { return _switchableGroupGenomeType; }
        }
    }
}
