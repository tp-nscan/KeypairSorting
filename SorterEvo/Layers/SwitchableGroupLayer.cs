using System;
using System.Collections.Generic;
using Genomic.Chromosomes;
using Genomic.Layers;
using MathUtils.Rand;
using SorterEvo.Genomes;

namespace SorterEvo.Layers
{
    public static class SwitchableGroupLayer
    {
        public static ILayer<ISwitchableGroupGenome> Create
            (
            int seed,
            SwitchableGroupGenomeType switchableGroupGenomeType,
            int genomeCount,
            int keyCount,
            int keyPairCount
            )
        {
            return Layer.Create
                (
                    seed: seed,
                    createFunc: CreateFunc(switchableGroupGenomeType, keyCount, keyPairCount),
                    genomeCount: genomeCount
                );
        }

        public static ILayer<ISwitchableGroupGenome> Update
        (
            this ILayer<ISwitchableGroupGenome> switchableGroupGenome,
            IReadOnlyList<Tuple<Guid, double>> scores,
            int selectionRatio,
            double mutationRate,
            double insertionRate,
            double deletionRate
        )
        {
            return Layer.Update(
                    switchableGroupGenome,
                    scores,
                    selectionRatio,
                    UpdateFunc
                        (
                            mutationRate: mutationRate,
                            insertionRate: insertionRate,
                            deletionRate: deletionRate
                        )
                );
        }


        public static Func<int, ISwitchableGroupGenome> CreateFunc
        (
            SwitchableGroupGenomeType switchableGroupGenomeType,
            int keyCount, 
            int keyPairCount
        )
        {
            return i => Rando.Fast(i).ToSwitchableGroupGenome
                (
                    switchableGroupGenomeType: switchableGroupGenomeType,
                    keyCount: keyCount,
                    groupSize: keyPairCount
                );
        }

        public static Func<ISwitchableGroupGenome, int, ISwitchableGroupGenome> UpdateFunc
            (
                double mutationRate,
                double insertionRate,
                double deletionRate
            )
        {
             return (sg, i) =>
            {
                var randy = Rando.Fast(i);
                return SwitchableGroupGenome.Make
                    (
                        guid: randy.NextGuid(),
                        parentGuid: sg.Guid,
                        keyCount: sg.KeyCount,
                        chromosome: sg.CopyChromosome
                                        (
                                            randy:randy,
                                            mutationRate: mutationRate,
                                            insertionRate: insertionRate,
                                            deletionRate: deletionRate
                                        ),
                        switchableGroupGenomeType: sg.SwitchableGroupGenomeType,
                        groupCount: sg.GroupCount
                    );
            };
        }
    }
}
