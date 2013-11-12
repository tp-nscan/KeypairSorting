﻿using System;
using System.Collections.Generic;
using System.Linq;
using Evo.GenomeBuilders;
using Evo.Repositories;

namespace Evo.Genomes
{
    public interface IGenome<T>
    {
        IReadOnlyList<IChromosome<T>> Chromosomes { get; }
        IGenomeBuildInfo GenomeBuildInfo { get; }
        Guid Guid { get; }
    }

    public static class Genome
    {
        public static IGenome<T> ToGenome<T>(
            this IGenomeBuildInfo genomeBuildInfo, 
            IChromosomeRepository chromosomeRepository)
        {
            return new GenomeImpl<T>
                (
                    guid: genomeBuildInfo.TargetId,
                    genomeBuildInfo: genomeBuildInfo,
                    chromosomes: genomeBuildInfo.ChromosomeBuildInfos
                                    .Select(t => t.ToChromosome<T>(chromosomeRepository))
                                    .ToList()
                );
        }
    }

    class GenomeImpl<T> : IGenome<T>
    {
        private readonly IGenomeBuildInfo _genomeBuildInfo;
        private readonly Guid _guid;
        private readonly IReadOnlyList<IChromosome<T>> _chromosomes;

        public GenomeImpl(Guid guid, IGenomeBuildInfo genomeBuildInfo,
            IReadOnlyList<IChromosome<T>> chromosomes)
        {
            _genomeBuildInfo = genomeBuildInfo;
            _chromosomes = chromosomes;
            _guid = guid;
        }

        public IReadOnlyList<IChromosome<T>> Chromosomes
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
