using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;

namespace Genomic.GenomePools
{
    public interface IGenomePoolEvals<T>
        where T : IGuid
    {
        IReadOnlyDictionary<Guid, IEvaluation<T>> Evaluations { get; }
    }

    public static class GenomePoolEvals
    {
        public static IGenomePoolEvals<T> ToCompPool<T>(this IEnumerable<T> items, Func<T, double> evaluator)
            where T : IGuid
        {
            return new GenomePoolEvalsImpl<T>(items.Select(i => i.Make(evaluator(i))));
        }
    }

    class GenomePoolEvalsImpl<T> : IGenomePoolEvals<T>
        where T : IGuid
    {
        public GenomePoolEvalsImpl(IEnumerable<IEvaluation<T>> evaluations)
        {
            _evaluations = evaluations.ToDictionary(t => t.Item.Guid);
        }

        private readonly IReadOnlyDictionary<Guid, IEvaluation<T>> _evaluations;
        public IReadOnlyDictionary<Guid, IEvaluation<T>> Evaluations
        {
            get { return _evaluations; }
        }
    }
}
