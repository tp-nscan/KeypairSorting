using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Genomic.Trackers;
using MathUtils.Collections;
using SorterEvo.Trackers;
using Sorting.CompetePools;

namespace SorterEvo.Workflows
{
    public enum SorterCompPoolWorkflowParts
    {
        SorterLayer,
        SorterCompPoolParams,
        AllParts
    }

    public interface ISorterCompPoolWorkflowBuilder : IEntityBuilder<ISorterCompPoolWorkflow>
    {
        IEnumerable<int> Seeds { get; }
    }

    public static class SorterCompPoolWorkflowBuilder
    {
        public static ISorterCompPoolWorkflowBuilder Make
        (
            Guid workFlowGuid,
            IEntityRepository repository,
            Guid sorterGroupGuid,
            Guid sorterCompPoolParamsGuid
        )
        {
            var dict = new Dictionary<string, IEntity>();
            dict[SorterCompPoolWorkflowParts.SorterLayer.ToString()] = repository.GetEntity(sorterGroupGuid);
            dict[SorterCompPoolWorkflowParts.SorterCompPoolParams.ToString()] = repository.GetEntity(sorterCompPoolParamsGuid);


            var entity = Entity.Make
            (
                guid: workFlowGuid,
                val: SorterCompPoolWorkflow.Make
                (
                    sorterLayer: dict.ToSorterGenome(),
                    sorterCompPoolParams: dict.ToSorterCompPoolParams(),
                    generation: 0
                )
            );

            return new SorterCompPoolWorkflowBuilderImpl(
                    guid: workFlowGuid,
                    inputEntities: dict,
                    entity: entity,
                    seeds: Enumerable.Empty<int>().ToList()
                );
        }


        public static ISorterCompPoolWorkflowBuilder Update
            (
                ISorterCompPoolWorkflowBuilder builder,
                IReadOnlyList<int> seeds,
                bool mergeWithPrev
            )
        {
            return new SorterCompPoolWorkflowBuilderImpl(
                 guid: builder.Guid.Add(seeds),
                 inputEntities: InputEntityOptions(builder, mergeWithPrev),
                 entity: seeds.Aggregate
                               (
                                 builder.Entity,
                                   (current, seed) =>
                                   {
                                       var newWorkflow = Entity.Make(
                                               guid: builder.Guid.Add(seed),
                                               val: current.Value.Step(seed)
                                               );
                                       return newWorkflow;
                                   }
                     ),
                 seeds: mergeWithPrev ? builder.Seeds.Concat(seeds).ToList() : seeds.ToList()
             );
        }

        public static ISorterCompPoolWorkflowTracker Trim(this ISorterCompPoolWorkflowTracker tracker, int count)
        {
            return new SorterCompPoolWorkflowTrackerImpl(
                    sorterPoolStats: GenomePoolStats.Make(tracker.SorterPoolStats.GenomeStatses.OrderBy(t => ((ISorterEval)t.ReferenceResult).SwitchesUsed).Take(count))
                );
        }

        public static ISorterCompPoolWorkflowBuilder UpdateAndTrack
        (
            ISorterCompPoolWorkflowBuilder builder,
            IReadOnlyList<int> seeds,
            bool mergeWithPrev,
            ISorterCompPoolWorkflowTracker tracker
        )
        {
            IEntity<ISorterCompPoolWorkflow> curWorkflowEntity = builder.Entity;
            foreach (var seed in seeds)
            {
                tracker.TrackItem(curWorkflowEntity.Value);
                curWorkflowEntity = Entity.Make
                    (
                        guid: curWorkflowEntity.Guid.Add(seed),
                        val: curWorkflowEntity.Value.Step(seed)
                    );
            }

            return new SorterCompPoolWorkflowBuilderImpl(
                 guid: builder.Guid.Add(seeds),
                 inputEntities: InputEntityOptions(builder, mergeWithPrev),
                 entity: curWorkflowEntity,
                 seeds: mergeWithPrev ? builder.Seeds.Concat(seeds).ToList() : seeds.ToList()
            );
        }


        public static ISorterCompPoolParams ToSorterCompPoolParams(this IReadOnlyDictionary<string, IEntity> dict)
        {
            return (ISorterCompPoolParams)dict[SorterCompPoolWorkflowParts.SorterCompPoolParams.ToString()].Value;
        }

        public static IReadOnlyDictionary<string, IEntity> ToEntityDictionary(this ISorterCompPoolWorkflowBuilder sorterCompParaPoolWorkflowBuilder)
        {
            var dict = new Dictionary<string, IEntity>();
            dict[SorterCompParaPoolWorkflowParts.AllParts.ToString()] = sorterCompParaPoolWorkflowBuilder.Entity;
            return dict;
        }


        public static IReadOnlyDictionary<string, IEntity> InputEntityOptions(ISorterCompPoolWorkflowBuilder builder,
            bool mergeWithPrev)
        {
            return mergeWithPrev ?
                  builder.InputEntities
                : builder.ToEntityDictionary();
        }

    }

    class SorterCompPoolWorkflowBuilderImpl : EntityBuilder<ISorterCompPoolWorkflow>, ISorterCompPoolWorkflowBuilder
    {
        public SorterCompPoolWorkflowBuilderImpl(
                Guid guid,
                IReadOnlyDictionary<string, IEntity> inputEntities,
                IEntity<ISorterCompPoolWorkflow> entity,
                IReadOnlyList<int> seeds
            )
            : base
                (
                    guid: guid,
                    inputEntities: inputEntities,
                    entity: entity
                )
        {
            _seeds = seeds;
        }

        private readonly IReadOnlyList<int> _seeds;
        public IEnumerable<int> Seeds
        {
            get { return _seeds; }
        }
    }
}
