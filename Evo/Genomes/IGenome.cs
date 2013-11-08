using System;
using System.Collections.Generic;
using System.Linq;
using Evo.GenomeBuilders;
using Evo.Repositories;

namespace Evo.Genomes
{
    public interface IGenome
    {
        IReadOnlyList<IChromosome> Chromosomes { get; }
        IGenomeBuildInfo GenomeBuildInfo { get; }
        Guid Guid { get; }
    }

    public static class Genome
    {
        public static IGenome ToGenome(
            this IGenomeBuildInfo genomeBuildInfo, 
            IChromosomeRepository chromosomeRepository)
        {
            return new GenomeImpl
                (
                    guid: genomeBuildInfo.TargetId,
                    genomeBuildInfo: genomeBuildInfo,
                    chromosomes: genomeBuildInfo.ChromosomeBuildInfos.Select(t => t.ToChromosome(chromosomeRepository)).ToList()
                );
        }
    }

    class GenomeImpl : IGenome
    {
        private readonly IGenomeBuildInfo _genomeBuildInfo;
        private readonly Guid _guid;
        private readonly IReadOnlyList<IChromosome> _chromosomes;

        public GenomeImpl(Guid guid, IGenomeBuildInfo genomeBuildInfo, 
            IReadOnlyList<IChromosome> chromosomes)
        {
            _genomeBuildInfo = genomeBuildInfo;
            _chromosomes = chromosomes;
            _guid = guid;
        }

        public IReadOnlyList<IChromosome> Chromosomes
        {
            get { return _chromosomes; }
        }

        public IGenomeBuildInfo GenomeBuildInfo
        {
            get { return _genomeBuildInfo; }
        }

        public Guid Guid
        {
            get { return _guid; }
        }
    }
}
