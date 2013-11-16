using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic
{
    internal class ModUlongChromosome : ChromosomeImpl<GeneUlongModN>
    {
        public ModUlongChromosome(
            Guid guid, 
            IReadOnlyList<uint> sequence,
            ulong maxVal
            ) : base(guid, sequence)
        {
            _maxVal = maxVal;
        }

        private readonly ulong _maxVal;
        public ulong MaxVal
        {
            get { return _maxVal; }
        }

        private IReadOnlyList<GeneUlongModN> _blockList;
        public override IChromosome ReplaceDataWith(IEnumerable<uint> data, Guid newGuid)
        {
            return new ModUlongChromosome
                (
                    guid: newGuid,
                    sequence: data.ToList(),
                    maxVal: _maxVal
                );
        }

        public override IReadOnlyList<GeneUlongModN> Blocks
        {
            get
            {
                return _blockList ??
                       (
                           _blockList = Sequence.ToUlongs()
                               .Select(ul => new GeneUlongModN(ul, MaxVal))
                                .ToList()
                           );
            }
        }

        public override GeneUlongModN NewBlock(IRando rando)
        {
            return new GeneUlongModN(rando.NextUlong(MaxVal), MaxVal);
        }
    }
}