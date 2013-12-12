using System;
using System.Collections.Generic;

namespace Entities
{
    public interface IEntityRepository
    {
        void AddEntity(IEntity entity);
        IEntity GetEntity(Guid id);
        bool ContainsEntity(Guid id);
    }

    public static class EntityRepository
    {
        public static IEntityRepository Make()
        {
            return new EntityRepositoryImpl();
        }
    }

    class EntityRepositoryImpl : IEntityRepository
    {
        readonly Dictionary<Guid, IEntity> _entities = new Dictionary<Guid, IEntity>();
        public void AddEntity(IEntity entity)
        {
            _entities[entity.Guid] = entity;
        }

        public IEntity GetEntity(Guid id)
        {
            return _entities.ContainsKey(id) ? _entities[id] : null;
        }

        public bool ContainsEntity(Guid id)
        {
            return _entities.ContainsKey(id);
        }
    }
}
