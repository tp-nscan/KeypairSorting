using System.Collections.Generic;
using System.Collections.Immutable;
using Genomic.Genomes;

namespace Genomic.Layers
{

    public interface IGenomeEvo<TG> where TG : IGenome
    {
        void AddLayerResult(IGenomeLayerResult<TG> genomeLayerResult);
        TG Genome { get; }
        double ReferenceScore { get; }
        IEnumerable<IGenomeLayerResult<TG>> GenomeLayerResults { get; }
    }

    public static class GenomeEvo
    {
        public static IGenomeEvo<TG> Make<TG>(TG genome, double referenceScore) 
            where TG : IGenome
        {
            return new GenomeEvoImpl<TG>(genome, referenceScore);
        }
    }

    class GenomeEvoImpl<TG> : IGenomeEvo<TG> where TG : IGenome
    {
        public GenomeEvoImpl(TG genome, double referenceScore)
        {
            _genome = genome;
            _referenceScore = referenceScore;
        }

        public void AddLayerResult(IGenomeLayerResult<TG> genomeLayerResult)
        {
            _genomeLayerResults = _genomeLayerResults.Add(genomeLayerResult);
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

        private IImmutableList<IGenomeLayerResult<TG>> _genomeLayerResults
                = ImmutableList.Create<IGenomeLayerResult<TG>>();
        public IEnumerable<IGenomeLayerResult<TG>> GenomeLayerResults
        {
            get { return _genomeLayerResults; }
        }
    }

}