using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genes;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic.Chromosomes
{
    class ChromosomeBits : ChromosomeImpl<IGeneBits>
    {
        public ChromosomeBits
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

        public override IChromosome ReplaceDataWith(IEnumerable<uint> data, Guid newGuid)
        {
             return new ChromosomeBits
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
