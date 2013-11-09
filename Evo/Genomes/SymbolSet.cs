using System;
using MathUtils.Rand;

namespace Evo.Genomes
{
    public interface ISymbolSet<T>
    {
        int Count { get; }
        T Choose(IRando rando);
    }

    public static class SymbolSet
    {
        public static ISymbolSet<T> Make<T>(int symbolCount)
        {
            if (typeof (T) == typeof (int))
            {
                return (ISymbolSet<T>) new IntSymbols(symbolCount);
            }
            throw new Exception(String.Format("datatype {0} not supported", typeof(T).Name));
        }

        public static ISymbolSet<T> MakeFlags<T>(int symbolCount)
        {
            if (typeof(T) == typeof(uint))
            {
                return (ISymbolSet<T>)new UIntSymbols(symbolCount);
            }
            if (typeof(T) == typeof(ulong))
            {
                return (ISymbolSet<T>)new ULongSymbols(symbolCount);
            }
            throw new Exception(String.Format("datatype {0} not supported", typeof(T).Name));
        }
    }

    public abstract class SymbolSetBase<T> : ISymbolSet<T>
    {
        protected SymbolSetBase(int count)
        {
            _count = count;
        }

        private readonly int _count;
        public int Count
        {
            get { return _count; }
        }

        public abstract T Choose(IRando rando);
    }

    public class IntSymbols : SymbolSetBase<int>
    {
        public IntSymbols(int symbolCount)
            : base(symbolCount)
        {
        }

        public override int Choose(IRando rando)
        {
            return rando.NextInt(Count);
        }
    }

    public class UIntSymbols : SymbolSetBase<uint>
    {
        private readonly int _bitCount;

        public UIntSymbols(int bitCount)
            : base(1 << (bitCount - 1))
        {
            _bitCount = bitCount;
        }

        public int BitCount
        {
            get { return _bitCount; }
        }

        public override uint Choose(IRando rando)
        {
            return rando.NextUint() >> (32 - BitCount);
        }
    }

    public class ULongSymbols : SymbolSetBase<ulong>
    {
        private readonly int _bitCount;

        public ULongSymbols(int bitCount)
            : base(1 << (bitCount - 1))
        {
            _bitCount = bitCount;
        }

        public int BitCount
        {
            get { return _bitCount; }
        }

        public override ulong Choose(IRando rando)
        {
            ulong temp = rando.NextUint();
            var whole = (temp << 32) + rando.NextUint();
            return whole >> (64 - BitCount);
        }
    }
}
