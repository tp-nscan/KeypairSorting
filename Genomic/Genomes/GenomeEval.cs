namespace Genomic.Genomes
{
    public interface IGenomeEval<TG> where TG : IGenome
    {
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
                double score
            ) where TG : IGenome
        {
            return new GenomeEvalImpl<TG>
                (
                    genome: genome,
                    score:score
                );
        }
    }

    class GenomeEvalImpl<TG> : IGenomeEval<TG> where TG : IGenome
    {
        private readonly TG _genome;
        private readonly double _score;

        public GenomeEvalImpl(TG genome, double score)
        {
            _genome = genome;
            _score = score;
        }

        public TG Genome
        {
            get { return _genome; }
        }

        public double Score
        {
            get { return _score; }
        }
    }
}
