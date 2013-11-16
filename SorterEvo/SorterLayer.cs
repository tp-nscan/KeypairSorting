using System;
using System.Collections.Generic;
using Genomic;
using MathUtils.Rand;

namespace SorterEvo
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
                    createFunc: CreateFunc(keyCount, keyPairCount),
                    genomeCount: genomeCount
                );
        }

        public static ILayer<ISorterGenome> Update
        (
            this ILayer<ISorterGenome> sorterLayer,
            IReadOnlyList<Tuple<Guid, double>> scores,
            int selectionRatio,
            double mutationRate,
            double insertionRate,
            double deletionRate
        )
            {
                return Layer.Update
                    (
                        sorterLayer,
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


        public static Func<int, ISorterGenome> CreateFunc(int keyCount, int keyPairCount)
        {
            return i => Rando.Fast(i).ToSorterGenome(keyCount, keyPairCount);
        }

        public static Func<ISorterGenome, int, ISorterGenome> UpdateFunc
            (
                double mutationRate,
                double insertionRate,
                double deletionRate
            )
        {
            return (sg, i) =>
            {
                var randy = Rando.Fast(i);
                return SorterGenome.Make
                    (
                        guid: randy.NextGuid(),
                        parentGuid: sg.Guid,
                        chromosome: sg.Chromosome.Copy(randy, mutationRate, insertionRate, deletionRate),
                        keyCount: sg.KeyCount,
                        keyPairCount: sg.KeyPairCount
                    );
            };
        }
    }
}
