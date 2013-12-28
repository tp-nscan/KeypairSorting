using System;
using System.Collections.Generic;

namespace MathUtils.Collections
{
    public static class DictionaryExt
    {

        public static void Merge<TK, TV>
        (
            this IDictionary<TK, TV> ableA,
            IReadOnlyDictionary<TK, TV> ableB,
            Func<TV, TV, TV> mergeFunc
        )
        {
            foreach (var key in ableB.Keys)
            {
                ableA[key] = ableA.ContainsKey(key)
                    ? mergeFunc(ableA[key], ableB[key])
                    : ableB[key];
            }
        }

        //public static IReadOnlyDictionary<TK, TV> Merge<TK, TV>
        //    (
        //        this IReadOnlyDictionary<TK, TV> ableA,
        //        IReadOnlyDictionary<TK, TV> ableB,
        //        Func<TV,TV,TV> mergeFunc
        //    )
        //{
        //    var dictRet = new Dictionary<TK, TV>();
        //    foreach (var key in ableA.Keys)
        //    {
        //        dictRet[key] = ableB.ContainsKey(key)
        //            ? mergeFunc(ableA[key], ableB[key])
        //            : ableA[key];
        //    }
        //    return dictRet;
        //}

    }
}
