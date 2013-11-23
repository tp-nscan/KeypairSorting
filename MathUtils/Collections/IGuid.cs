using System;
using System.Collections.Generic;

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
    }
}
