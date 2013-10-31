using System;
using System.Collections.Generic;
using System.Linq;

namespace MathUtils.Bits
{
    public static class BitArrayUtils
    {
        public static int CompareDict(this bool[] bits, bool[] comp)
        {
            for (int i = bits.Length - 1; i >= 0; i--)
            {
                if (bits[i] != comp[i])
                {
                    return bits[i] ? 1 : -1;
                }
            }
            return 0;
        }

        public static bool IsSorted(this bool[] bits)
        {
            var foundAFalse = false;
            for (var i = bits.Length - 1; i >= 0; i--)
            {
                if (bits[i])
                {
                    if (foundAFalse)
                    {
                        return false;
                    }
                }
                else
                {
                    foundAFalse = true;
                }
            }
            return true;
        }

        public static IEnumerable<bool> Xor(this IEnumerable<bool> bits, IEnumerable<bool> otherBits)
        {
            return bits.Zip(otherBits, (a, b) => a ^ b);
        }

        public static IEnumerable<bool> FlipWhen(this IEnumerable<bool> bits, IEnumerable<bool> flipBits)
        {
            return bits.Zip(flipBits, (a, b) => b ? !a : a);
        }

        public static IEnumerable<bool[]> AllBitSetsOfLength(int length)
        {
            if (length <= 16)
            {
                foreach (var @ushort in Enumerable.Range(0, (int)Math.Pow(2, length)))
                {
                    yield return ((ushort)@ushort).ToBits().ToArray();
                }
                yield break;
            }
            if (length <= 32)
            {
                foreach (var @uint in Enumerable.Range(0, (int)Math.Pow(2, length)))
                {
                    yield return ((uint)@uint).ToBits().ToArray();
                }
                yield break;
            }
            throw new Exception("length too long");
        }

        public static bool[] SetTrueBitInArray(int trueBitPos, int arrayLength)
        {
            return Enumerable.Repeat(false, trueBitPos)
                .Concat(new [] {true})
                    .Concat(Enumerable.Repeat(false, arrayLength - trueBitPos - 1))
                        .ToArray();
        }

        public static string ToTabString(this bool[] bits)
        {
            return  bits.Aggregate
                    (
                        "",
                        (s, bv) => s + (string.IsNullOrEmpty(s) ? string.Empty : "\t") + bv);
        }
    }
}
