using System;
using System.Linq;
using Evo.Genomes;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Evo.GenomeBuilders
{
    public interface IGenomeBuilder<out TG> where TG:IGenome
    {
        Func<TG> BuildGenome { get; }
    }


    public class UniformChromosomeRandomBuilder : IBuilder<IUniformChromosome>
    {
        private readonly int _chromosomeLength;
        private readonly int _symbolCount;
        private readonly int _seed;

        public UniformChromosomeRandomBuilder(
            int chromosomeLength, 
            int symbolCount, 
            int seed)
        {
            _chromosomeLength = chromosomeLength;
            _symbolCount = symbolCount;
            _seed = seed;
        }

        public int ChromosomeLength
        {
            get { return _chromosomeLength; }
        }

        public int SymbolCount
        {
            get { return _symbolCount; }
        }

        public int Seed
        {
            get { return _seed; }
        }

        public IUniformChromosome Buld()
        {
            var symbolSet = BasicSymbolSet.Make(SymbolCount);
            return new UniformChromosomeImpl
                (
                    symbolSet.Choose(Rando.Fast(Seed)).Take(ChromosomeLength),
                    symbolSet
                );
        }
    }

    public class UniformChromosomeCopyBuilder : IBuilder<IUniformChromosome>
    {
        private readonly IUniformChromosome _parentChromosome;
        private readonly double _deletionRate;
        private readonly double _mutationRate;
        private readonly double _insertionRate;
        private readonly int _seed;

        public UniformChromosomeCopyBuilder
            (
                IUniformChromosome parentChromosome, 
                double mutationRate, 
                double insertionRate, 
                double deletionRate, 
                int seed)
        {
            _parentChromosome = parentChromosome;
            _mutationRate = mutationRate;
            _insertionRate = insertionRate;
            _deletionRate = deletionRate;
            _seed = seed;
        }

        public IUniformChromosome ParentChromosome
        {
            get { return _parentChromosome; }
        }

        public double DeletionRate
        {
            get { return _deletionRate; }
        }

        public double MutationRate
        {
            get { return _mutationRate; }
        }

        public double InsertionRate
        {
            get { return _insertionRate; }
        }

        public int Seed
        {
            get { return _seed; }
        }

        public IUniformChromosome Buld()
        {
            var rando = Rando.Fast(Seed);
            return UniformChromosome.MakeUniformChromosome
                (
                    data: ParentChromosome.MutateInsertDelete
                    (
                        doMutation: rando.Spawn().ToBoolEnumerator(MutationRate),
                        doInsertion: rando.Spawn().ToBoolEnumerator(InsertionRate),
                        doDeletion: rando.Spawn().ToBoolEnumerator(DeletionRate),
                        mutator: T => rando.NextInt(ParentChromosome.SymbolSet.Count),
                        inserter: T => rando.NextInt(ParentChromosome.SymbolSet.Count),
                        deleter: () => rando.NextInt(ParentChromosome.SymbolSet.Count)
                    ),
                    symbolSet: ParentChromosome.SymbolSet
                );
        }
    }

}
