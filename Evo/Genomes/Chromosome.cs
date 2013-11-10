using System;
using System.Collections.Generic;
using System.Linq;
using Evo.GenomeBuilders;
using Evo.Repositories;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Evo.Genomes
{
    public interface IChromosome
    {
        int GeneCount { get; } 
        Guid Id { get; }
        Type GeneType { get; }
    }

    public interface IChromosome<T> : IChromosome
    {
        ISymbolSet<T> SymbolSet { get; }
        IReadOnlyList<T> Genes { get; } 
    }

    public static class Chromosome
    {
        public static IChromosome<T> ToChromosome<T>
            (
                this IChromosomeBuildInfo buildInfo,
                IChromosomeRepository chromosomeRepository = null
            )
        {
            var asCopyInfo = buildInfo as IChromosomeCopyInfo;
            if (asCopyInfo != null)
            {
                return asCopyInfo.ToChromosomeCopy<T>(chromosomeRepository);
            }

            var asGenInfo = buildInfo as IChromosomeGenInfo;
            if (asGenInfo != null)
            {
                return asGenInfo.ToChromosomeGen<T>();
            }

            throw new Exception("unhandled chromosome build type");
        }

        static IChromosome<T> ToChromosomeCopy<T>
            (
                this IChromosomeCopyInfo copyInfo,
                IChromosomeRepository chromosomeRepository
            )
        {
            var rando = Rando.Fast(copyInfo.Seed);
            var parentChromosome = (IChromosome<T>)chromosomeRepository.GetChromosome(copyInfo.ParentIds.First());
            return UniformChromosome.MakeUniformChromosome
                (
                    data: parentChromosome.Genes.MutateInsertDelete
                    (
                        doMutation: rando.Spawn().ToBoolEnumerator(copyInfo.MutationRate),
                        doInsertion: rando.Spawn().ToBoolEnumerator(copyInfo.InsertionRate),
                        doDeletion: rando.Spawn().ToBoolEnumerator(copyInfo.DeletionRate),
                        mutator:  _ => parentChromosome.SymbolSet.Choose(rando, _ ),
                        inserter: _ => parentChromosome.SymbolSet.Choose(rando, _),
                        deleter:  _ => parentChromosome.SymbolSet.Choose(rando, _)
                    ),
                    symbolSet: parentChromosome.SymbolSet,
                    guid: copyInfo.TargetId
                );
        }

        static IChromosome<T> ToChromosomeGen<T>(
            this IChromosomeGenInfo genInfo
            )
        {
            var rando = Rando.Fast(genInfo.Seed);
            var symbolSet = SymbolSet.Make<T>(genInfo.Initializer);
            return UniformChromosome.MakeUniformChromosome
                (
                    data: Enumerable.Range(0, genInfo.ChromosomeLength).Select(t => symbolSet.Choose(rando, default(T))),
                    symbolSet:symbolSet,
                    guid: genInfo.TargetId
                );
        }
    }

    public static class UniformChromosome
    {
        public static IChromosome<T> MakeUniformChromosome<T>(
            IEnumerable<T> data, ISymbolSet<T> symbolSet, Guid guid)
        {
            return new UniformChromosomeImpl<T>(
                    data,
                    symbolSet,
                    guid
                );
        }
    }

    class UniformChromosomeImpl<T> : IChromosome<T>
    {
        public UniformChromosomeImpl(IEnumerable<T> data, ISymbolSet<T> symbolSet, Guid id)
        {
            _genes = data.ToList();
            _symbolSet = symbolSet;
            _id = id;
        }

        private readonly IReadOnlyList<T> _genes;
        IReadOnlyList<T> IChromosome<T>.Genes
        {
            get { return _genes; }
        }

        public int GeneCount
        {
            get { return _genes.Count; }
        }

        private readonly Guid _id;
        public Guid Id
        {
            get { return _id; }
        }

        public Type GeneType
        {
            get { return typeof(T); }
        }

        private readonly ISymbolSet<T> _symbolSet;
        public ISymbolSet<T> SymbolSet
        {
            get { return _symbolSet; }
        }
    }
}
