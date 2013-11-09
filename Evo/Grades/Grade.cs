using System;
using System.Collections.Generic;
using System.Linq;
using Evo.Genomes;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Evo.Grades
{
    public interface IGrade<TG>
        where TG : IGenome
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

        public static IGrade<TG> Create<TG>
        (
            Guid id,
            int seed,
            Func<int, TG> createFunc,
            Func<TG, int, TG> copyFunc,
            IReadOnlyList<Tuple<Guid, double>> scores,
            int genomeCount
        )
        where TG : IGenome
        {
            var randy = Rando.Fast(seed);
            return Make
                (
                    gradeId: id,
                    generation: 0,
                    genomes: Enumerable.Range(0, genomeCount)
                                 .Select(i => createFunc(randy.NextInt())),
                    copyGenomeFunc: copyFunc,
                    seed: seed
                );
        }

        public static IGrade<TG> Update<TG>
        (
            this IGrade<TG> compPool,
            IReadOnlyList<Tuple<Guid, double>> scores,
            int selectionRatio
        )
            where TG : IGenome
        {
            var randy = Rando.Fast(compPool.Seed);

            var winners = scores.OrderByDescending(t => t.Item2)
                .Take(scores.Count / selectionRatio)
                .Select(p => compPool.GetGenome(p.Item1))
                .ToList();

            return Make
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


        public static IGrade<TG> Make<TG>
            (
                Guid gradeId,
                int generation,
                IEnumerable<TG> genomes,
                Func<TG, int, TG> copyGenomeFunc,
                int seed
            )
            where TG : IGenome
        {
            return new GradeImpl<TG>
                (
                    gradeId,
                    generation,
                    genomes,
                    copyGenomeFunc,
                    seed
                );
        }
    }

    class GradeImpl<TG> : IGrade<TG>
        where TG : IGenome
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
