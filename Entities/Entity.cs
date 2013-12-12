using System;
using System.Collections.Generic;

namespace Entities
{
    public interface IEntity
    {
        Guid Guid { get; }
        string EntityType { get; }
        object Value { get; }
    }

    public interface IEntity<T> : IEntity
    {
        new T Value { get; }
    }

    public static class Entity
    {
        public static IEntity<T> FromBuilder<T>(this IEntityBuilder<T> entityBuilder, IEntityRepository repository)
        {
            return new EntityImpl<T>
                (
                   guid: entityBuilder.Guid, 
                   entityType: entityBuilder.OutType,
                   value: entityBuilder.MakeValue(repository)
                );
        }

        public static IEntity<int> NumberEntity(Guid guid, int val, Guid entityBuilderGuid)
        {
            return new NumberEntity(guid, val);
        }
    }

    internal class EntityImpl<T> : IEntity<T>
    {
        public EntityImpl(Guid guid, string entityType, T value)
        {
            _guid = guid;
            _value = value;
            _entityType = entityType;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        private readonly string _entityType;
        public string EntityType
        {
            get { return _entityType; }
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

    internal class NumberEntity : EntityImpl<int>
    {
        public NumberEntity(Guid guid, int value)
            : base(
                guid: guid, 
                entityType:"NumberEntity", 
                value: value
            )
        {
        }
    }

    internal class NumberListEntity : EntityImpl<IEntity<IReadOnlyList<int>>>
    {
        public NumberListEntity(Guid guid, Guid sourceGuid)
            : base
            (
                guid: guid, 
                entityType: "NumberListEntity",
                value: FromGuid(guid, sourceGuid)
            )
        {
        }

        public static IEntity<IReadOnlyList<int>> FromGuid(Guid guid, Guid sourceGuid)
        {
            return null;
        }
    }


}
