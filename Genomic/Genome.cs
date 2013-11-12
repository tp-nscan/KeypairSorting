using System;
using System.Collections.Generic;
using System.Linq;

namespace Genomic
{
    public interface IParentGuid
    {
        Guid ParentGuid { get; }
    }

    public interface ISingleChromosome
    {
        IChromosome Chromosome { get; } 
    }

    public interface IMultipleChromosomes
    {
        IReadOnlyList<IChromosome> Chromosomes { get; } 
    }

    public interface IGenome
    {
        Guid Guid { get; }
    }

    public interface ISimpleGenome : IGenome, ISingleChromosome, IParentGuid
    {
    }


    public static class Genome
    {
        public static ISimpleGenome MakeSimpleGenome(Guid guid, IChromosome chromosome, Guid parentGuid)
        {
         return new SimpleGenomeImpl(guid, chromosome, parentGuid);
        }

        //public static ISimpleGenome Copy(this ISimpleGenome genome, Guid newGuid, IChromosome chromosome, Guid parentGuid)
        //{
        //    return new SimpleGenomeImpl(newGuid, genome.Chromosome.C .Copy(), genome.Guid);
        //}
    }

    class SimpleGenomeImpl : ISimpleGenome
    {

        public SimpleGenomeImpl(Guid guid, IChromosome chromosome, Guid parentGuid)
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

        private readonly IChromosome _chromosome;
        public IChromosome Chromosome
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