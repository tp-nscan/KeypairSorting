using System;
using MathUtils.Collections;

namespace Genomic.Genomes
{
    public interface IGenomeEval<TG> : IGuid
                                       where TG : IGenome
    {
        int Generation { get; }
        TG Genome { get; }
        /// <summary>
        /// Low scores are better
        /// </summary>
        double Score { get; }
    }

    public static class GenomeEval
    {
        public static IGenomeEval<TG> Make<TG>
        (
            TG genome,
            double score, 
            int generation
        ) where TG : IGenome
        {
            return new GenomeEvalImpl<TG>
                (
                    genome: genome,
                    score: score,
                    generation: generation
                );
        }
    }

    class GenomeEvalImpl<TG> : IGenomeEval<TG> where TG : IGenome
    {
        private readonly TG _genome;
        private readonly double _score;

        public GenomeEvalImpl(TG genome, double score, int generation)
        {
            _genome = genome;
            _score = score;
            _generation = generation;
        }

        private readonly int _generation;
        public int Generation
        {
            get { return _generation; }
        }

        public TG Genome
        {
            get { return _genome; }
        }

        public double Score
        {
            get { return _score; }
        }

        public Guid Guid
        {
            get { return _genome.Guid; }
        }
    }
}
