using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic
{
    public interface IChromosome
    {
        Guid Guid { get; }
        IReadOnlyList<uint> Sequence { get; } 
        ISymbolSet SymbolSet { get; }
    }

    public interface IUniformChromosome : IChromosome
    {
        new IUniformSymbolSet SymbolSet { get; }
    }

    public static class Chromosome
    {
        public static IUniformChromosome ToUniformChromosome(this IRando rando, Guid guid, uint symbolCount, int sequenceLength)
        {
            return rando.ToUints(symbolCount).Take(sequenceLength)
                .ToUniformChromosome(guid, SymbolSet.MakeUniformSymbolSet(symbolCount));
        }

        public static IChromosome ToChromosome(this IReadOnlyList<uint> sequence, Guid guid, ISymbolSet symbolSet)
        {
            return new ChromosomeImpl(guid, sequence, symbolSet);
        }

        public static IUniformChromosome ToUniformChromosome(this IEnumerable<uint> sequence, Guid guid, IUniformSymbolSet symbolSet)
        {
            return new UniformChromosomeImpl(guid, sequence.ToList(), symbolSet);
        }
        public static IUniformChromosome Copy
            (
                this IUniformChromosome chromosome, 
                Guid newGuid, 
                int seed, 
                double mutationRate, 
                double insertionRate, 
                double deletionRate
            )
        {
            var randy = Rando.Fast(seed);
            var newVals = chromosome.SymbolSet.Choose(randy).ToMoveNext();
            return new UniformChromosomeImpl
                (
                    guid: newGuid, 
                    sequence: chromosome.Sequence.MutateInsertDeleteToList
                                (
                                    doMutation: randy.ToBoolEnumerator(mutationRate),
                                    doInsertion: randy.ToBoolEnumerator(insertionRate),
                                    doDeletion: randy.ToBoolEnumerator(deletionRate),
                                    mutator: x => newVals.Next(),
                                    inserter: x => newVals.Next(),
                                    paddingFunc: x => newVals.Next()
                                ), 
                    symbolSet: chromosome.SymbolSet
                );
        }
    }

    internal class UniformChromosomeImpl : ChromosomeImpl, IUniformChromosome
    {
        public UniformChromosomeImpl(Guid guid, IReadOnlyList<uint> sequence, IUniformSymbolSet symbolSet) 
            : base(guid, sequence, symbolSet)
        {
        }

        public new IUniformSymbolSet SymbolSet
        {
            get { return (IUniformSymbolSet) base.SymbolSet; }
        }
    }

    class ChromosomeImpl : IChromosome
    {
        public ChromosomeImpl(Guid guid, IReadOnlyList<uint> sequence, ISymbolSet symbolSet)
        {
            _guid = guid;
            _sequence = sequence;
            _symbolSet = symbolSet;
        }

        private readonly IReadOnlyList<uint> _sequence;
        public IReadOnlyList<uint> Sequence
        {
            get { return _sequence; }
        }

        private readonly ISymbolSet _symbolSet;
        public ISymbolSet SymbolSet
        {
            get { return _symbolSet; }
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

    }
}