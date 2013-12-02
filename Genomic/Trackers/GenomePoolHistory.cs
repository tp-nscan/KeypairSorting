using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Genomic.Genomes;

namespace Genomic.Trackers
{
    public interface IGenomePoolHistory<TG> where TG : IGenome
    {
        void AddGenomeEvos(IEnumerable<IGenomeTracker<TG>> genomeEvos);
        IReadOnlyList<IGenomeEval<TG>> AddGenomeLayerResults(IEnumerable<IGenomeEval<TG>> genomeLayerResults);
        IEnumerable<IGenomeTracker<TG>> GenomeEvos { get; } 
    }

    public static class GenomePoolHistory
    {
        public static IGenomePoolHistory<TG> Make<TG>() where TG : IGenome
        {
            return new GenomePoolHistoryImpl<TG>();
        }
    }

    class GenomePoolHistoryImpl<TG> : IGenomePoolHistory<TG> where TG : IGenome
    {
        public GenomePoolHistoryImpl()
        {
        }

        public void AddGenomeEvos(IEnumerable<IGenomeTracker<TG>> genomeEvos)
        {
            foreach (var genomeEvo in genomeEvos)
            {
                _genomeEvos = _genomeEvos.Add(genomeEvo.Genome.Guid, genomeEvo);
            }
        }

        public IReadOnlyList<IGenomeEval<TG>> AddGenomeLayerResults(IEnumerable<IGenomeEval<TG>> genomeLayerResults)
        {
            var listRet = new List<IGenomeEval<TG>>();

            foreach (var genomeLayerResult in genomeLayerResults)
            {
                if (_genomeEvos.ContainsKey(genomeLayerResult.Genome.Guid))
                {
                    _genomeEvos[genomeLayerResult.Genome.Guid].AddGenomeEval(genomeLayerResult);
                }
                else
                {
                    listRet.Add(genomeLayerResult);
                }
            }

            return listRet;
        }

        private IImmutableDictionary<Guid, IGenomeTracker<TG>> _genomeEvos 
                        = ImmutableDictionary.Create<Guid, IGenomeTracker<TG>>();

        public IEnumerable<IGenomeTracker<TG>> GenomeEvos
        {
            get { return _genomeEvos.Values; }
        }
    }
}
