using System;
using System.Collections.Generic;

namespace Evo
{
    public interface ISimpleGenomeEntity
    {
        
    }

    public class SimpleGenomeRandGenParams
    {
        public int Seed { get; set; }

    }

    public class ChromosomeRandGenParams
    {
        public int Seed { get; set; }

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
        IEntityFunction<TS, T> EntityFunction { get; }
        T Value { get; }
    }

    public static class Entity
    {
        
    }

    abstract class EntityImpl<TS,T> : IEntity<TS,T> where T : class 
    {
        protected EntityImpl( Guid guid,
                              string entityType,
                              IReadOnlyDictionary<string, IEntity> inputEntities,
                              IEntityFunction<TS,T> entityFunction, 
                              T value)
        {
            _guid = guid;
            _entityType = entityType;
            _entityFunction = entityFunction;
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

        private readonly IEntityFunction<TS, T> _entityFunction;
        public IEntityFunction<TS, T> EntityFunction
        {
            get { return _entityFunction; }
        }

        private T _value;
        public T Value
        {
            get { return _value ?? (_value = EntityFunction.Function(Input)); }
        }
    }
}