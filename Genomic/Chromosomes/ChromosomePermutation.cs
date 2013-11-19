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
                Guid guid, 
                IReadOnlyList<uint> sequence, 
                int permutationItemCount, 
                double mixingRate
            )
        {
            return new ChromosomePermutationImpl(
                    guid: guid, 
                    sequence: sequence, 
                    permutationItemCount: permutationItemCount, 
                    mixingRate: mixingRate
                );
        }
    }

    internal class ChromosomePermutationImpl : ChromosomeImpl<IGenePermutation>, 
        IChromosomePermutation
    {
        public ChromosomePermutationImpl(
            Guid guid, 
            IReadOnlyList<uint> sequence, 
            int permutationItemCount, 
            double mixingRate
            ) : base(guid, sequence)
        {
            _permutationItemCount = permutationItemCount;
            _mixingRate = mixingRate;
        }


        public override IChromosome ReplaceDataWith(IEnumerable<uint> data, Guid newGuid)
        {
            return ChromosomePermutation.Make
                (
                    guid: newGuid,
                    sequence: data.ToList(),
                    permutationItemCount: _permutationItemCount,
                    mixingRate : MixingRate
                );
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
    }
}
