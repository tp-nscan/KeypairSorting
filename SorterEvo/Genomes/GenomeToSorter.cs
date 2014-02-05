using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;
using Sorting.KeyPairs;
using Sorting.Sorters;
using Sorting.Sorters.StageGenerators;
using Sorting.Switchables;

namespace SorterEvo.Genomes
{
    public static class GenomeToSorter
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

        public static ISorter ToSorter2(this ISorterGenome sorterGenome)
        {
            IList<IReadOnlyList<IKeyPair>> keyPairGroups = sorterGenome.Chromosome.Sequence
                .Select(i => KeyPairRepository.AtIndex((int)i))
                .Slice(18)
                .ToList();

            var sorter = StagedSorter.Make
                (
                    guid: sorterGenome.Guid,
                    keyCount: sorterGenome.KeyCount,
                    sorterStages: Enumerable.Range(0, 77)
                                    .Select(
                                    i => keyPairGroups[i].ToReducedSorterStage(SwitchableGroups[i])
                                    ).ToList()
                );

            return sorter;
        }


        private const int SwitchableGroupSize = 40;

        private static List<ISwitchableGroup<uint>> _switchableGroups;
        public static IReadOnlyList<ISwitchableGroup<uint>> SwitchableGroups
        {
            get
            {
                return _switchableGroups ?? (
                    _switchableGroups = Enumerable.Range(0, 80)
                        .Select(i => Rando.Fast(109 + i)
                            .ToSwitchableGroup<uint>(Guid.NewGuid(), 13, SwitchableGroupSize))
                        .ToList()
                );
                  
            }
        }

    }
}
