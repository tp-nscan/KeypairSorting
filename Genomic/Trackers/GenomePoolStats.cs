using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Genomic.Genomes;

namespace Genomic.Trackers
{
    public interface IGenomePoolStats<TG> where TG : IGenome
    {
        void AddGenomeStatses(IEnumerable<IGenomeStats<TG>> genomeStatses);
        void AddGenomeEvals(IEnumerable<IGenomeEval<TG>> genomeEvals, Func<TG, object> refrenceFunc);
        IEnumerable<IGenomeStats<TG>> GenomeStatses { get; } 
    }

    public static class GenomePoolStats
    {
        public static IGenomePoolStats<TG> Make<TG>() where TG : IGenome
        {
            return new GenomePoolStatsImpl<TG>(Enumerable.Empty<IGenomeStats<TG>>());
        }

        public static IGenomePoolStats<TG> Make<TG>(IEnumerable<IGenomeStats<TG>> genomeStatses) where TG : IGenome
        {
            return new GenomePoolStatsImpl<TG>(genomeStatses);
        }
    }

    class GenomePoolStatsImpl<TG> : IGenomePoolStats<TG> where TG : IGenome
    {
        public GenomePoolStatsImpl(IEnumerable<IGenomeStats<TG>> genomeStatses)
        {
            foreach (var genomeStats in genomeStatses)
            {
                _genomeStatses = _genomeStatses.Add(genomeStats.Genome.Guid, genomeStats);
            }
        }

        public void AddGenomeStatses(IEnumerable<IGenomeStats<TG>> genomeStatses)
        {
            foreach (var genomeStats in genomeStatses)
            {
                _genomeStatses = _genomeStatses.Add(genomeStats.Genome.Guid, genomeStats);
            }
        }

        public void AddGenomeEvals(IEnumerable<IGenomeEval<TG>> genomeEvals, 
                                                Func<TG, object> refrenceFunc)
        {
            foreach (var genomeEval in genomeEvals)
            {
                if (_genomeStatses.ContainsKey(genomeEval.Genome.Guid))
                {
                    _genomeStatses[genomeEval.Genome.Guid].AddGenomeEval(genomeEval);
                }
                else
                {
                    var genomeStat = GenomeStats.Make
                        (
                            genome: genomeEval.Genome, 
                            referenceResult:  refrenceFunc(genomeEval.Genome), 
                            firstGeneration: genomeEval.Generation
                        );
                    genomeStat.AddGenomeEval(genomeEval);
                    _genomeStatses = _genomeStatses.Add(genomeStat.Genome.Guid, genomeStat);
                }
            }
        }

        private IImmutableDictionary<Guid, IGenomeStats<TG>> _genomeStatses 
                        = ImmutableDictionary.Create<Guid, IGenomeStats<TG>>();

        public IEnumerable<IGenomeStats<TG>> GenomeStatses
        {
            get { return _genomeStatses.Values; }
        }
    }
}
