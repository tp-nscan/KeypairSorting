namespace Genomic.Genomes
{
    public interface IGenomeEval<TG> where TG : IGenome
    {
        TG Genome { get; }
        int Generation { get; }
        double Score { get; }
    }

    public static class GenomeEval
    {
        public static IGenomeEval<TG> Make<TG>
            (
                TG genome, 
                int generation, 
                double score
            ) where TG : IGenome
        {
            return new GenomeEvalImpl<TG>
                (
                    genome: genome, 
                    generation: generation, 
                    score:score
                );
        }
    }

    class GenomeEvalImpl<TG> : IGenomeEval<TG> where TG : IGenome
    {
        private readonly TG _genome;
        private readonly int _generation;
        private readonly double _score;

        public GenomeEvalImpl(TG genome, int generation, double score)
        {
            _genome = genome;
            _generation = generation;
            _score = score;
        }

        public TG Genome
        {
            get { return _genome; }
        }

        public int Generation
        {
            get { return _generation; }
        }

        public double Score
        {
            get { return _score; }
        }
    }
}
