using System;
using System.Collections.Generic;
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

        public static ILayer<ISwitchableGroupGenome> Multiply
            (
                this ILayer<ISwitchableGroupGenome> switchableGroupGenomeLayer,
                int seed,
                int newGenomeCount,
                double mutationRate,
                double insertionRate,
                double deletionRate
            )
        {
            return switchableGroupGenomeLayer.Multiply
                (
                    genomeCopyFunc: CopyFunc
                        (
                            mutationRate: mutationRate,
                            insertionRate: insertionRate,
                            deletionRate: deletionRate
                        ),
                    newGenomeCount: newGenomeCount,
                    seed: seed
                );
        }

        public static ILayer<ISwitchableGroupGenome> NextGen
        (
            this ILayer<ISwitchableGroupGenome> layer,
            int seed,
            IReadOnlyList<Tuple<Guid, double>> scores,
            int newGenomeCount
        )
        {
            return layer.NextGen
                (
                    seed : seed,
                    scores : scores,
                    newGenomeCount: newGenomeCount
                );
        }

        static Func<int, ISwitchableGroupGenome> CreateFunc
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

        public static Func<ISwitchableGroupGenome, int, ISwitchableGroupGenome> CopyFunc
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
                                            randy: randy,
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
