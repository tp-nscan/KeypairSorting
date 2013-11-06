using System.Collections.Generic;
using System.Linq;
using MathUtils.Rand;

namespace Evo.Genomes
{
    public interface IChromosome : IReadOnlyList<int>
    {
        ISymbolSet SymbolSet { get; }
    }

    public interface IUniformChromosome : IChromosome
    {
        new IBasicSymbolSet SymbolSet { get; }
    }

    public static class UniformChromosome
    {
        public static IUniformChromosome MakeUniformChromosome(
            IEnumerable<int> data, IBasicSymbolSet symbolSet)
        {
            return new UniformChromosomeImpl(
                    data,
                    symbolSet
                );
        }
    }

    class UniformChromosomeImpl : List<int>, IUniformChromosome
    {
        public UniformChromosomeImpl(IEnumerable<int> data, IBasicSymbolSet symbolSet)
            : base(data)
        {
            _symbolSet = symbolSet;
        }

        private readonly IBasicSymbolSet _symbolSet;
        public IBasicSymbolSet SymbolSet
        {
            get { return _symbolSet; }
        }

        ISymbolSet IChromosome.SymbolSet
        {
            get { return SymbolSet; }
        }
    }
}
