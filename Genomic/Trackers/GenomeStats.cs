using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Genomic.Genomes;
using MathUtils.Collections;

namespace Genomic.Trackers
{

    public interface IGenomeStats<TG> : IGuid
                                        where TG : IGenome
    {
        void AddGenomeEval(IGenomeEval<TG> genomeEval);
        int FirstGeneration { get; }
        TG Genome { get; }
        object ReferenceResult { get; }
        IEnumerable<IGenomeEval<TG>> GenomeEvals { get; }
    }

    public static class GenomeStats
    {
        public static IGenomeStats<TG> Make<TG>(TG genome, object referenceResult, int firstGeneration) 
            where TG : IGenome
        {
            return new GenomeStatsImpl<TG>(genome, referenceResult, firstGeneration);
        }
    }

    class GenomeStatsImpl<TG> : IGenomeStats<TG> where TG : IGenome
    {
        public GenomeStatsImpl(TG genome, object referenceResult, int firstGeneration)
        {
            _genome = genome;
            _referenceResult = referenceResult;
            _firstGeneration = firstGeneration;
        }

        public void AddGenomeEval(IGenomeEval<TG> genomeEval)
        {
            _genomeEvals = _genomeEvals.Add(genomeEval);
        }

        private readonly int _firstGeneration;
        public int FirstGeneration
        {
            get { return _firstGeneration; }
        }

        private readonly TG _genome;
        public TG Genome
        {
            get { return _genome; }
        }

        private readonly object _referenceResult;
        public object ReferenceResult
        {
            get { return _referenceResult; }
        }

        private IImmutableList<IGenomeEval<TG>> _genomeEvals
                = ImmutableList.Create<IGenomeEval<TG>>();

        public IEnumerable<IGenomeEval<TG>> GenomeEvals
        {
            get { return _genomeEvals; }
        }

        public Guid Guid
        {
            get { return Genome.Guid; }
        }
    }
}