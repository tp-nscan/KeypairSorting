using System;
using System.Collections.Generic;
using System.Linq;

namespace MathUtils.Rand
{
    public static class RandSort
    {
        public static IEnumerable<T> SubSortShuffle<T>
            (
                this IEnumerable<T> items,
                Func<T, double> orderFunc,
                int randSeed
            )
        {
            var rando = Rando.Fast(randSeed);
            return items.Select(i => new Tuple<T, double>(i, rando.NextDouble()))
                .OrderBy(t => orderFunc(t.Item1))
                .ThenBy(u => u.Item2)
                .Select(v => v.Item1);
        }
    }
}
