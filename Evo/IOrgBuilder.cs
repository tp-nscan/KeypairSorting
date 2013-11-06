using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Evo.Genomes;

namespace Evo
{
    public interface IOrgBuilder<in TG, out TO>
        where TG : IGenome
        where TO : IOrg<TG,TO>
    {
        Func<TG, TO> BuildOrg { get; }
    }
}
