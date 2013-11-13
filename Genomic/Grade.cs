using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic
{
    public interface IGrade<out TG> where TG : IGenome
    {
        int Generation { get; }
        IReadOnlyList<TG> Genomes { get; }
        TG GetGenome(Guid genomeId);
        int Seed { get; }
    }

    public static class Grade
    {
        public static IGrade<TG> Create<TG>
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

        public static IGrade<TG> Update<TG>
            (
                IGrade<TG> grade,
                IReadOnlyList<Tuple<Guid, double>> scores,
                int selectionRatio,
                Func<TG, int, TG> genomeCopyFunc 
            ) where TG : IGenome
        {
            var randy = Rando.Fast(grade.Seed);

            var winners = scores.OrderByDescending(t => t.Item2)
                .Take(scores.Count / selectionRatio)
                .Select(p => grade.GetGenome(p.Item1))
                .ToList();

            return Make
                (
                    generation: grade.Generation + 1,
                    genomes: winners.Concat
                    (
                        winners.Repeat().Take(grade.Genomes.Count - winners.Count)
                                .Select(g => genomeCopyFunc(g, randy.NextInt()))
                    ),
                    seed: Rando.Fast(grade.Seed ^ grade.Seed).NextInt()
                );
        }

        public static IGrade<TG> Make<TG>
        (
            int generation,
            IEnumerable<TG> genomes,
            int seed
        )
        where TG : IGenome
        {
            return new GradeImpl<TG>
                (
                    generation,
                    seed,
                    genomes
                );
        }

    }

    class GradeImpl<TG> : IGrade<TG> where TG : IGenome
    {
        private readonly int _generation;
        private readonly IReadOnlyDictionary<Guid, TG> _genomes;
        private readonly int _seed;

        public GradeImpl(int generation, int seed, IEnumerable<TG> genomes)
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
            return _genomes[genomeId];
        }

        public int Seed
        {
            get { return _seed; }
        }
    }
}
