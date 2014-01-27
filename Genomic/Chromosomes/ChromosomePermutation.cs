using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genes;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic.Chromosomes
{
    public interface IChromosomePermutation : IChromosome<IGenePermutation>
    {
        double MixingRate { get; }
        int PermutationItemCount { get; }
    }

    public static class ChromosomePermutation
    {
        public static IChromosomePermutation Make
            (
                IReadOnlyList<uint> sequence, 
                int permutationItemCount, 
                double mixingRate
            )
        {
            return new ChromosomePermutationImpl(
                    sequence: sequence, 
                    permutationItemCount: permutationItemCount, 
                    mixingRate: mixingRate
                );
        }
    }

    internal class ChromosomePermutationImpl : ChromosomeImpl<IGenePermutation>, IChromosomePermutation
    {
        public ChromosomePermutationImpl(
            IReadOnlyList<uint> sequence, 
            int permutationItemCount, 
            double mixingRate
            ) : base(sequence)
        {
            _permutationItemCount = permutationItemCount;
            _mixingRate = mixingRate;
        }

        private readonly double _mixingRate;
        public double MixingRate
        {
            get { return _mixingRate; }
        }

        private readonly int _permutationItemCount;
        public int PermutationItemCount
        {
            get { return _permutationItemCount; }
        }

        private IReadOnlyList<IGenePermutation> _blockList;
        public override IReadOnlyList<IGenePermutation> Blocks
        {
            get
            {
                return _blockList ??
                       (
                           _blockList = Sequence.Chunk(PermutationItemCount)
                               .Select(ul => GenePermutation.Make(ul, MixingRate))
                                .ToList()
                       );
            }
        }

        public override IGenePermutation NewBlock(IRando rando)
        {
            return GenePermutation.Make
                (
                    items: Enumerable.Range(0, PermutationItemCount)
                                .Cast<uint>()
                                .ToArray().FisherYatesShuffle(rando),
                    mixingRate: MixingRate
                );
        }

        public override IChromosome<IGenePermutation> Mutate(Func<IReadOnlyList<IGenePermutation>, IReadOnlyList<IGenePermutation>> mutator)
        {
            return new ChromosomePermutationImpl
                (
                    mutator(Blocks).SelectMany(b => b.ToIntStream).ToList(),
                    PermutationItemCount,
                    MixingRate
                );
        }

        public override Tuple<IChromosome<IGenePermutation>, IChromosome<IGenePermutation>> Recombine(
            Func<
                 IReadOnlyList<IGenePermutation>, IReadOnlyList<IGenePermutation>,
                 Tuple<IReadOnlyList<IGenePermutation>, IReadOnlyList<IGenePermutation>>
                > recombinator,
            IReadOnlyList<IGenePermutation> partner)
        {
            var children = recombinator(Blocks, partner);

            return new Tuple<IChromosome<IGenePermutation>, IChromosome<IGenePermutation>>(
                new ChromosomePermutationImpl
                (
                    children.Item1.SelectMany(b => b.ToIntStream).ToList(),
                    PermutationItemCount,
                    MixingRate
                ),
                new ChromosomePermutationImpl
                (
                    children.Item1.SelectMany(b => b.ToIntStream).ToList(),
                    PermutationItemCount,
                    MixingRate
                )
           );
        }
    }
}
