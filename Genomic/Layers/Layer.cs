using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic.Layers
{
    public interface ILayer<out TG> where TG : IGenome
    {
        int Generation { get; }
        IReadOnlyList<TG> Genomes { get; }
        TG GetGenome(Guid genomeId);
        int Seed { get; }
    }

    public static class Layer
    {
        public static ILayer<TG> Create<TG>
            (
                int seed,
                Func<int, TG> createFunc,
                int genomeCount
            ) where TG : IGenome
        {
            var randy = Rando.Fast(seed);
            return Make
                (
                    generation: 0,
                    genomes: Enumerable.Range(0, genomeCount)
                                 .Select(i => createFunc(randy.NextInt())),
                    seed: seed
                );
        }

        public static ILayer<TG> Update<TG>
            (
                ILayer<TG> layer,
                IReadOnlyList<Tuple<Guid, double>> scores,
                int selectionRatio,
                Func<TG, int, TG> genomeCopyFunc 
            ) where TG : IGenome
        {
            var newSeed = Rando.Fast(layer.Seed *397).NextInt();
            var randy = Rando.Fast(newSeed);

            var winners = scores.OrderByDescending(t => t.Item2)
                .Take(scores.Count / selectionRatio)
                .Select(p => layer.GetGenome(p.Item1))
                .ToList();

            return Make
                (
                    generation: layer.Generation + 1,
                    genomes: winners.Concat
                    (
                        winners.Repeat().Take(layer.Genomes.Count - winners.Count)
                                .Select(g => genomeCopyFunc(g, randy.NextInt()))
                    ),
                    seed: newSeed
                );
        }

        public static ILayer<TG> Make<TG>
        (
            int generation,
            IEnumerable<TG> genomes,
            int seed
        )
        where TG : IGenome
        {
            return new LayerImpl<TG>
                (
                    generation,
                    seed,
                    genomes
                );
        }

    }

    class LayerImpl<TG> : ILayer<TG> where TG : IGenome
    {
        private readonly int _generation;
        private readonly IReadOnlyDictionary<Guid, TG> _genomes;
        private readonly int _seed;

        public LayerImpl(int generation, int seed, IEnumerable<TG> genomes)
        {
            _generation = generation;
            _seed = seed;
            _genomes = genomes.ToDictionary(t => t.Guid);
        }

        public int Generation
        {
            get { return _generation; }
        }

        private List<TG> _genomesList; 
        public IReadOnlyList<TG> Genomes
        {
            get { return _genomesList ?? (_genomesList = new List<TG>(_genomes.Values)); }
        }

        public TG GetGenome(Guid genomeId)
        {
            return _genomes.ContainsKey(genomeId) ? _genomes[genomeId] : default(TG);
        }

        public int Seed
        {
            get { return _seed; }
        }
    }
}
