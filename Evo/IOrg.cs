using System;
using Evo.GenomeBuilders;
using Evo.Genomes;

namespace Evo
{
    public interface IOrg<out TG, out TO> where TG : IGenome where TO : IOrg<TG, TO>
    {
        Guid Guid { get; }

        TG Genome { get; }
        IBuilder<TG> GenomeBuilder { get; }
        IBuilder<TO> OrgBuilder { get; }
    }

}
