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
        IReadOnlyList<uint> Sequence { get; }
    }

    public interface IChromosome<T> : IChromosome
        where T : IGene
    {
        IReadOnlyList<T> Blocks { get; }
        T NewBlock(IRando rando);
        IChromosome<T> Mutate(Func<IReadOnlyList<T>, IReadOnlyList<T>> mutator);
    }

    public static class Chromosome
    {
        public static IChromosomeUint ToChromosomeUint
            (
                this IReadOnlyList<uint> sequence,
                uint maxVal
            )
        {
            return new ChromosomeUintImpl(sequence.ToList(), maxVal);
        }

        public static IChromosomeUlongN ToChromosomeUlongN
            (
                this IReadOnlyList<uint> sequence,
                ulong maxVal
            )
        {
            return new ChromosomeUlongNImpl(sequence.ToList(), maxVal);
        }

        public static IChromosomePermutation ToChromosomePermutation
            (
                this IReadOnlyList<uint> sequence, 
                Guid guid,
                int permutationItemCount,
                double mixingRate
            )
        {
            return ChromosomePermutation.Make
                (
                    sequence:   sequence.ToList(),
                    permutationItemCount:   permutationItemCount,
                    mixingRate: mixingRate
                );
        }

        public static IChromosomeBits ToChromosomeBits
        (
                this IReadOnlyList<uint> sequence,
                Guid guid,
                int bitCount
        )
        {
            return new ChromosomeBitsImpl
                (
                    sequence: sequence.ToList(),
                    bitCount: bitCount
                );
        }

        public static Func<IReadOnlyList<T>, IReadOnlyList<T>> StandardMutator<T>
            (
                double deletionRate, 
                double insertionRate, 
                double mutationRate, 
                Func<T> geneMutator,
                IRando rando
            )
        {
            return list =>
            {
                var lp = new ListPropigator<T>
                (
                    deletor: (l, i) => l.RemoveAt(i),
                    deletionFrequency: deletionRate,
                    inserter: (l, i) => l.Insert(i, geneMutator.Invoke()),
                    insertionFrequency: insertionRate,
                    mutator: (l, i) => l.SetItem(i, geneMutator.Invoke()),
                    mutationFrequency: mutationRate,
                    finalIndex: list.Count
                );

                return list.Mutate(lp, rando);
            };
        }

        public static IChromosome<T> StandardPropigate<T>
            (
                this IChromosome<T> chromosome,
                IRando rando,
                double mutationRate,
                double insertionRate,
                double deletionRate
            ) where T : IGene
        {
            return chromosome.Mutate(
                mutator: StandardMutator
                    (
                        deletionRate: deletionRate,
                        insertionRate: insertionRate,
                        mutationRate: mutationRate,
                        geneMutator: () => chromosome.NewBlock(rando),
                        rando: rando
                    )
                );
        }

        //public static IChromosome<T> StandardPropigate<T>
        //    (
        //        this IChromosome<T> chromosome,
        //        IRando rando,
        //        double mutationRate,
        //        double insertionRate,
        //        double deletionRate
        //    ) where T : IGene
        //{
        //    return chromosome.Mutate(
        //        mutator: StandardMutator
        //            (
        //                deletionRate: deletionRate,
        //                insertionRate: insertionRate,
        //                mutationRate: mutationRate, 
        //                geneMutator: () => chromosome.NewBlock(rando),
        //                rando: rando
        //            )
        //        );
        //}

    }

    abstract class ChromosomeImpl<T> : IChromosome<T> where T : IGene
    {
        protected ChromosomeImpl(IReadOnlyList<uint> sequence)
        {
            _sequence = sequence;
        }

        private readonly IReadOnlyList<uint> _sequence;

        public IReadOnlyList<uint> Sequence
        {
            get { return _sequence; }
        }

        public abstract IReadOnlyList<T> Blocks
        {
            get;
        }

        public abstract T NewBlock(IRando rando);

        public abstract IChromosome<T> Mutate(Func<IReadOnlyList<T>, IReadOnlyList<T>> mutator);
    }
}