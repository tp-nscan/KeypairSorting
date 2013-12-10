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
        Guid OutGuid { get; }
        string OutType { get; }
    }

    public abstract class EntityBuilder<T> : IEntityBuilder<T>
    {
        protected EntityBuilder(
            Guid guid, 
            string entityBuilderType, 
            Guid outGuid, 
            IReadOnlyDictionary<string, Guid> inputEntities, 
            string outType)
        {
            _outGuid = outGuid;
            _inputEntities = inputEntities;
            _outType = outType;
            _guid = guid;
            _entityBuilderType = entityBuilderType;
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        private readonly string _entityBuilderType;
        public string EntityBuilderType
        {
            get { return _entityBuilderType; }
        }

        private readonly IReadOnlyDictionary<string, Guid> _inputEntities;
        public IReadOnlyDictionary<string, Guid> InputEntities
        {
            get { return _inputEntities; }
        }

        public abstract T MakeValue(IEntityRepository repository);

        private readonly Guid _outGuid;
        public Guid OutGuid
        {
            get { return _outGuid; }
        }

        private readonly string _outType;
        public string OutType
        {
            get { return _outType; }
        }
    }
}
