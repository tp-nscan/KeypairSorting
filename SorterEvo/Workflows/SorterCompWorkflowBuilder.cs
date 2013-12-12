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
        SorterCompPoolParams
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

            return new SorterCompWorkflowBuilderNew(
                                guid: workFlowGuid, 
                                inputEntities: dict
                            );
        }

        public static ISorterCompWorkflowBuilder Update
            (
                ISorterCompWorkflowBuilder builder,
                IReadOnlyList<int> seeds
            )
        {

            return new SorterCompWorkflowBuilderUpdate(
                                builder: builder,
                                seeds: seeds
                            );
        }
    }

    class SorterCompWorkflowBuilderNew : EntityBuilder<ISorterCompWorkflow>, ISorterCompWorkflowBuilder
    {
        public SorterCompWorkflowBuilderNew(
            Guid guid, 
            IReadOnlyDictionary<string, IEntity> inputEntities
            ) : base(guid, inputEntities)
        {
            _entity = Entities.Entity.Make
                (
                    guid: guid, 
                    val: SorterCompWorkflow.Make
                        (
                            sorterLayer: SorterGenomeLayer,
                            switchableGroupLayer: SwitchableGroupLayer,
                            sorterCompPoolParams: SorterCompPoolParams
                        )
                );

            _seeds = new List<int>();
        }

        private readonly IEntity<ISorterCompWorkflow> _entity;
        public override IEntity<ISorterCompWorkflow> Entity
        {
            get { return _entity; }
        }

        ILayer<ISorterGenome> SorterGenomeLayer
        {
            get
            {
                return (ILayer<ISorterGenome>)InputEntities[SorterCompWorkflowParts.SorterLayer.ToString()].Value;
            }
        }

        ILayer<ISwitchableGroupGenome> SwitchableGroupLayer
        {
            get
            {
                return (ILayer<ISwitchableGroupGenome>)InputEntities[SorterCompWorkflowParts.SwitchableLayer.ToString()].Value;
            }
        }

        SorterCompPoolParams SorterCompPoolParams
        {
            get
            {
                return (SorterCompPoolParams)InputEntities[SorterCompWorkflowParts.SorterCompPoolParams.ToString()].Value;
            }
        }

        private readonly IReadOnlyList<int> _seeds;
        public IEnumerable<int> Seeds
        {
            get { return _seeds; }
        }
    }

    public class SorterCompWorkflowBuilderUpdate : EntityBuilder<ISorterCompWorkflow>, ISorterCompWorkflowBuilder
    {
        public SorterCompWorkflowBuilderUpdate(
                ISorterCompWorkflowBuilder builder,
                IReadOnlyList<int> seeds
            )
            : base(builder.Guid.Add(seeds), builder.InputEntities)
        {
            _seeds = builder.Seeds.Concat(seeds).ToList();

            foreach (var seed in seeds)
            {
                _entity = Entities.Entity.Make
                    (
                        guid: builder.Guid.Add(seed),
                        val: builder.Entity.Value.Step(seed)
                    );
            }
        }

        private readonly IEntity<ISorterCompWorkflow> _entity;
        public override IEntity<ISorterCompWorkflow> Entity
        {
            get { return _entity; }
        }

        private readonly IReadOnlyList<int> _seeds;
        public IEnumerable<int> Seeds
        {
            get { return _seeds; }
        }
    }
}
