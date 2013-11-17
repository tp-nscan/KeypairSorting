using System.Collections.Generic;
using MathUtils.Rand;

namespace Genomic.Genes
{
    public interface IGene
    {
        IEnumerable<uint> AsSerialized { get; }
        IGene Mutate(IRando rando);
    }
}
