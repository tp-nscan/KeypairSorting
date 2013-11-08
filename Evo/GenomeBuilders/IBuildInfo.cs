using System;

namespace Evo.GenomeBuilders
{
    public interface IBuildInfo
    {
        Guid TargetId { get; }
        int Seed { get; }
    }


}