using System.Collections.Generic;
using System.Collections.Immutable;
using Genomic.Genomes;

namespace Genomic.Trackers
{

    public interface IGenomeTracker<TG> where TG : IGenome
    {
        void AddGenomeEval(IGenomeEval<TG> genomeEval);
        TG Genome { get; }
        double ReferenceScore { get; }
        IEnumerable<IGenomeEval<TG>> GenomeEvals { get; }
    }

    public static class GenomeTracker
    {
        public static IGenomeTracker<TG> Make<TG>(TG genome, double referenceScore) 
            where TG : IGenome
        {
            return new GenomeTrackerImpl<TG>(genome, referenceScore);
        }
    }

    class GenomeTrackerImpl<TG> : IGenomeTracker<TG> where TG : IGenome
    {
        public GenomeTrackerImpl(TG genome, double referenceScore)
        {
            _genome = genome;
            _referenceScore = referenceScore;
        }

        public void AddGenomeEval(IGenomeEval<TG> genomeEval)
        {
            _genomeEvals = _genomeEvals.Add(genomeEval);
        }

        private readonly TG _genome;
        public TG Genome
        {
            get { return _genome; }
        }

        private readonly double _referenceScore;
        public double ReferenceScore
        {
            get { return _referenceScore; }
        }

        private IImmutableList<IGenomeEval<TG>> _genomeEvals
                = ImmutableList.Create<IGenomeEval<TG>>();
        public IEnumerable<IGenomeEval<TG>> GenomeEvals
        {
            get { return _genomeEvals; }
        }
    }

}