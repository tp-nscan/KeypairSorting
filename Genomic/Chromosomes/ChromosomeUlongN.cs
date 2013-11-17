using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genes;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic.Chromosomes
{
    internal class ChromosomeUlongN : ChromosomeImpl<IGeneUlongModN>
    {
        public ChromosomeUlongN(
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

        private IReadOnlyList<IGeneUlongModN> _blockList;
        public override IChromosome ReplaceDataWith(IEnumerable<uint> data, Guid newGuid)
        {
            return new ChromosomeUlongN
                (
                    guid: newGuid,
                    sequence: data.ToList(),
                    maxVal: _maxVal
                );
        }

        public override IReadOnlyList<IGeneUlongModN> Blocks
        {
            get
            {
                return _blockList ??
                       (
                           _blockList = Sequence.ToUlongs()
                               .Select(ul => GeneUlongModN.Make(ul, MaxVal))
                                .ToList()
                       );
            }
        }

        public override IGeneUlongModN NewBlock(IRando rando)
        {
            return GeneUlongModN.Make(rando.NextUlong(MaxVal), MaxVal);
        }
    }
}