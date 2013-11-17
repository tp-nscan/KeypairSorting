using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Rand;

namespace Genomic.Genes
{
    public interface IGeneBits : IGene
    {
        IReadOnlyList<bool> Bits { get; }
    }

    public static class GeneBits
    {

    }

    class GeneBitsImpl : IGeneBits
    {
        private IEnumerable<uint> _asSerialized;

        public IEnumerable<uint> AsSerialized
        {
            get 
            { 
                return _asSerialized ?? 
                (
                    _asSerialized = Bits.Select(b=>b ? (uint)1 : 0)
                ); 
            }
        }

        public IGene Mutate(IRando rando)
        {
            throw new NotImplementedException();
        }

        private IReadOnlyList<bool> _bits;
        public IReadOnlyList<bool> Bits
        {
            get { return _bits; }
        }
    }

}
