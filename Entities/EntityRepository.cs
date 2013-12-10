using System;

namespace Entities
{
    public interface IEntityRepository
    {
        IEntity GetEntity(Guid id);
    }
}
