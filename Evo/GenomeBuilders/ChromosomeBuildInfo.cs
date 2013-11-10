using System;
using System.Collections.Generic;
using System.Linq;

namespace Evo.GenomeBuilders
{
    public interface IChromosomeBuildInfo : IBuildInfo
    {
    }

    public interface IChromosomeGenInfo : IChromosomeBuildInfo
    {
        int ChromosomeLength { get; }
        object Initializer { get; }
    }

    public interface IChromosomeCopyInfo : IChromosomeBuildInfo, IParentIds
    {
        double DeletionRate { get; }
        double MutationRate { get; }
        double InsertionRate { get; }
    }

    public static class ChromosomeBuildInfo
    {
        public static IChromosomeGenInfo GenInfo(
            Guid targetId,
            int seed,
            int chromosomeLength,
            object symbolSetInit
            )
        {
            return new ChromosomeGenInfoImpl
                (
                    targetId: targetId,
                    seed: seed,
                    chromosomeLength: chromosomeLength,
                    initializer: symbolSetInit
                );
        }

        public static IChromosomeCopyInfo CopyInfo(
                Guid targetId,
                int seed,
                IEnumerable<Guid> parentIds,
                double deletionRate,
                double mutationRate,
                double insertionRate
            )
        {
            return new ChromosomeCopyInfoImpl
                (
                    targetId: targetId,
                    seed: seed,
                    parentIds: parentIds,
                    deletionRate: deletionRate,
                    mutationRate: mutationRate,
                    insertionRate: insertionRate
                );
        }

    }

    abstract class ChromosomeBuildInfoImpl : IChromosomeBuildInfo
    {
        protected ChromosomeBuildInfoImpl(Guid targetId, int seed)
        {
            _seed = seed;
            _targetId = targetId;
        }

        private readonly Guid _targetId;
        private readonly int _seed;

        public Guid TargetId
        {
            get { return _targetId; }
        }

        public int Seed
        {
            get { return _seed; }
        }
    }

    class ChromosomeCopyInfoImpl : ChromosomeBuildInfoImpl, IChromosomeCopyInfo
    {
        private readonly IReadOnlyList<Guid> _parentIds;
        private readonly double _deletionRate;
        private readonly double _mutationRate;
        private readonly double _insertionRate;

        public ChromosomeCopyInfoImpl
            (
                Guid targetId, 
                int seed, 
                IEnumerable<Guid> parentIds, 
                double deletionRate, 
                double mutationRate, 
                double insertionRate) : base(targetId, seed)
        {
            _parentIds = parentIds.ToList();
            _deletionRate = deletionRate;
            _mutationRate = mutationRate;
            _insertionRate = insertionRate;
        }

        public IEnumerable<Guid> ParentIds
        {
            get { return _parentIds; }
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
    }

    class ChromosomeGenInfoImpl : ChromosomeBuildInfoImpl, IChromosomeGenInfo
    {
        private readonly int _chromosomeLength;
        private readonly object _initializer;

        public ChromosomeGenInfoImpl(
            Guid targetId, 
            int seed, 
            int chromosomeLength,
            object initializer)
            : base(targetId, seed)
        {
            _chromosomeLength = chromosomeLength;
            _initializer = initializer;
        }

        public int ChromosomeLength
        {
            get { return _chromosomeLength; }
        }

        public object Initializer
        {
            get { return _initializer; }
        }
    }
}
