﻿using System;
using System.Linq;
using Genomic.Chromosomes;
using Genomic.Genomes;
using MathUtils.Collections;
using MathUtils.Rand;
using Sorting.Switchables;

namespace SorterEvo.Genomes
{
    public interface ISwitchableGroupGenome : ISimpleGenome<IChromosome>
    {
        int KeyCount { get; }
        SwitchableGroupGenomeType SwitchableGroupGenomeType { get; }
    }

    public static class SwitchableGroupGenome 
    {
        public static ISwitchableGroupGenome ToSwitchableGroupGenome(
            this IRando rando, 
            SwitchableGroupGenomeType switchableGroupGenomeType,
            int keyCount,
            int groupSize)
        {
            switch (switchableGroupGenomeType)
            {
                case SwitchableGroupGenomeType.UInt:
                    return new SwitchableGroupGenomeImpl
                        (
                            guid: rando.NextGuid(),
                            keyCount: keyCount,
                            chromosome: rando.ToUintEnumerator(((uint)1) << (keyCount - 1))
                                             .Take(groupSize)
                                             .ToChromosomeUintN(rando.NextGuid(), ((uint)1) << keyCount),
                            switchableGroupGenomeType: SwitchableGroupGenomeType.UInt
                        );

                case SwitchableGroupGenomeType.ULong:
                    return new SwitchableGroupGenomeImpl
                        (
                            guid: rando.NextGuid(),
                            keyCount: keyCount,
                            chromosome: rando.ToUlongEnumerator(((ulong)1) << (keyCount - 1))
                                                .Take(groupSize)
                                                .ToUints()
                                                .ToChromosomeUlongN(rando.NextGuid(), ((ulong)1) << keyCount),
                            switchableGroupGenomeType: SwitchableGroupGenomeType.ULong
                        );

                case SwitchableGroupGenomeType.BitArray:
                    return new SwitchableGroupGenomeImpl
                        (
                            guid: rando.NextGuid(),
                            keyCount: keyCount,
                            chromosome: rando.ToUintEnumerator(2)
                                                .Take(groupSize * keyCount)
                                                .ToChromosomeBits(rando.NextGuid(), keyCount),
                            switchableGroupGenomeType: SwitchableGroupGenomeType.BitArray
                        );

                case SwitchableGroupGenomeType.IntArray:

                    return new SwitchableGroupGenomeImpl
                        (
                            guid: rando.NextGuid(),
                            keyCount: keyCount,
                            chromosome: Enumerable.Range(0, groupSize)
                            .SelectMany(i => Enumerable.Range(0, keyCount)
                                                       .Select(t=>(uint)t)
                                                       .ToList()
                                                       .FisherYatesShuffle(rando)
                                        )
                                         .ToChromosomePermutation(rando.NextGuid(), keyCount, 0.2),

                            switchableGroupGenomeType: SwitchableGroupGenomeType.IntArray
                        );

                default:
                    throw new Exception();
            }
        }

        public static ISwitchableGroupGenome ToSwitchableGroupGenome(ISwitchableGroup switchableGroup)
        {
            if (switchableGroup.SwitchableDataType == typeof(uint))
            {
                var symbolCount = 2 ^ (uint)switchableGroup.KeyCount;
                return new SwitchableGroupGenomeImpl
                    (
                        guid: switchableGroup.Guid,
                        keyCount: switchableGroup.KeyCount,
                        chromosome: ((ISwitchableGroup<uint>)switchableGroup).Switchables
                                            .Select(t => t.Item)
                                            .ToChromosomeUintN(Guid.NewGuid(), symbolCount),
                        switchableGroupGenomeType: SwitchableGroupGenomeType.UInt
                    );
            }

            if (switchableGroup.SwitchableDataType == typeof(ulong))
            {
                var symbolCount = 2 ^ (ulong) switchableGroup.KeyCount;
                return new SwitchableGroupGenomeImpl
                    (
                        guid: switchableGroup.Guid,
                        keyCount: switchableGroup.KeyCount,
                        chromosome: ((ISwitchableGroup<uint>)switchableGroup).Switchables
                                            .Select(t => t.Item)
                                            .ToChromosomeUlongN(Guid.NewGuid(), symbolCount),
                        switchableGroupGenomeType: SwitchableGroupGenomeType.ULong
                    );
            }

            if (switchableGroup.SwitchableDataType == typeof(int[]))
            {
                var symbolCount = switchableGroup.KeyCount;
                var mixingRate = 0.3;
                return new SwitchableGroupGenomeImpl
                    (
                        guid: switchableGroup.Guid,
                        keyCount: switchableGroup.KeyCount,
                        chromosome: ((ISwitchableGroup<int[]>)switchableGroup).Switchables
                                            .SelectMany(t => t.Item.Cast<uint>())
                                            .ToChromosomePermutation(Guid.NewGuid(), symbolCount, mixingRate),
                        switchableGroupGenomeType: SwitchableGroupGenomeType.IntArray
                    );
            }

            if (switchableGroup.SwitchableDataType == typeof(bool[]))
            {
                const int symbolCount = 2;
                return new SwitchableGroupGenomeImpl
                    (
                        guid: switchableGroup.Guid,
                        keyCount: switchableGroup.KeyCount,
                        chromosome: ((ISwitchableGroup<bool[]>)switchableGroup).Switchables
                                            .SelectMany(t => t.Item.Cast<uint>())
                                            .ToChromosomeBits(Guid.NewGuid(), symbolCount),
                        switchableGroupGenomeType: SwitchableGroupGenomeType.BitArray
                    );
            }

            throw new Exception(string.Format("data type {0} not  handled", switchableGroup.SwitchableDataType.Name));
        }
    }

    class SwitchableGroupGenomeImpl : SimpleGenomeImpl<IChromosome>, ISwitchableGroupGenome
    {
        public SwitchableGroupGenomeImpl
            (
                Guid guid,
                int keyCount,
                IChromosome chromosome,
                SwitchableGroupGenomeType switchableGroupGenomeType
            )
            : base
            (
                guid: guid,
                chromosome: chromosome,
                parentGuid: Guid.Empty
            )
        {
            _keyCount = keyCount;
            _switchableGroupGenomeType = switchableGroupGenomeType;
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
