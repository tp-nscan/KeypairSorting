using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genes;
using MathUtils.Rand;

namespace Genomic.Chromosomes
{
    internal class ChromosomeUintN : ChromosomeImpl<IGeneUintModN>
    {
        public ChromosomeUintN
            (
                Guid guid, 
                IReadOnlyList<uint> sequence, 
                uint maxVal
            ) : base(guid, sequence)
        {
            _maxVal = maxVal;
        }

        private readonly uint _maxVal;
        public uint MaxVal
        {
            get { return _maxVal; }
        }

        private IReadOnlyList<IGeneUintModN> _blockList;
        public override IChromosome ReplaceDataWith(IEnumerable<uint> data, Guid newGuid)
        {
            return new ChromosomeUintN
                (
                    guid: newGuid,
                    sequence: data.ToList(),
                    maxVal: MaxVal
                );
        }

        public override IReadOnlyList<IGeneUintModN> Blocks
        {
            get
            {
                return _blockList ?? 
                    (
                        _blockList = Sequence
                        .Select(t => GeneUintModN.Make(t, MaxVal))
                        .ToList()
                    );
            }
        }

        public override IGeneUintModN NewBlock(IRando rando)
        {
            return GeneUintModN.Make(rando.NextUint(MaxVal), MaxVal);
        }
    }
}