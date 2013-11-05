using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathUtils.Vector
{
    public static class VectorExt
    {
        public static double Project(this IReadOnlyList<double> target, IReadOnlyList<double> baseline)
        {
            double vRet = 0;
            for (var i = 0; i < target.Count; i++)
            {
                if (baseline[i] > 0)
                {
                    vRet += target[i]/baseline[i];
                }
            }
            return vRet;
        }
    }
}
