using System;
using System.Collections.Immutable;
using SorterEvo.Genomes;
using Sorting.CompetePools;

namespace SorterEvo.Evals
{
    public interface ISorterGenomeEval
    {
        ISorterGenome SorterGenome { get; }
        IImmutableStack<Guid> Ancestors { get; }
        ISorterEval SorterEval { get; }
    }

    public static class SorterGenomeEval
    {
        public static ISorterGenomeEval Make(
                ISorterGenome sorterGenome,
                IImmutableStack<Guid> ancestors,
                ISorterEval sorterEval
            )
        {
            return new SorterGenomeEvalImpl
                (
                    sorterGenome: sorterGenome,
                    ancestors: ancestors,
                    sorterEval: sorterEval
                );
        }

        public static ISorterGenomeEval Make(
            ISorterGenome sorterGenome,
            ISorterGenomeEval parentGenomeEval,
            ISorterEval sorterEval
        )
        {
            return Make
                (
                    sorterGenome: sorterGenome,
                    ancestors: parentGenomeEval.Ancestors.Push
                            (
                                sorterGenome.Guid
                            ),
                    sorterEval: sorterEval
                );
        }
    }

    class SorterGenomeEvalImpl : ISorterGenomeEval
    {
        private readonly ISorterGenome _sorterGenome;
        private readonly IImmutableStack<Guid> _ancestors;
        private readonly ISorterEval _sorterEval;

        public SorterGenomeEvalImpl
            (
                ISorterGenome sorterGenome, 
                IImmutableStack<Guid> ancestors, 
                ISorterEval sorterEval
            )
        {
            _sorterGenome = sorterGenome;
            _ancestors = ancestors;
            _sorterEval = sorterEval;
        }

        public ISorterGenome SorterGenome
        {
            get { return _sorterGenome; }
        }

        public IImmutableStack<Guid> Ancestors
        {
            get { return _ancestors; }
        }

        public ISorterEval SorterEval
        {
            get { return _sorterEval; }
        }
    }
}
