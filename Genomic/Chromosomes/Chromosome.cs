using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genes;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic.Chromosomes
{
    public interface IChromosome
    {
        Guid Guid { get; }
        IReadOnlyList<uint> Sequence { get; }
        IChromosome ReplaceDataWith(IEnumerable<uint> data, Guid newGuid);
    }

    public interface IChromosome<T> : IChromosome
        where T : IGene
    {
        IReadOnlyList<T> Blocks { get; }
        T NewBlock(IRando rando);
    }

    public static class Chromosome
    {
        public static IChromosome<IGeneUintModN> ToChromosomeUintN
            (
                this IEnumerable<uint> sequence, 
                Guid guid, 
                uint maxVal
            )
        {
            return new ChromosomeUintN(guid, sequence.ToList(), maxVal);
        }

        public static IChromosome ToChromosomeUlongN
            (
                this IEnumerable<uint> sequence, 
                Guid guid, 
                ulong maxVal
            )
        {
            return new ChromosomeUlongNImpl(guid, sequence.ToList(), maxVal);
        }

        public static IChromosome ToChromosomePermutation
            (
                this IEnumerable<uint> sequence, 
                Guid guid,
                int permutationItemCount,
                double mixingRate
            )
        {
            return ChromosomePermutation.Make
                (
                    guid:   guid,
                    sequence:   sequence.ToList(),
                    permutationItemCount:   permutationItemCount,
                    mixingRate: mixingRate
                );
        }

        public static IChromosome ToChromosomeBits
        (
                this IEnumerable<uint> sequence,
                Guid guid,
                int bitCount
        )
        {
            return new ChromosomeBitsImpl
                (
                    guid: guid,
                    sequence: sequence.ToList(),
                    bitCount: bitCount
                );
        }

        public static IChromosome<T> Copy<T>
            (
                this IChromosome<T> chromosome,
                IRando randy,
                double mutationRate,
                double insertionRate,
                double deletionRate
            ) where T : IGene
        {
            return (IChromosome<T>) chromosome.ReplaceDataWith(
                data: chromosome.Blocks.MutateInsertDelete
                    (
                        doMutation: randy.ToBoolEnumerator(mutationRate),
                        doInsertion: randy.ToBoolEnumerator(insertionRate),
                        doDeletion: randy.ToBoolEnumerator(deletionRate),
                        mutator: x => (T)x.Mutate(randy),
                        inserter: x => (T)x.Mutate(randy),
                        paddingFunc: x => chromosome.NewBlock(randy)
                    )
                    .SelectMany(b => b.AsSerialized),
                newGuid: Guid.NewGuid()
                );
        }
    }

    abstract class ChromosomeImpl<T> : IChromosome<T> where T : IGene
    {
        protected ChromosomeImpl(Guid guid, IReadOnlyList<uint> sequence)
        {
            _guid = guid;
            _sequence = sequence;
        }

        private readonly IReadOnlyList<uint> _sequence;
        public IReadOnlyList<uint> Sequence
        {
            get { return _sequence; }
        }

        public abstract IChromosome ReplaceDataWith(IEnumerable<uint> data, Guid newGuid);

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public abstract IReadOnlyList<T> Blocks
        {
            get;
        }

        public abstract T NewBlock(IRando rando);
    }
}