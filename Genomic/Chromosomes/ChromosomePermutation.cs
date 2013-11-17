using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genes;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic.Chromosomes
{
    internal class ChromosomePermutation : ChromosomeImpl<IGenePermutation>
    {
        public ChromosomePermutation(
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
            return new ChromosomePermutation
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
