using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathUtils.Collections
{
    public interface IGuid
    {
        Guid Guid { get; }
    }

    public static class GuidExt
    {
        public static IEnumerable<Guid> NewGuids()
        {
            while (true)
            {
                yield return Guid.NewGuid();
            }
        }
    }
}
