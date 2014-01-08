using System;
using System.Collections.Immutable;
using SorterEvo.Genomes;
using Sorting.CompetePools;

namespace SorterEvo.Evals
{
    public interface ISorterGenomeEval
    {
        ISorterGenome SorterGenome { get; }
        IImmutableStack<Guid> ParentGuids { get; }
        ISorterEval SorterEval { get; }
    }

    public static class SorterGenomeEval
    {
        public static ISorterGenomeEval Make(
                ISorterGenome sorterGenome,
                IImmutableStack<Guid> parentGuids,
                ISorterEval sorterEval
            )
        {
            return new SorterGenomeEvalImpl
                (
                    sorterGenome: sorterGenome,
                    parentGuids: parentGuids,
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
                    parentGuids: parentGenomeEval.ParentGuids.Push
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
        private readonly IImmutableStack<Guid> _parentGuids;
        private readonly ISorterEval _sorterEval;

        public SorterGenomeEvalImpl
            (
                ISorterGenome sorterGenome, 
                IImmutableStack<Guid> parentGuids, 
                ISorterEval sorterEval
            )
        {
            _sorterGenome = sorterGenome;
            _parentGuids = parentGuids;
            _sorterEval = sorterEval;
        }

        public ISorterGenome SorterGenome
        {
            get { return _sorterGenome; }
        }

        public IImmutableStack<Guid> ParentGuids
        {
            get { return _parentGuids; }
        }

        public ISorterEval SorterEval
        {
            get { return _sorterEval; }
        }
    }
}
