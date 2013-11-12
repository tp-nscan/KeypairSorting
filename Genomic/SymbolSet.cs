using System.Collections.Generic;
using MathUtils.Rand;

namespace Genomic
{
    public interface IUniformSymbolSet : ISymbolSet
    {
        uint MaxVal { get; }
    }

    public interface ISymbolSet
    {
    }

    public static class SymbolSet
    {
        public static IEnumerable<uint> Choose(this IUniformSymbolSet uniformSymbolSet, IRando randy)
        {
            return randy.ToUints(uniformSymbolSet.MaxVal);
        }

        public static IUniformSymbolSet MakeUniformSymbolSet(uint maxVal)
        {
            return new UniformSymbolSetImpl(maxVal);
        }
    }

    class UniformSymbolSetImpl : IUniformSymbolSet
    {
        private readonly uint _maxVal;

        public UniformSymbolSetImpl(uint maxVal)
        {
            _maxVal = maxVal;
        }

        public uint MaxVal
        {
            get { return _maxVal; }
        }
    }
}