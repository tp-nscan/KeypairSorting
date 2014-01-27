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
        Tuple<IChromosome<T>, IChromosome<T>> Recombine(
            Func<IReadOnlyList<T>, IReadOnlyList<T>, Tuple<IReadOnlyList<T>, IReadOnlyList<T>>> recombinator, IReadOnlyList<T> partner);
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

        public static Func<IReadOnlyList<T>, IReadOnlyList<T>, Tuple<IReadOnlyList<T>, IReadOnlyList<T>>>
            StandardRecombinator<T>(
                                        IRando rando,
                                        double recombinationRate
                                   )
        {
            return (tLeft, tRight) =>
            {
                var retLeft = new List<T>();
                var retRight = new List<T>();

                var minCount = Math.Min(tLeft.Count, tRight.Count);

                var feedLeft = tLeft;
                var feedRight = tRight;

                for (var i = 0; i < minCount; i++)
                {
                    retLeft.Add(feedLeft[i]);
                    retRight.Add(feedRight[i]);

                    if (rando.NextDouble() > recombinationRate) continue;

                    var temp = feedLeft;
                    feedLeft = feedRight;
                    feedRight = temp;
                }

                if (feedLeft.Count > minCount)
                {
                    retLeft.AddRange(feedLeft.Skip(minCount).Take(feedLeft.Count - minCount));
                }

                if (feedRight.Count > minCount)
                {
                    retRight.AddRange(feedRight.Skip(minCount).Take(feedRight.Count - minCount));
                }

                return new Tuple<IReadOnlyList<T>, IReadOnlyList<T>>(retLeft, retRight);
            };
        }

        public static Tuple<IChromosome<T>, IChromosome<T>> StandardRecombine<T>
            (
                this IChromosome<T> chromosome,
                IChromosome<T> partner,
                IRando rando,
                double recombinationRate
            ) where T : IGene
        {
            return chromosome.Recombine(StandardRecombinator<T>(rando, recombinationRate), partner.Blocks);
        }

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
        public abstract Tuple<IChromosome<T>, IChromosome<T>> Recombine(
            Func<IReadOnlyList<T>, IReadOnlyList<T>, Tuple<IReadOnlyList<T>, IReadOnlyList<T>>> recombinator, 
            IReadOnlyList<T> partner);
    }
}