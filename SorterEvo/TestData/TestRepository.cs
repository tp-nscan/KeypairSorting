using System;
using Entities;

namespace SorterEvo.TestData
{
    public static class TestRepository
    {
        public static Guid SorterLayerGuid = Guid.NewGuid();
        public static Guid SwitchableGroupLayerGuid = Guid.NewGuid();
        public static Guid SorterCompParaPoolParamsGuid = Guid.NewGuid();
        public static Guid SorterCompPoolParamsGuid = Guid.NewGuid();

        private static readonly IEntityRepository entityRepository;
        public static IEntityRepository EntityRepository
        {
            get { return entityRepository; }
        }

        static TestRepository()
        {
            entityRepository = Entities.EntityRepository.Make();

            entityRepository.AddEntity(
                    Entity.Make(
                        guid: SorterLayerGuid, 
                        val: Layers.SorterLayer())
                );

            entityRepository.AddEntity(
                    Entity.Make(
                        guid: SwitchableGroupLayerGuid, 
                        val: Layers.SwitchableGroupLayer())
                );

            entityRepository.AddEntity(
                    Entity.Make(
                        guid: SorterCompParaPoolParamsGuid, 
                        val: Layers.SorterCompParaPoolParams())
                );

            entityRepository.AddEntity(
                    Entity.Make(
                        guid: SorterCompPoolParamsGuid,
                        val: Layers.SorterCompPoolParams())
                );
                    }

    }
}
