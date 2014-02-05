using System;
using System.Collections.Immutable;
using Genomic.Genomes;
using SorterEvo.Genomes;
using Sorting.CompetePools;
using Sorting.Sorters;

namespace SorterEvo.Evals
{
    public interface ISorterGenomeEval : IGenomeEval<ISorterGenome>
    {
        ISorterGenome SorterGenome { get; }
        IImmutableStack<int> Ancestors { get; }
        ISorterEval SorterEval { get; }
        IStagedSorter StagedSorter { get; }
    }

    public static class SorterGenomeEval
    {
        public static ISorterGenomeEval Make(
                ISorterGenome sorterGenome,
                IImmutableStack<int> ancestors,
                ISorterEval sorterEval,
                int generation,
                bool success
            )
        {
            return new SorterGenomeEvalImpl
                (
                    sorterGenome: sorterGenome,
                    ancestors: ancestors,
                    sorterEval: sorterEval,
                    generation: generation,
                    score: sorterEval.SwitchUseCount,
                    success: success
                );
        }

        public static ISorterGenomeEval Make(
            ISorterGenome sorterGenome,
            ISorterGenomeEval parentGenomeEval,
            ISorterEval sorterEval,
            int generation,
                bool success
        )
        {
            return Make
                (
                    sorterGenome: sorterGenome,
                    ancestors: (parentGenomeEval==null) ? ImmutableStack<int>.Empty : 
                            parentGenomeEval.Ancestors.Push
                            (
                                sorterEval.SwitchUseCount
                            ),
                    sorterEval: sorterEval,
                    generation: generation,
                    success: success
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
        private readonly IStagedSorter _stagedSorter;
        private readonly bool _success;

        public SorterGenomeEvalImpl
            (
                ISorterGenome sorterGenome, 
                IImmutableStack<int> ancestors, 
                ISorterEval sorterEval, 
                int generation, 
                double score, 
                bool success
            )
        {
            _sorterGenome = sorterGenome;
            _ancestors = ancestors;
            _sorterEval = sorterEval;
            _generation = generation;
            _score = score;
            _success = success;
            _stagedSorter = sorterEval.Reduce(Guid.NewGuid()).ToStagedSorter();
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

        public IStagedSorter StagedSorter
        {
            get { return _stagedSorter; }
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

        public bool Success
        {
            get { return _success; }
        }
    }
}
