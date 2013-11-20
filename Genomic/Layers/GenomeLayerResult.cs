using Genomic.Genomes;

namespace Genomic.Layers
{
    public interface IGenomeLayerResult<TG> where TG : IGenome
    {
        TG Genome { get; }
        int Generation { get; }
        double Score { get; }
    }

    public static class GenomeLayerResult
    {
        public static IGenomeLayerResult<TG> Make<TG>(
            TG genome, int generation, double score) where TG : IGenome
        {
            return new GenomeLayerResultImpl<TG>
                (
                    genome: genome, 
                    generation: generation, 
                    score:score
                );
        }
    }

    class GenomeLayerResultImpl<TG> : IGenomeLayerResult<TG> where TG : IGenome
    {
        private readonly TG _genome;
        private readonly int _generation;
        private readonly double _score;

        public GenomeLayerResultImpl(TG genome, int generation, double score)
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
