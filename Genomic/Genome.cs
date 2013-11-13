﻿using System;
using System.Collections.Generic;
using MathUtils.Rand;

namespace Genomic
{
    public interface IParentGuid
    {
        Guid ParentGuid { get; }
    }

    public interface ISingleChromosome<out TC> where TC : IChromosome
    {
        TC Chromosome { get; } 
    }

    public interface IMultipleChromosomes
    {
        IReadOnlyList<IChromosome> Chromosomes { get; } 
    }

    public interface IGenome
    {
        Guid Guid { get; }
    }

    public interface ISimpleGenome<out TC> : IGenome, ISingleChromosome<TC>, IParentGuid
        where TC : IChromosome
    {
    }


    public static class Genome
    {
        public static ISimpleGenome<TC> MakeSimple<TC>(Guid guid, TC chromosome, Guid parentGuid)
            where TC : IChromosome
        {
            return new SimpleGenomeImpl<TC>(guid, chromosome, parentGuid);
        }

        public static ISimpleGenome<IUniformChromosome> Copy
            (
                this ISimpleGenome<IUniformChromosome> genome,
                IRando randy, 
                double mutationRate,
                double insertionRate,
                double deletionRate
            )
        {
            return new SimpleGenomeImpl<IUniformChromosome>
                (
                    guid: randy.NextGuid(),
                    chromosome: genome.Chromosome.Copy(randy, mutationRate, insertionRate, deletionRate),
                    parentGuid: genome.Guid
                );
        }
    }

    public class SimpleGenomeImpl<TC> : ISimpleGenome<TC>
        where TC : IChromosome
    {

        public SimpleGenomeImpl(Guid guid, TC chromosome, Guid parentGuid)
        {
            _guid = guid;
            _chromosome = chromosome;
            _parentGuid = parentGuid;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        private readonly TC _chromosome;
        public TC Chromosome
        {
            get { return _chromosome; }
        }

        private readonly Guid _parentGuid;
        public Guid ParentGuid
        {
            get { return _parentGuid; }
        }
    }
}