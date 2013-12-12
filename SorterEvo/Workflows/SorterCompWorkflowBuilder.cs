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

    }

    public static class SorterCompWorkflowBuilder
    {
        public static ISorterCompWorkflowBuilder Make
            (
                ILayer<ISorterGenome> sorterLayer,
                ILayer<ISwitchableGroupGenome> switchableGroupLayer,
                SorterCompPoolParams sorterPoolCompParams,
                IEnumerable<int> seeds 
            )
        {
            return null;
        }
    }

    public class SorterCompWorkflowBuilderNew : EntityBuilder<ISorterCompWorkflow>, ISorterCompWorkflowBuilder
    {
        public SorterCompWorkflowBuilderNew(
                Guid guid,
                IReadOnlyDictionary<string, Guid> inputEntities,
                IEnumerable<int> seeds 
            )
            : base(guid, inputEntities)
        {
            _seeds = seeds.ToList();
        }

        public override string EntityBuilderType
        {
            get { return ""; }
        }

        public override ISorterCompWorkflow MakeValue(IEntityRepository repository)
        {
            var sorterLayer = (ILayer<ISorterGenome>)repository.GetEntity(InputEntities[SorterCompWorkflowParts.SorterLayer.ToString()]).Value;
            var switchableGroupLayer = (ILayer<ISwitchableGroupGenome>)repository.GetEntity(InputEntities[SorterCompWorkflowParts.SwitchableLayer.ToString()]).Value;
            var sorterCompPoolParams = (SorterCompPoolParams)repository.GetEntity(InputEntities[SorterCompWorkflowParts.SorterCompPoolParams.ToString()]).Value;

            return SorterCompWorkflow.Make
                (
                    sorterLayer:sorterLayer,
                    switchableGroupLayer:switchableGroupLayer,
                    sorterCompPoolParams: sorterCompPoolParams
                );
        }

        public override string OutType
        {
            get { return ""; }
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
            _seeds = seeds.ToList();
        }

        public override string EntityBuilderType
        {
            get { return Workflows.EntityBuilderType.Update.ToString(); }
        }

        public override ISorterCompWorkflow MakeValue(IEntityRepository repository)
        {
            return null;
        }

        public override string OutType
        {
            get { return ""; }
        }

        private readonly IReadOnlyList<int> _seeds;
        public IEnumerable<int> Seeds
        {
            get { return _seeds; }
        }
    }
}
