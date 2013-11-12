using System;
using System.Collections.Generic;
using System.Linq;
using Evo.Genomes;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Evo.Grades
{
    public interface IGrade<TG, T>
        where TG : IGenome<T>
    {
        Func<TG, int, TG> CopyGenomeFunc { get; }
        Guid GradeId { get; }
        TG GetGenome(Guid genomeId);
        int Generation { get; }
        IReadOnlyList<TG> Genomes { get; }
        int Seed { get; }
    }

    public static class Grade
    {

        public static IGrade<TG, T> Create<TG, T>
        (
            Guid id,
            int seed,
            Func<int, TG> createFunc,
            Func<TG, int, TG> copyFunc,
            IReadOnlyList<Tuple<Guid, double>> scores,
            int genomeCount
        )
        where TG : IGenome<T>
        {
            var randy = Rando.Fast(seed);
            return Make<TG, T>
                (
                    gradeId: id,
                    generation: 0,
                    genomes: Enumerable.Range(0, genomeCount)
                                 .Select(i => createFunc(randy.NextInt())),
                    copyGenomeFunc: copyFunc,
                    seed: seed
                );
        }

        public static IGrade<TG, T> Update<TG, T>
        (
            this IGrade<TG, T> compPool,
            IReadOnlyList<Tuple<Guid, double>> scores,
            int selectionRatio
        )
            where TG : IGenome<T>
        {
            var randy = Rando.Fast(compPool.Seed);

            var winners = scores.OrderByDescending(t => t.Item2)
                .Take(scores.Count / selectionRatio)
                .Select(p => compPool.GetGenome(p.Item1))
                .ToList();

            return Make<TG, T>
                (
                    gradeId: randy.NextGuid(),
                    generation: compPool.Generation +1,
                    genomes: winners.Concat
                    (
                        winners.Repeat().Take(compPool.Genomes.Count - winners.Count)
                                .Select(g=>compPool.CopyGenomeFunc(g, randy.NextInt()))
                    ),  
                    copyGenomeFunc:compPool.CopyGenomeFunc,
                    seed: Rando.Fast(compPool.Seed ^ compPool.Seed).NextInt()
                );
        }


        public static IGrade<TG, T> Make<TG, T>
            (
                Guid gradeId,
                int generation,
                IEnumerable<TG> genomes,
                Func<TG, int, TG> copyGenomeFunc,
                int seed
            )
            where TG : IGenome<T>
        {
            return new GradeImpl<TG, T>
                (
                    gradeId,
                    generation,
                    genomes,
                    copyGenomeFunc,
                    seed
                );
        }
    }

    class GradeImpl<TG, T> : IGrade<TG, T>
        where TG : IGenome<T>
    {
        private readonly IReadOnlyDictionary<Guid, TG> _genomes;
        private readonly Guid _gradeId;
        private readonly int _generation;

        public GradeImpl
            (
                Guid gradeId,
                int generation,
                IEnumerable<TG> genomes, 
                Func<TG, int, TG> copyGenomeFunc, 
                int seed
            )
        {
            _gradeId = gradeId;
            _generation = generation;
            _copyGenomeFunc = copyGenomeFunc;
            _seed = seed;
            _genomes = genomes.ToDictionary(t => t.Guid);
        }

        private readonly Func<TG, int, TG> _copyGenomeFunc;
        public Func<TG, int, TG> CopyGenomeFunc
        {
            get { return _copyGenomeFunc; }
        }

        public Guid GradeId
        {
            get { return _gradeId; }
        }

        public TG GetGenome(Guid genomeId)
        {
            return _genomes[genomeId];
        }

        public int Generation
        {
            get { return _generation; }
        }

        private List<TG> _orgsList;
        public IReadOnlyList<TG> Genomes
        {
            get
            {
                return _orgsList ?? (_orgsList = new List<TG>(_genomes.Values));
            }
        }

        private readonly int _seed;
        public int Seed
        {
            get { return _seed; }
        }
    }
}
