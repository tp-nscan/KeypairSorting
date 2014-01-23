using System;
using System.Collections.Immutable;
using Genomic.Genomes;
using SorterEvo.Genomes;
using Sorting.CompetePools;

namespace SorterEvo.Evals
{
    public interface ISorterGenomeEval : IGenomeEval<ISorterGenome>
    {
        ISorterGenome SorterGenome { get; }
        IImmutableStack<int> Ancestors { get; }
        ISorterEval SorterEval { get; }
    }

    public static class SorterGenomeEval
    {
        public static ISorterGenomeEval Make(
                ISorterGenome sorterGenome,
                IImmutableStack<int> ancestors,
                ISorterEval sorterEval,
                int generation
            )
        {
            return new SorterGenomeEvalImpl
                (
                    sorterGenome: sorterGenome,
                    ancestors: ancestors,
                    sorterEval: sorterEval,
                    generation: generation,
                    score: sorterEval.SwitchUseCount
                );
        }

        public static ISorterGenomeEval Make(
            ISorterGenome sorterGenome,
            ISorterGenomeEval parentGenomeEval,
            ISorterEval sorterEval,
            int generation
        )
        {
            return Make
                (
                    sorterGenome: sorterGenome,
                    ancestors: parentGenomeEval.Ancestors.Push
                            (
                                sorterEval.SwitchUseCount
                            ),
                    sorterEval: sorterEval,
                    generation: generation
                );
        }
    }

    class SorterGenomeEvalImpl : ISorterGenomeEval
    {
        private readonly ISorterGenome _sorterGenome;
        private readonly IImmutableStack<int> _ancestors;
        private readonly ISorterEval _sorterEval;
        private readonly int _generation;
        private readonly double _score;

        public SorterGenomeEvalImpl
            (
                ISorterGenome sorterGenome, 
                IImmutableStack<int> ancestors, 
                ISorterEval sorterEval, 
                int generation, 
                double score
            )
        {
            _sorterGenome = sorterGenome;
            _ancestors = ancestors;
            _sorterEval = sorterEval;
            _generation = generation;
            _score = score;
        }

        public ISorterGenome SorterGenome
        {
            get { return _sorterGenome; }
        }

        public IImmutableStack<int> Ancestors
        {
            get { return _ancestors; }
        }

        public ISorterEval SorterEval
        {
            get { return _sorterEval; }
        }

        public Guid Guid
        {
            get { return _sorterGenome.Guid; }
        }

        public int Generation
        {
            get { return _generation; }
        }

        public ISorterGenome Genome
        {
            get { return SorterGenome; }
        }

        public double Score
        {
            get { return _score; }
        }
    }
}
