using System.Collections.Generic;
using MathUtils.Rand;

namespace Evo.Genomes
{
    public interface ISymbolSet
    {
        IEnumerable<int> Choose(IRando rando);
    }

    public interface IBasicSymbolSet : ISymbolSet
    {
        int Count { get; }
    }

    public static class BasicSymbolSet
    {
        public static IBasicSymbolSet Make(int symbolCount)
        {
            return new BasicSymbolSetImpl(symbolCount);
        }
    }

    public class BasicSymbolSetImpl : IBasicSymbolSet
    {
        public BasicSymbolSetImpl(int count)
        {
            _count = count;
        }

        public IEnumerable<int> Choose(IRando rando)
        {
            while (true)
            {
                yield return rando.NextInt(Count);
            }
        }

        private readonly int _count;
        public int Count
        {
            get { return _count; }
        }
    }
}
