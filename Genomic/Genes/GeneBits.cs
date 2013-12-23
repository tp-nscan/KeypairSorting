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
        public static IGeneBits ToGeneBits(this IEnumerable<bool> bits)
        {
            return new GeneBitsImpl(bits);
        }

        public static IGeneBits ToGeneBits(this IEnumerable<uint> data)
        {
            return new GeneBitsImpl(data);
        }
    }

    class GeneBitsImpl : IGeneBits
    {
        public GeneBitsImpl(IEnumerable<bool> bits)
        {
            _bits = bits.ToList();
        }

        public GeneBitsImpl(IEnumerable<uint> data)
        {
            _bits = data.Select(t=> (t==1)).ToList();
        }

        private IEnumerable<uint> _asSerialized;
        public IEnumerable<uint> ToIntStream
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
            return rando.ToBoolEnumerator(0.5).Take(Bits.Count)
                .ToGeneBits();
        }

        private readonly IReadOnlyList<bool> _bits;
        public IReadOnlyList<bool> Bits
        {
            get { return _bits; }
        }
    }

}
