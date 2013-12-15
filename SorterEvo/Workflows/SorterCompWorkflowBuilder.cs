using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Genomic.Layers;
using MathUtils.Collections;
using SorterEvo.Genomes;

namespace SorterEvo.Workflows
{
    public enum EntityBuilderType
    {
        New,
        Update
    }

    public enum SorterCompWorkflowParts
    {
        SorterLayer,
        SwitchableLayer,
        SorterCompPoolParams,
        AllParts
    }

    public interface ISorterCompWorkflowBuilder : IEntityBuilder<ISorterCompWorkflow>
    {
        IEnumerable<int> Seeds { get;}
    }

    public static class SorterCompWorkflowBuilder
    {

        public static ISorterCompWorkflowBuilder Make
            (
                Guid workFlowGuid,
                IEntityRepository repository,
                Guid switchableGroupGuid,
                Guid sorterGroupGuid,
                Guid sorterPoolCompParamsGuid
            )
        {
            var dict = new Dictionary<string, IEntity>();
            dict[SorterCompWorkflowParts.SwitchableLayer.ToString()] = repository.GetEntity(switchableGroupGuid);
            dict[SorterCompWorkflowParts.SorterLayer.ToString()] = repository.GetEntity(sorterGroupGuid);
            dict[SorterCompWorkflowParts.SorterCompPoolParams.ToString()] = repository.GetEntity(sorterPoolCompParamsGuid);


            var entity = Entities.Entity.Make
            (
                guid: workFlowGuid,
                val: SorterCompWorkflow.Make
                (
                    sorterLayer: dict.ToSorterGenome(),
                    switchableGroupLayer: dict.ToSwitchableGroupLayer(),
                    sorterCompPoolParams: dict.ToSorterCompPoolParams()
                )
            );

            return new SorterCompWorkflowBuilderImpl(
                    guid: workFlowGuid,
                    inputEntities : dict,
                    entity: entity,
                    seeds: Enumerable.Empty<int>().ToList()
                );
        }


        public static ISorterCompWorkflowBuilder Update
            (
                ISorterCompWorkflowBuilder builder,
                IReadOnlyList<int> seeds,
                bool mergeWithPrev
            )
        {
               return new SorterCompWorkflowBuilderImpl(
                    guid: builder.Guid.Add(seeds),
                    inputEntities : InputEntityOptions(builder, mergeWithPrev),
                    entity: seeds.Aggregate(
                                        builder.Entity, 
                                        (current, seed) => Entity.Make(
                                                            guid: builder.Guid.Add(seed), 
                                                            val: current.Value.Step(seed)
                                                            )
                                        ),
                    seeds: mergeWithPrev ? builder.Seeds.Concat(seeds).ToList() : seeds.ToList()
                );
        }


        public static ILayer<ISorterGenome> ToSorterGenome(this IReadOnlyDictionary<string, IEntity> dict)
        {
            return (ILayer<ISorterGenome>)dict[SorterCompWorkflowParts.SorterLayer.ToString()].Value;
        }

        public static ILayer<ISwitchableGroupGenome> ToSwitchableGroupLayer(this IReadOnlyDictionary<string, IEntity> dict)
        {
            return (ILayer<ISwitchableGroupGenome>)dict[SorterCompWorkflowParts.SwitchableLayer.ToString()].Value;
        }

        public static SorterCompPoolParams ToSorterCompPoolParams(this IReadOnlyDictionary<string, IEntity> dict)
        {
            return (SorterCompPoolParams)dict[SorterCompWorkflowParts.SorterCompPoolParams.ToString()].Value;
        }


        public static IReadOnlyDictionary<string, IEntity> ToEntityDictionary(this ISorterCompWorkflowBuilder sorterCompWorkflowBuilder)
        {
            var dict = new Dictionary<string, IEntity>();
            dict[SorterCompWorkflowParts.AllParts.ToString()] = sorterCompWorkflowBuilder.Entity;
            return dict;
        }


        public static IReadOnlyDictionary<string, IEntity> InputEntityOptions(ISorterCompWorkflowBuilder builder,
            bool mergeWithPrev)
        {
            return mergeWithPrev
                ? builder.InputEntities
                : builder.ToEntityDictionary();
        }
    }

        public class SorterCompWorkflowBuilderImpl : EntityBuilder<ISorterCompWorkflow>, ISorterCompWorkflowBuilder
    {
        public SorterCompWorkflowBuilderImpl(
                Guid guid,
                IReadOnlyDictionary<string, IEntity> inputEntities,
                IEntity<ISorterCompWorkflow> entity,
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
