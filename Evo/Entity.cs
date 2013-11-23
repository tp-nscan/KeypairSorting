using System;
using System.Collections.Generic;

namespace Evo
{
    public interface IRepositoryFunction<TS, out T>
        where T : class 
    {
        string Key { get; }
        Func<TS, T> Function { get; }
    }

    public interface IEntity
    {
        string EntityType { get; }
        Guid Guid { get; }
        IReadOnlyDictionary<string, IEntity> InputEntities { get; }
    }

    public interface IEntity<TS, out T> : IEntity
        where T : class 
    {
        TS Input { get; }
        IRepositoryFunction<TS, T> RepositoryFunction { get; }
        T Value { get; }
    }

    public static class Entity
    {
        
    }

    abstract class EntityImpl<TS,T> : IEntity<TS,T> where T : class 
    {
        protected EntityImpl(Guid guid,
                          string entityType,
                          IReadOnlyDictionary<string, IEntity> inputEntities,
                          IRepositoryFunction<TS,T> repositoryFunction, 
                          T value)
        {
            _guid = guid;
            _entityType = entityType;
            _repositoryFunction = repositoryFunction;
            _value = value;
            _inputEntities = inputEntities;
            
        }

        private readonly IReadOnlyDictionary<string, IEntity> _inputEntities;
        public IReadOnlyDictionary<string, IEntity> InputEntities
        {
            get { return _inputEntities; }
        }

        private readonly string _entityType;
        public string EntityType
        {
            get { return _entityType; }
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public abstract TS Input
        {
            get;
        }

        private readonly IRepositoryFunction<TS, T> _repositoryFunction;
        public IRepositoryFunction<TS, T> RepositoryFunction
        {
            get { return _repositoryFunction; }
        }

        private T _value;
        public T Value
        {
            get { return _value ?? (_value = RepositoryFunction.Function(Input)); }
        }
    }
}