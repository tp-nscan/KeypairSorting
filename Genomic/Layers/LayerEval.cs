using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;

namespace Genomic.Layers
{
    public interface ILayerEval<TG, TE> where TE : IGenomeEval<TG> 
                                        where TG : IGenome
    {
        IReadOnlyList<TE> GenomeEvals { get; }
        TE GetGenomeEval(Guid genomeId);
    }

    public static class LayerEval
    {
        public static ILayerEval<TG, TE> Make<TG, TE>(this IEnumerable<TE> genomeEvals)
            where TE : IGenomeEval<TG>
            where TG : IGenome
        {
            return new LayerEvalImpl<TG, TE>(genomeEvals: genomeEvals);
        }
    }

    class LayerEvalImpl<TG, TE> : ILayerEval<TG, TE>
                                where TE : IGenomeEval<TG>
                                where TG : IGenome
    {
        private readonly IReadOnlyDictionary<Guid, TE> _genomeEvals;

        public LayerEvalImpl(IEnumerable<TE> genomeEvals)
        {
            _genomeEvals = genomeEvals.ToDictionary(t => t.Genome.Guid);
        }

        private List<TE> _genomeEvalsList;
        public IReadOnlyList<TE> GenomeEvals
        {
            get { return _genomeEvalsList ?? (_genomeEvalsList = new List<TE>(_genomeEvals.Values)); }
        }

        public TE GetGenomeEval(Guid genomeId)
        {
            return _genomeEvals.ContainsKey(genomeId) ? _genomeEvals[genomeId] : default(TE);
        }
    }
}
