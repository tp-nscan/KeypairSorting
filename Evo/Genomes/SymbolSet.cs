using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Rand;

namespace Evo.Genomes
{
    public interface ISymbolSet
    {
        Type InitTokenType { get; }
    }
    public interface ISymbolSet<T> : ISymbolSet
    {
        T Choose(IRando rando, T basis);
    }

    public static class SymbolSet
    {
        static SymbolSet()
        {
            SymbolSetMakers = new Dictionary<Type, Func<object, ISymbolSet>>();

            SymbolSetMakers[typeof(IntSymbols.Initializer)] = o => new IntSymbols((IntSymbols.Initializer) o);
            SymbolSetMakers[typeof(UIntSymbols.Initializer)] = o => new UIntSymbols((UIntSymbols.Initializer)o);
            SymbolSetMakers[typeof(ULongSymbols.Initializer)] = o => new ULongSymbols((ULongSymbols.Initializer)o);
            SymbolSetMakers[typeof(IntArraySymbols.Initializer)] = o => new IntArraySymbols((IntArraySymbols.Initializer)o);
        }

        private static readonly Dictionary<Type, Func<object, ISymbolSet>> SymbolSetMakers;

        public static void RegisterSymbolSet(Type initType, Func<object, ISymbolSet> maker)
        {
            SymbolSetMakers[initType] = maker;
        }

        public static ISymbolSet<T> Make<T>(object initializer)
        {
            var func = SymbolSetMakers[initializer.GetType()];
            return (ISymbolSet<T>) func(initializer);
        }
    }

    public abstract class SymbolSetBase<T> : ISymbolSet<T>
    {
        protected SymbolSetBase(object initInitializer)
        {
            _initInitializer = initInitializer;
        }

        private readonly object _initInitializer;
        public object InitInitializer
        {
            get { return _initInitializer; }
        }

        public abstract T Choose(IRando rando, T basis);

        public Type InitTokenType
        {
            get { return _initInitializer.GetType(); }
        }
    }

    public class IntSymbols : SymbolSetBase<int>
    {
        public IntSymbols(Initializer initializer)
            : base(initializer)
        {
            _count = initializer.Count;
        }

        private readonly int _count;
        public override int Choose(IRando rando, int basis)
        {
            return rando.NextInt(_count);
        }

        public class Initializer
        {
            public Initializer(int count)
            {
                _count = count;
            }

            private readonly int _count;
            public int Count
            {
                get { return _count; }
            }
        }
    }

    public class UIntSymbols : SymbolSetBase<uint>
    {
        public UIntSymbols(Initializer uIntSymbolsInitializer)
            : base(uIntSymbolsInitializer)
        {
            _bitCount = uIntSymbolsInitializer.BitCount;
        }

        private readonly int _bitCount;
        public override uint Choose(IRando rando, uint basis)
        {
            return rando.NextUint() >> (32 - _bitCount);
        }

        public class Initializer
        {
            public Initializer(int bitCount)
            {
                _bitCount = bitCount;
            }

            private readonly int _bitCount;
            public int BitCount
            {
                get { return _bitCount; }
            }
        }
    }

    public class ULongSymbols : SymbolSetBase<ulong>
    {
        public ULongSymbols(Initializer ulongSymbolsInitializer)
            : base(ulongSymbolsInitializer)
        {
            _bitCount = ulongSymbolsInitializer.BitCount;
        }

        private readonly int _bitCount;
        public override ulong Choose(IRando rando, ulong basis)
        {
            ulong temp = rando.NextUint();
            var whole = (temp << 32) + rando.NextUint();
            return whole >> (64 - _bitCount);
        }

        public class Initializer
        {
            public Initializer(int bitCount)
            {
                _bitCount = bitCount;
            }

            private readonly int _bitCount;
            public int BitCount
            {
                get { return _bitCount; }
            }
        }
    }

    public class IntArraySymbols : SymbolSetBase<int[]>
    {
        public IntArraySymbols(Initializer ulongSymbolsInitializer)
            : base(ulongSymbolsInitializer)
        {
            _sortedArray = Enumerable.Range(0, ulongSymbolsInitializer.ArrayLength).ToArray();
        }

        private readonly IReadOnlyList<int> _sortedArray;
        public override int[] Choose(IRando rando, int[] basis)
        {
            return rando.FisherYatesShuffle(_sortedArray);
        }

        public class Initializer
        {
            public Initializer(int arrayLength)
            {
                _arrayLength = arrayLength;
            }

            private readonly int _arrayLength;
            public int ArrayLength
            {
                get { return _arrayLength; }
            }
        }
    }
}
