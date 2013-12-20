using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Genomic.Layers;
using MathUtils.Collections;
using SorterEvo.Genomes;
using SorterEvo.Trackers;

namespace SorterEvo.Workflows
{
    public enum SorterCompParaPoolWorkflowParts
    {
        SorterLayer,
        SwitchableLayer,
        SorterCompParaPoolParams,
        AllParts
    }

    public interface ISorterCompParaPoolWorkflowBuilder : IEntityBuilder<ISorterCompParaPoolWorkflow>
    {
        IEnumerable<int> Seeds { get;}
    }

    public static class SorterCompParaPoolWorkflowBuilder
    {

        public static ISorterCompParaPoolWorkflowBuilder Make
            (
                Guid workFlowGuid,
                IEntityRepository repository,
                Guid switchableGroupGuid,
                Guid sorterGroupGuid,
                Guid sorterCompPoolParamsGuid
            )
        {
            var dict = new Dictionary<string, IEntity>();
            dict[SorterCompParaPoolWorkflowParts.SwitchableLayer.ToString()] = repository.GetEntity(switchableGroupGuid);
            dict[SorterCompParaPoolWorkflowParts.SorterLayer.ToString()] = repository.GetEntity(sorterGroupGuid);
            dict[SorterCompParaPoolWorkflowParts.SorterCompParaPoolParams.ToString()] = repository.GetEntity(sorterCompPoolParamsGuid);


            var entity = Entity.Make
            (
                guid: workFlowGuid,
                val: SorterCompParaPoolWorkflow.Make
                (
                    sorterLayer: dict.ToSorterGenome(),
                    switchableGroupLayer: dict.ToSwitchableGroupLayer(),
                    sorterCompParaPoolParams: dict.ToSorterCompParaPoolParams(),
                    generation: 0
                )
            );

            return new SorterCompParaPoolWorkflowBuilderImpl(
                    guid: workFlowGuid,
                    inputEntities : dict,
                    entity: entity,
                    seeds: Enumerable.Empty<int>().ToList()
                );
        }


        public static ISorterCompParaPoolWorkflowBuilder Update
            (
                ISorterCompParaPoolWorkflowBuilder builder,
                IReadOnlyList<int> seeds,
                bool mergeWithPrev
            )
        {
               return new SorterCompParaPoolWorkflowBuilderImpl(
                    guid: builder.Guid.Add(seeds),
                    inputEntities : InputEntityOptions(builder, mergeWithPrev),
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

        public static ISorterCompParaPoolWorkflowBuilder UpdateAndTrack
        (
            ISorterCompParaPoolWorkflowBuilder builder,
            IReadOnlyList<int> seeds,
            bool mergeWithPrev,
            ISorterCompParaPoolWorkflowTracker tracker
        )
        {
            IEntity<ISorterCompParaPoolWorkflow> curWorkflowEntity = builder.Entity;
            foreach (var seed in seeds)
            {
                tracker.TrackItem(curWorkflowEntity.Value);
                curWorkflowEntity = Entity.Make
                    (
                        guid: curWorkflowEntity.Guid.Add(seed),
                        val: curWorkflowEntity.Value.Step(seed)
                    );
            }

            return new SorterCompParaPoolWorkflowBuilderImpl(
                 guid: builder.Guid.Add(seeds),
                 inputEntities: InputEntityOptions(builder, mergeWithPrev),
                 entity: curWorkflowEntity,
                 seeds: mergeWithPrev ? builder.Seeds.Concat(seeds).ToList() : seeds.ToList()
            );
        }

        public static ILayer<ISorterGenome> ToSorterGenome(this IReadOnlyDictionary<string, IEntity> dict)
        {
            return (ILayer<ISorterGenome>)dict[SorterCompParaPoolWorkflowParts.SorterLayer.ToString()].Value;
        }

        public static ILayer<ISwitchableGroupGenome> ToSwitchableGroupLayer(this IReadOnlyDictionary<string, IEntity> dict)
        {
            return (ILayer<ISwitchableGroupGenome>)dict[SorterCompParaPoolWorkflowParts.SwitchableLayer.ToString()].Value;
        }

        public static SorterCompParaPoolParams ToSorterCompParaPoolParams(this IReadOnlyDictionary<string, IEntity> dict)
        {
            return (SorterCompParaPoolParams)dict[SorterCompParaPoolWorkflowParts.SorterCompParaPoolParams.ToString()].Value;
        }


        public static IReadOnlyDictionary<string, IEntity> ToEntityDictionary(this ISorterCompParaPoolWorkflowBuilder sorterCompParaPoolWorkflowBuilder)
        {
            var dict = new Dictionary<string, IEntity>();
            dict[SorterCompParaPoolWorkflowParts.AllParts.ToString()] = sorterCompParaPoolWorkflowBuilder.Entity;
            return dict;
        }


        public static IReadOnlyDictionary<string, IEntity> InputEntityOptions(ISorterCompParaPoolWorkflowBuilder builder,
            bool mergeWithPrev)
        {
            return mergeWithPrev
                ? builder.InputEntities
                : builder.ToEntityDictionary();
        }
    }

    public class SorterCompParaPoolWorkflowBuilderImpl : EntityBuilder<ISorterCompParaPoolWorkflow>, ISorterCompParaPoolWorkflowBuilder
    {
        public SorterCompParaPoolWorkflowBuilderImpl(
                Guid guid,
                IReadOnlyDictionary<string, IEntity> inputEntities,
                IEntity<ISorterCompParaPoolWorkflow> entity,
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
