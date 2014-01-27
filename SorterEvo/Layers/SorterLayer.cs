using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Chromosomes;
using Genomic.Genes;
using Genomic.Layers;
using MathUtils.Collections;
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

        public static ILayer<ISorterGenome> Make
        (
            IEnumerable<ISorterGenome> sorterGenomes,
            int generation
        )
        {
            return Layer.Make
                (
                    generation: generation,
                    genomes: sorterGenomes
                );
        }

        public static ILayer<ISorterGenome> ReproducePreserveParents
            (
                this ILayer<ISorterGenome> sorterGenomeLayer,
                int seed,
                int newGenomeCount,
                double mutationRate,
                double insertionRate,
                double deletionRate
            )
        {
            return sorterGenomeLayer.MultiplyPreserveParents
                (
                    genomeReproFunc: SorterPropigator
                        (
                            mutationRate: mutationRate,
                            insertionRate: insertionRate,
                            deletionRate: deletionRate
                        ),
                    newGenomeCount: newGenomeCount,
                    seed: seed
                );
        }

        public static ILayer<ISorterGenome> Reproduce
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
                    genomeReproFunc: SorterPropigator
                        (
                            mutationRate: mutationRate,
                            insertionRate: insertionRate,
                            deletionRate: deletionRate
                        ),
                    newGenomeCount: newGenomeCount,
                    seed: seed
                );
        }

        public static ILayer<ISorterGenome> Recombinate
        (
            this ILayer<ISorterGenome> sorterGenomeLayer,
            double recombinationRate,
            int seed
        )
        {
            var rando = Rando.Fast(seed);

            var recombinedGenomes = sorterGenomeLayer.Genomes
                .ToList()
                .PairRandomly(rando.Spawn())
                .Select(p=>p.Recombine(rando.Spawn(), recombinationRate))
                .SelectMany(rp=> new[] {rp.Item1, rp.Item2})
                .ToList();


            return Make
            (
                recombinedGenomes,
                sorterGenomeLayer.Generation
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

        public static Func<ISorterGenome, int, Guid, ISorterGenome> SorterPropigator
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
                        chromosome: (IChromosomeUint) sg.Chromosome
                            .StandardPropigate(randy, mutationRate, insertionRate, deletionRate),
                        keyCount: sg.KeyCount,
                        keyPairCount: sg.KeyPairCount
                    );
            };
        }
    }

}
