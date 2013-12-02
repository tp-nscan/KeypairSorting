using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;

namespace Genomic.Layers
{
    public interface ILayerEval<TG, TE> where TE : IGenomeEval<TG> 
                                        where TG : IGenome
    {
        int Generation { get; }
        IReadOnlyList<TE> GenomeEvals { get; }
        TE GetGenomeEval(Guid genomeId);
        int Seed { get; }
    }

    public static class LayerEval
    {

    }

    class LayerEvalImpl<TG, TE> : ILayerEval<TG, TE>
                                where TE : IGenomeEval<TG>
                                where TG : IGenome
    {
        private readonly int _generation;
        private readonly IReadOnlyDictionary<Guid, TE> _genomes;
        private readonly int _seed;

        public LayerEvalImpl(int generation, int seed, IEnumerable<TE> genomes)
        {
            _generation = generation;
            _seed = seed;
            _genomes = genomes.ToDictionary(t => t.Genome.Guid);
        }

        public int Generation
        {
            get { return _generation; }
        }

        private List<TE> _genomeEvalsList;
        public IReadOnlyList<TE> GenomeEvals
        {
            get { return _genomeEvalsList ?? (_genomeEvalsList = new List<TE>(_genomes.Values)); }
        }

        public TE GetGenomeEval(Guid genomeId)
        {
            return _genomes.ContainsKey(genomeId) ? _genomes[genomeId] : default(TE);
        }

        public int Seed
        {
            get { return _seed; }
        }
    }
}
