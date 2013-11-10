using System;
using System.Collections.Generic;
using System.Linq;

namespace Evo.GenomeBuilders
{
    public interface IGenomeBuildInfo
    {
        IEnumerable<IChromosomeBuildInfo> ChromosomeBuildInfos { get; } 
        Guid TargetId { get; }
    }

    public interface IGenomeCopyInfo : IGenomeBuildInfo, IParentIds
    {
        new IEnumerable<IChromosomeCopyInfo> ChromosomeBuildInfos { get; } 
    }

    public interface IGenomeGenInfo : IGenomeBuildInfo
    {
        new IEnumerable<IChromosomeGenInfo> ChromosomeBuildInfos { get; } 
    }

    public static class GenomeBuildInfo
    {
        public static IGenomeBuildInfo SingleChromosomeGen
            (
                Guid genomeId,
                Guid chromosomeId,
                int seed,
                int chromosomeLength,
                object symbolSetInit
            )
        {
            return new GenomeGenInfoImpl
                (
                   targetId: genomeId,
                   chromosomeBuildInfos:

                           new[] 
                           { 
                                ChromosomeBuildInfo.GenInfo
                                (
                                    targetId: chromosomeId,
                                    seed: seed,
                                    chromosomeLength: chromosomeLength,
                                    symbolSetInit: symbolSetInit
                                                                            
                                ) 
                           }
                );
        }

        public static IGenomeCopyInfo SingleChromosomeCopy
        (
                Guid targetGenomeId,
                Guid targetChromosomeId,
                Guid parentGenomeId,
                Guid parentChromosomeId,
                int seed,
                double deletionRate,
                double mutationRate,
                double insertionRate
        )
        {
            return new GenomeCopyInfoImpl
            (
               targetId: targetGenomeId,
               chromosomeBuildInfos:

                       new[] 
                           { 
                                ChromosomeBuildInfo.CopyInfo
                                (
                                    targetId: targetChromosomeId,
                                    seed: seed,
                                    parentIds: new[] { parentChromosomeId },
                                    deletionRate: deletionRate,
                                    mutationRate: mutationRate,
                                    insertionRate: insertionRate
                                ) 
                           },

                parentIds: new[] { parentGenomeId }
            );
        }
    }

    class GenomeGenInfoImpl : IGenomeGenInfo
    {
        private readonly IReadOnlyList<IChromosomeGenInfo> _chromosomeBuildInfos;
        private readonly Guid _targetId;

        public GenomeGenInfoImpl(Guid targetId, IEnumerable<IChromosomeGenInfo> chromosomeBuildInfos)
        {
            _targetId = targetId;
            _chromosomeBuildInfos = chromosomeBuildInfos.ToArray();
        }

        public IEnumerable<IChromosomeGenInfo> ChromosomeBuildInfos
        {
            get { return _chromosomeBuildInfos; }
        }

        IEnumerable<IChromosomeBuildInfo> IGenomeBuildInfo.ChromosomeBuildInfos
        {
            get { return ChromosomeBuildInfos; }
        }

        public Guid TargetId
        {
            get { return _targetId; }
        }
    }

    class GenomeCopyInfoImpl : IGenomeCopyInfo
    {
        private readonly IReadOnlyList<IChromosomeCopyInfo> _chromosomeBuildInfos;
        private readonly Guid _targetId;
        private readonly IReadOnlyList<Guid> _parentIds;

        public GenomeCopyInfoImpl(
            Guid targetId, 
            IEnumerable<IChromosomeCopyInfo> chromosomeBuildInfos, 
            IEnumerable<Guid> parentIds)
        {
            _targetId = targetId;
            _parentIds = parentIds.ToList();
            _chromosomeBuildInfos = chromosomeBuildInfos.ToArray();
        }

        public IEnumerable<IChromosomeBuildInfo> ChromosomeBuildInfos
        {
            get { return _chromosomeBuildInfos; }
        }

        IEnumerable<IChromosomeCopyInfo> IGenomeCopyInfo.ChromosomeBuildInfos
        {
            get { return _chromosomeBuildInfos; }
        }

        public Guid TargetId
        {
            get { return _targetId; }
        }

        public IEnumerable<Guid> ParentIds
        {
            get { return _parentIds; }
        }
    }

}
