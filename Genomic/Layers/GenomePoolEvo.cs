using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Genomic.Genomes;

namespace Genomic.Layers
{
    public interface IGenomePoolEvo<TG> where TG : IGenome
    {
        void AddGenomeEvos(IEnumerable<IGenomeEvo<TG>> genomeEvos);
        IReadOnlyList<IGenomeLayerResult<TG>> AddGenomeLayerResults(IEnumerable<IGenomeLayerResult<TG>> genomeLayerResults);
        IEnumerable<IGenomeEvo<TG>> GenomeEvos { get; } 
    }

    public static class GenomePoolEvo
    {
        public static IGenomePoolEvo<TG> Make<TG>() where TG : IGenome
        {
            return new GenomePoolEvoImpl<TG>();
        }
    }

    class GenomePoolEvoImpl<TG> : IGenomePoolEvo<TG> where TG : IGenome
    {
        public GenomePoolEvoImpl()
        {
        }

        public void AddGenomeEvos(IEnumerable<IGenomeEvo<TG>> genomeEvos)
        {
            foreach (var genomeEvo in genomeEvos)
            {
                _genomeEvos = _genomeEvos.Add(genomeEvo.Genome.Guid, genomeEvo);
            }
        }

        public IReadOnlyList<IGenomeLayerResult<TG>> AddGenomeLayerResults(IEnumerable<IGenomeLayerResult<TG>> genomeLayerResults)
        {
            var listRet = new List<IGenomeLayerResult<TG>>();

            foreach (var genomeLayerResult in genomeLayerResults)
            {
                if (_genomeEvos.ContainsKey(genomeLayerResult.Genome.Guid))
                {
                    _genomeEvos[genomeLayerResult.Genome.Guid].AddLayerResult(genomeLayerResult);
                }
                else
                {
                    listRet.Add(genomeLayerResult);
                }
            }

            return listRet;
        }

        private IImmutableDictionary<Guid, IGenomeEvo<TG>> _genomeEvos 
                        = ImmutableDictionary.Create<Guid, IGenomeEvo<TG>>();

        public IEnumerable<IGenomeEvo<TG>> GenomeEvos
        {
            get { return _genomeEvos.Values; }
        }
    }
}
