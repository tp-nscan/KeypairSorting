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
        IChromosome ReplaceDataWith(IEnumerable<uint> data, Guid newGuid);
    }

    public interface IChromosome<T> : IChromosome
        where T : IChromosomeBlock
    {
        IReadOnlyList<T> Blocks { get; }
    }

    public static class Chromosome
    {
        public static IChromosome<ModNBlock> ToUniformChromosome(
                this IRando rando, Guid guid, uint symbolCount, int sequenceLength)
        {
            return rando.ToUints(symbolCount).Take(sequenceLength)
                .ToUniformChromosome(guid, symbolCount);
        }

        public static IChromosome ToModNChromosome(this IReadOnlyList<uint> sequence, Guid guid, uint maxVal)
        {
            return new ModNChromosome(guid, sequence, maxVal);
        }

        public static IChromosome<ModNBlock> ToUniformChromosome(this IEnumerable<uint> sequence, Guid guid, uint maxVal)
        {
            return new ModNChromosome
                (
                    guid: guid,
                    sequence: sequence.ToList(),
                    maxVal: maxVal
                );
        }

        //public static IChromosome<IChromosomeBlock> Copy
        //(
        //    this IChromosome<IChromosomeBlock> chromosome,
        //    IRando randy,
        //    double mutationRate,
        //    double insertionRate,
        //    double deletionRate
        //)
        //{
        //    return (IChromosome<IChromosomeBlock>)chromosome.ReplaceDataWith(
        //        data: chromosome.Blocks.MutateInsertDelete
        //            (
        //                doMutation: randy.ToBoolEnumerator(mutationRate),
        //                doInsertion: randy.ToBoolEnumerator(insertionRate),
        //                doDeletion: randy.ToBoolEnumerator(deletionRate),
        //                mutator: x => x.Mutate(randy),
        //                inserter: x => x.Mutate(randy),
        //                paddingFunc: x => x.Mutate(randy)
        //            )
        //            .SelectMany(b => b.AsSerialized),
        //        newGuid: Guid.NewGuid()
        //        );
        //}


        public static IChromosome<T> Copy<T>
            (
                this IChromosome<T> chromosome,
                IRando randy,
                double mutationRate,
                double insertionRate,
                double deletionRate
            ) where T : IChromosomeBlock
        {
            return (IChromosome<T>) chromosome.ReplaceDataWith(
                data: chromosome.Blocks.MutateInsertDelete
                    (
                        doMutation: randy.ToBoolEnumerator(mutationRate),
                        doInsertion: randy.ToBoolEnumerator(insertionRate),
                        doDeletion: randy.ToBoolEnumerator(deletionRate),
                        mutator: x => (T)x.Mutate(randy),
                        inserter: x => (T)x.Mutate(randy),
                        paddingFunc: x => (T)x.Mutate(randy)
                    )
                    .SelectMany(b => b.AsSerialized),
                newGuid: Guid.NewGuid()
                );
        }
    }

    abstract class ChromosomeImpl<T> : IChromosome<T> where T : IChromosomeBlock
    {
        protected ChromosomeImpl(Guid guid, IReadOnlyList<uint> sequence)
        {
            _guid = guid;
            _sequence = sequence;
        }

        private readonly IReadOnlyList<uint> _sequence;
        public IReadOnlyList<uint> Sequence
        {
            get { return _sequence; }
        }

        public abstract IChromosome ReplaceDataWith(IEnumerable<uint> data, Guid newGuid);

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public abstract IReadOnlyList<T> Blocks
        {
            get;
        }

    }

    class ModNChromosome : ChromosomeImpl<ModNBlock>
    {
        public ModNChromosome(
            Guid guid, 
            IReadOnlyList<uint> sequence, 
            uint maxVal
            ) : base(guid, sequence)
        {
            _maxVal = maxVal;
        }

        private readonly uint _maxVal;
        public uint MaxVal
        {
            get { return _maxVal; }
        }

        private IReadOnlyList<ModNBlock> _blockList;
        public override IChromosome ReplaceDataWith(IEnumerable<uint> data, Guid newGuid)
        {
            return new ModNChromosome
                (
                    guid: newGuid,
                    sequence: data.ToList(),
                    maxVal: MaxVal
                );
        }

        public override IReadOnlyList<ModNBlock> Blocks
        {
            get
            {
                return _blockList ?? (_blockList = Sequence.Select
                    (
                        t => new ModNBlock(t, MaxVal)).ToList()
                    );
            }
        }
    }

}