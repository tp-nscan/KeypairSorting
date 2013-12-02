using System;
using System.Collections.Generic;
using Genomic.Chromosomes;
using Genomic.Layers;
using MathUtils.Rand;
using SorterEvo.Genomes;

namespace SorterEvo.Layers
{
    public interface ISorterLayer : ILayer<ISorterGenome>
    {
        
    }

    public static class SorterLayer
    {
        public static ISorterLayer Create
            (
                int seed,
                int genomeCount,
                int keyCount,
                int keyPairCount
            )
        {
            return (ISorterLayer) Layer.Create
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
                        CopyFunc
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

        public static Func<ISorterGenome, int, ISorterGenome> CopyFunc
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

    class SorterLayerImpl : LayerImpl<ISorterGenome>, ISorterLayer
    {
        public SorterLayerImpl(
            int generation, 
            int seed, 
            IEnumerable<ISorterGenome> genomes) 
            : base(generation, seed, genomes)
        {
        }
    }


}
