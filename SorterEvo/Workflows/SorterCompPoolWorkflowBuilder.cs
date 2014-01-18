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
    public enum ScpWorkflowParts
    {
        SorterLayer,
        ScpParams,
        AllParts
    }

    public interface IScpWorkflowBuilder : IEntityBuilder<IScpWorkflow>
    {
        IEnumerable<int> Seeds { get; }
    }

    public static class ScpWorkflowBuilder
    {
        public static IScpWorkflowBuilder Make
        (
            Guid workFlowGuid,
            IEntityRepository repository,
            Guid sorterGroupGuid,
            Guid scpParamsGuid
        )
        {
            var dict = new Dictionary<string, IEntity>();
            dict[ScpWorkflowParts.SorterLayer.ToString()] = repository.GetEntity(sorterGroupGuid);
            dict[ScpWorkflowParts.ScpParams.ToString()] = repository.GetEntity(scpParamsGuid);


            var entity = Entity.Make
            (
                guid: workFlowGuid,
                val: ScpWorkflow.Make
                (
                    sorterLayer: dict.ToSorterGenome(),
                    scpParams: dict.ToScpParams(),
                    generation: 0
                )
            );

            return new ScpWorkflowBuilderImpl(
                    guid: workFlowGuid,
                    inputEntities: dict,
                    entity: entity,
                    seeds: Enumerable.Empty<int>().ToList()
                );
        }


        public static IScpWorkflowBuilder Update
            (
                IScpWorkflowBuilder builder,
                IReadOnlyList<int> seeds,
                bool mergeWithPrev
            )
        {
            return new ScpWorkflowBuilderImpl(
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

        public static IScpWorkflowTracker Trim(this IScpWorkflowTracker tracker, int count)
        {
            return new ScpWorkflowTrackerImpl(
                    sorterPoolStats: GenomePoolStats.Make(tracker.SorterPoolStats.GenomeStatses.OrderBy(t => ((ISorterEval)t.ReferenceResult).SwitchUseCount).Take(count))
                );
        }

        public static IScpWorkflowBuilder UpdateAndTrack
        (
            IScpWorkflowBuilder builder,
            IReadOnlyList<int> seeds,
            bool mergeWithPrev,
            IScpWorkflowTracker tracker
        )
        {
            IEntity<IScpWorkflow> curWorkflowEntity = builder.Entity;
            foreach (var seed in seeds)
            {
                tracker.TrackItem(curWorkflowEntity.Value);
                curWorkflowEntity = Entity.Make
                    (
                        guid: curWorkflowEntity.Guid.Add(seed),
                        val: curWorkflowEntity.Value.Step(seed)
                    );
            }

            return new ScpWorkflowBuilderImpl(
                 guid: builder.Guid.Add(seeds),
                 inputEntities: InputEntityOptions(builder, mergeWithPrev),
                 entity: curWorkflowEntity,
                 seeds: mergeWithPrev ? builder.Seeds.Concat(seeds).ToList() : seeds.ToList()
            );
        }


        public static IScpParams ToScpParams(this IReadOnlyDictionary<string, IEntity> dict)
        {
            return (IScpParams)dict[ScpWorkflowParts.ScpParams.ToString()].Value;
        }

        public static IReadOnlyDictionary<string, IEntity> ToEntityDictionary(this IScpWorkflowBuilder sorterCompParaPoolWorkflowBuilder)
        {
            var dict = new Dictionary<string, IEntity>();
            dict[SorterCompParaPoolWorkflowParts.AllParts.ToString()] = sorterCompParaPoolWorkflowBuilder.Entity;
            return dict;
        }


        public static IReadOnlyDictionary<string, IEntity> InputEntityOptions(IScpWorkflowBuilder builder,
            bool mergeWithPrev)
        {
            return mergeWithPrev ?
                  builder.InputEntities
                : builder.ToEntityDictionary();
        }

    }

    class ScpWorkflowBuilderImpl : EntityBuilder<IScpWorkflow>, IScpWorkflowBuilder
    {
        public ScpWorkflowBuilderImpl(
                Guid guid,
                IReadOnlyDictionary<string, IEntity> inputEntities,
                IEntity<IScpWorkflow> entity,
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
