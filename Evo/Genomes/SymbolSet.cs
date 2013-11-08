namespace Evo.Genomes
{
    public interface ISymbolSet
    {
        int Count { get; }
    }

    public static class SymbolSet
    {
        public static ISymbolSet Make(int symbolCount)
        {
            return new BasicSymbolSetImpl(symbolCount);
        }
    }

    public class BasicSymbolSetImpl : ISymbolSet
    {
        public BasicSymbolSetImpl(int count)
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
