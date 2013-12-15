using System;
using System.Collections.Generic;

namespace Entities
{
    public interface IEntityBuilder<T>
    {
        Guid Guid { get; }
        IReadOnlyDictionary<string, IEntity> InputEntities { get; }
        IEntity<T> Entity { get; }
    }

    public abstract class EntityBuilder<T> : IEntityBuilder<T>
    {
        protected EntityBuilder
            (
                Guid guid,
                IReadOnlyDictionary<string, IEntity> inputEntities, 
                IEntity<T> entity
            )
        {
            _inputEntities = inputEntities;
            _guid = guid;
            _entity = entity;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        private readonly IReadOnlyDictionary<string, IEntity> _inputEntities;
        public IReadOnlyDictionary<string, IEntity> InputEntities
        {
            get { return _inputEntities; }
        }

        private readonly IEntity<T> _entity;
        public IEntity<T> Entity
        {
            get { return _entity; }
        }
    }
}
