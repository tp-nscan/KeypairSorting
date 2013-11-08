using System;
using System.Collections.Generic;
using System.Linq;
using Evo.GenomeBuilders;
using Evo.Repositories;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Evo.Genomes
{
    public interface IChromosome : IReadOnlyList<int>
    {
        Guid Id { get; }
        ISymbolSet SymbolSet { get; }
    }

    public static class Chromosome
    {
        public static IChromosome ToChromosome(
                        this IChromosomeBuildInfo buildInfo,
                        IChromosomeRepository chromosomeRepository = null
        )
        {
            var asCopyInfo = buildInfo as IChromosomeCopyInfo;
            if (asCopyInfo != null)
            {
                return asCopyInfo.ToChromosomeCopy(chromosomeRepository);
            }

            var asGenInfo = buildInfo as IChromosomeGenInfo;
            if (asGenInfo != null)
            {
                return asGenInfo.ToChromosomeGen();
            }

            throw new Exception("unhandled chromosome build type");
        }

        static IChromosome ToChromosomeCopy(
            this IChromosomeCopyInfo copyInfo,
            IChromosomeRepository chromosomeRepository
            )
        {
            var rando = Rando.Fast(copyInfo.Seed);
            var parentChromosome = chromosomeRepository.GetChromosome(copyInfo.ParentIds.First());
            return UniformChromosome.MakeUniformChromosome
                (
                    data: parentChromosome.MutateInsertDelete
                    (
                        doMutation: rando.Spawn().ToBoolEnumerator(copyInfo.MutationRate),
                        doInsertion: rando.Spawn().ToBoolEnumerator(copyInfo.InsertionRate),
                        doDeletion: rando.Spawn().ToBoolEnumerator(copyInfo.DeletionRate),
                        mutator: T => rando.NextInt(parentChromosome.SymbolSet.Count),
                        inserter: T => rando.NextInt(parentChromosome.SymbolSet.Count),
                        deleter: () => rando.NextInt(parentChromosome.SymbolSet.Count)
                    ),
                    symbolSet: parentChromosome.SymbolSet,
                    guid: copyInfo.TargetId
                );
        }

        static IChromosome ToChromosomeGen(
            this IChromosomeGenInfo genInfo
            )
        {
            var rando = Rando.Fast(genInfo.Seed);
            return UniformChromosome.MakeUniformChromosome
                (
                    data: rando.ToIntEnumerator(genInfo.SymbolCount).Take(genInfo.ChromosomeLength),
                    symbolSet: SymbolSet.Make(genInfo.SymbolCount),
                    guid: genInfo.TargetId
                );
        }
    }

    public static class UniformChromosome
    {
        public static IChromosome MakeUniformChromosome(
            IEnumerable<int> data, ISymbolSet symbolSet, Guid guid)
        {
            return new UniformChromosomeImpl(
                    data,
                    symbolSet,
                    guid
                );
        }
    }

    class UniformChromosomeImpl : List<int>, IChromosome
    {
        public UniformChromosomeImpl(IEnumerable<int> data, ISymbolSet symbolSet, Guid id)
            : base(data)
        {
            _symbolSet = symbolSet;
            _id = id;
        }

        private readonly Guid _id;
        public Guid Id
        {
            get { return _id; }
        }

        private readonly ISymbolSet _symbolSet;
        public ISymbolSet SymbolSet
        {
            get { return _symbolSet; }
        }

        ISymbolSet IChromosome.SymbolSet
        {
            get { return SymbolSet; }
        }
    }
}
