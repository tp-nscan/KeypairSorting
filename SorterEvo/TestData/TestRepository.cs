using System;
using Entities;

namespace SorterEvo.TestData
{
    public static class TestRepository
    {
        public static Guid SorterLayerGuid = Guid.NewGuid();
        public static Guid SwitchableGroupLayerGuid = Guid.NewGuid();
        public static Guid SorterPoolCompParamsGuid = Guid.NewGuid();

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
                        val: SorterEvoTestData.SorterLayer())
                );

            entityRepository.AddEntity(
                    Entity.Make(
                        guid: SwitchableGroupLayerGuid, 
                        val: SorterEvoTestData.SwitchableGroupLayer())
                );

            entityRepository.AddEntity(
                    Entity.Make(
                        guid: SorterPoolCompParamsGuid, 
                        val: SorterEvoTestData.SorterPoolCompParams())
                );
        }

    }
}
