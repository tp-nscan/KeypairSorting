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
                                 .Select(i => createFunc(randy.NextInt()))
                );
        }

        public static ILayer<TG> Multiply<TG>
            (
                this ILayer<TG> layer,
                Func<TG, int, TG> genomeCopyFunc,
                int newGenomeCount,
                int seed
            ) where TG : IGenome
        {
            var randy = Rando.Fast(seed);
            return Make(
                    generation: layer.Generation,
                    genomes: layer.Genomes.Concat
                    (
                        layer.Genomes.Repeat().Take(newGenomeCount - layer.Genomes.Count)
                                .Select(g => genomeCopyFunc(g, randy.NextInt()))
                    )
                );
        }

        public static ILayer<TG> NextGen<TG>(
                                              this ILayer<TG> layer,
                                              int seed,
                                              IReadOnlyList<Tuple<Guid, double>> scores,
                                              int newGenomeCount
                                            ) where TG : IGenome
        {
            return Make(
                    generation: layer.Generation + 1,
                    genomes: scores.OrderByDescending(t => t.Item2)
                                   .Take(newGenomeCount)
                                   .Select(p => layer.GetGenome(p.Item1))
                                   .ToList()
                );
        }

        public static ILayer<TG> Make<TG>(
                                           int generation,
                                           IEnumerable<TG> genomes
                                         ) where TG : IGenome
        {
            return new LayerImpl<TG>
                (
                    generation,
                    genomes
                );
        }

    }

    public class LayerImpl<TG> : ILayer<TG> where TG : IGenome
    {
        private readonly int _generation;
        private readonly IReadOnlyDictionary<Guid, TG> _genomes;

        public LayerImpl(int generation, IEnumerable<TG> genomes)
        {
            _generation = generation;
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
    }
}
