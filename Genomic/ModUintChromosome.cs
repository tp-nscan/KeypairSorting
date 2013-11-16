using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Rand;

namespace Genomic
{
    class ModUintChromosome : ChromosomeImpl<GeneUintModN>
    {
        public ModUintChromosome(
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

        private IReadOnlyList<GeneUintModN> _blockList;
        public override IChromosome ReplaceDataWith(IEnumerable<uint> data, Guid newGuid)
        {
            return new ModUintChromosome
                (
                    guid: newGuid,
                    sequence: data.ToList(),
                    maxVal: MaxVal
                );
        }

        public override IReadOnlyList<GeneUintModN> Blocks
        {
            get
            {
                return _blockList ?? (_blockList = Sequence.Select
                    (
                        t => new GeneUintModN(t, MaxVal)).ToList()
                    );
            }
        }

        public override GeneUintModN NewBlock(IRando rando)
        {
            return new GeneUintModN(rando.NextUint(MaxVal), MaxVal);
        }
    }
}