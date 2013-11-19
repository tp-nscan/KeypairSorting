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
                Guid guid,
                IReadOnlyList<uint> sequence,
                int bitCount
            )
        {
            return new ChromosomeBitsImpl
                (
                    guid: guid,
                    sequence: sequence,
                    bitCount: bitCount
                );
        }
    }

    class ChromosomeBitsImpl : ChromosomeImpl<IGeneBits>, IChromosomeBits
    {
        public ChromosomeBitsImpl
            (
                Guid guid,
                IReadOnlyList<uint> sequence, 
                int bitCount
            ) : base(guid, sequence)
        {
            _bitCount = bitCount;
        }

        private readonly int _bitCount;
        public int BitCount
        {
            get { return _bitCount; }
        }

        public override IChromosome ReplaceDataWith
            (
                IEnumerable<uint> data, 
                Guid newGuid
            )
        {
             return ChromosomeBits.Make
                 (
                    guid: newGuid,
                    sequence: data.ToList(),
                    bitCount: _bitCount
                 );
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
    }
}
