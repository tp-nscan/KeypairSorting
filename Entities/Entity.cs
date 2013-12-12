using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Entities
{
    public interface IEntity
    {
        Guid Guid { get; }
        Type EntityType { get; }
        object Value { get; }
    }

    public interface IEntity<T> : IEntity
    {
        new T Value { get; }
    }

    public static class Entity
    {
        public static IReadOnlyDictionary<string, IEntity> ToLocalNames<T>(this IEntityRepository repository,
                                                     IReadOnlyDictionary<string, Guid> entityKeys)
        {
            var entityDictionary = new Dictionary<string, IEntity>();
            foreach (var keyValuePair in entityKeys)
            {
                if (! repository.ContainsEntity(keyValuePair.Value))
                {
                    throw new Exception(String.Format("Entity with guid {0} and local name {1} not found in repository", keyValuePair.Value, keyValuePair.Key));
                }
                entityDictionary[keyValuePair.Key] = repository.GetEntity(keyValuePair.Value);
            }
            return new ReadOnlyDictionary<string, IEntity>(entityDictionary);
        }

        public static IEntity<T> Make<T>(Guid guid, T val)
        {
            return new EntityImpl<T>(guid, val);
        }
    }

    internal class EntityImpl<T> : IEntity<T>
    {
        public EntityImpl(Guid guid, T value)
        {
            _guid = guid;
            _value = value;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public Type EntityType
        {
            get { return typeof(T); }
        }

        object IEntity.Value
        {
            get { return Value; }
        }

        private readonly T _value;
        public T Value
        {
            get { return _value; }
        }
    }
}
