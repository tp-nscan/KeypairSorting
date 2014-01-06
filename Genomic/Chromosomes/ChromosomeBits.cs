using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genes;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic.Chromosomes
{
    public interface IChromosomeBits : IChromosome<IGeneBits>
    {
        int BitCount { get; }
    }

    public static class ChromosomeBits
    {
        public static IChromosomeBits Make
            (
                IReadOnlyList<uint> sequence,
                int bitCount
            )
        {
            return new ChromosomeBitsImpl
                (
                    sequence: sequence,
                    bitCount: bitCount
                );
        }
    }

    class ChromosomeBitsImpl : ChromosomeImpl<IGeneBits>, IChromosomeBits
    {
        public ChromosomeBitsImpl
            (
                IReadOnlyList<uint> sequence, 
                int bitCount
            ) : base(sequence)
        {
            _bitCount = bitCount;
        }

        private readonly int _bitCount;
        public int BitCount
        {
            get { return _bitCount; }
        }

        private IReadOnlyList<IGeneBits> _blockList;
        public override IReadOnlyList<IGeneBits> Blocks
        {
            get
            {
                return _blockList ??
                       (
                           _blockList = Sequence.Chunk(BitCount)
                               .Select(ul => ul.ToGeneBits())
                               .ToList()
                       );
            }
        }

        public override IGeneBits NewBlock(IRando rando)
        {
            return rando.ToBoolEnumerator(0.5).Take(BitCount)
                .ToGeneBits();
        }

        public override IChromosome<IGeneBits> Mutate(Func<IReadOnlyList<IGeneBits>, IReadOnlyList<IGeneBits>> mutator)
        {
            return new ChromosomeBitsImpl
                (
                    mutator(Blocks).SelectMany(b => b.ToIntStream).ToList(), 
                    BitCount
                );
        }
    }
}
