using System.Collections.Generic;
using System.Linq;

namespace MathUtils.Bits
{
    public static class IntArrayUtils
    {
        public static int CompareDict(this int[] sequence, int[] comp)
        {
            for (int i = sequence.Length - 1; i >= 0; i--)
            {
                if (sequence[i] != comp[i])
                {
                    return sequence[i] > comp[i] ? 1 : -1;
                }
            }
            return 0;
        }

        public static bool IsSorted(this int[] sequence)
        {
            return !sequence.Where((t, i) => i != t).Any();
        }

        public static int ToHash(IReadOnlyList<uint> ints)
        {
            var retVal = ints[0].DeZero(42);
            for (var i = 0; i < ints.Count; i++)
            {
                retVal = (retVal * 397) ^ ints[i].DeZero(42);
            }
            return retVal;
        }


    }
}
