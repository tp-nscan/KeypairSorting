using System;
using System.Collections.Generic;

namespace Evo.GenomeBuilders
{
    public interface IParentIds
    {
        IEnumerable<Guid> ParentIds { get; }
    }

}
