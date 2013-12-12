using System;
using System.Collections.Generic;

namespace Entities
{
    public interface IEntityBuilder<T>
    {
        Guid Guid { get; }
        string EntityBuilderType { get; }
        IReadOnlyDictionary<string, Guid> InputEntities { get; }
        T MakeValue(IEntityRepository repository);
        string OutType { get; }
    }

    public abstract class EntityBuilder<T> : IEntityBuilder<T>
    {
        protected EntityBuilder(
            Guid guid,
            IReadOnlyDictionary<string, Guid> inputEntities
        )
        {
            _inputEntities = inputEntities;
            _guid = guid;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        public abstract string EntityBuilderType
        {
            get;
        }

        private readonly IReadOnlyDictionary<string, Guid> _inputEntities;
        public IReadOnlyDictionary<string, Guid> InputEntities
        {
            get { return _inputEntities; }
        }

        public abstract T MakeValue(IEntityRepository repository);

        public abstract string OutType
        {
            get;
        }
    }
}
