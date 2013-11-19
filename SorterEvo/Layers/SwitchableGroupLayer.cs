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
                int groupSize
            )
        {
            return Layer.Create
                (
                    seed: seed,
                    createFunc: CreateFunc(switchableGroupGenomeType, keyCount, groupSize),
                    genomeCount: genomeCount
                );
        }

        public static ILayer<ISwitchableGroupGenome> Update
        (
            this ILayer<ISwitchableGroupGenome> switchableGroupGenomeLayer,
            IReadOnlyList<Tuple<Guid, double>> scores,
            int selectionRatio,
            double mutationRate,
            double insertionRate,
            double deletionRate
        )
        {
            return Layer.Update(
                    switchableGroupGenomeLayer,
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
            int groupSize
        )
        {
            return i => Rando.Fast(i).ToSwitchableGroupGenome
                (
                    switchableGroupGenomeType: switchableGroupGenomeType,
                    keyCount: keyCount,
                    groupSize: groupSize
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
