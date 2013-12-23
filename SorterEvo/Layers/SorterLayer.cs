using System;
using System.Collections.Generic;
using Genomic.Chromosomes;
using Genomic.Layers;
using MathUtils.Rand;
using SorterEvo.Genomes;

namespace SorterEvo.Layers
{
    public static class SorterLayer
    {
        public static ILayer<ISorterGenome> Create
        (
                int seed,
                int genomeCount,
                int keyCount,
                int keyPairCount
        )
        {
            return Layer.Create
                (
                    seed: seed,
                    createFunc: CreateFunc(keyCount: keyCount, keyPairCount: keyPairCount),
                    genomeCount: genomeCount
                );
        }

        public static ILayer<ISorterGenome> Multiply
            (
                this ILayer<ISorterGenome> sorterGenomeLayer,
                int seed,
                int newGenomeCount,
                double mutationRate,
                double insertionRate,
                double deletionRate
            )
        {
            return sorterGenomeLayer.Multiply
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

        public static ILayer<ISorterGenome> NextGen
        (
            this ILayer<ISorterGenome> layer,
            int seed,
            IReadOnlyList<Tuple<Guid, double>> scores,
            int newGenomeCount
        )
        {
            return Layer.NextGen
                (
                    layer: layer,
                    seed: seed,
                    scores: scores,
                    newGenomeCount: newGenomeCount
                );
        }

        public static Func<int, ISorterGenome> CreateFunc(int keyCount, int keyPairCount)
        {
            return i => Rando.Fast(i).ToSorterGenome(keyCount, keyPairCount);
        }

        public static Func<ISorterGenome, int, Guid, ISorterGenome> CopyFunc
            (
                double mutationRate,
                double insertionRate,
                double deletionRate
            )
        {
            return (sg, i, guid) =>
            {
                var randy = Rando.Fast(i);
                return SorterGenome.Make
                    (
                        guid: guid,
                        parentGuid: sg.Guid,
                        chromosome: sg.Chromosome.Copy(randy, mutationRate, insertionRate, deletionRate),
                        keyCount: sg.KeyCount,
                        keyPairCount: sg.KeyPairCount
                    );
            };
        }
    }

}
