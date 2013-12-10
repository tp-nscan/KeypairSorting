using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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

        public static int ToHash(this Guid guid)
        {
            var aw = guid.ToByteArray();
            var hashRet = BitConverter.ToInt32(aw, 0);
            hashRet += 23 * BitConverter.ToInt32(aw, 4);
            hashRet += 23 * BitConverter.ToInt32(aw, 8);
            hashRet += 23 * BitConverter.ToInt32(aw, 12);
            return hashRet;
        }

        public static Guid Add(this Guid lhs, Guid rhs)
        {
            var leftArray = lhs.ToByteArray();
            var leftInts = Enumerable.Range(0, 4).Select(d => BitConverter.ToUInt32(leftArray, d * 4));

            var rightArray = lhs.ToByteArray();
            var rightInts = Enumerable.Range(0, 4).Select(d => BitConverter.ToUInt32(rightArray, d * 4));

            return new Guid (
                    leftInts.Zip(rightInts, (x, y) => x + y).SelectMany(BitConverter.GetBytes).ToArray()
                );

        }
    }
}
