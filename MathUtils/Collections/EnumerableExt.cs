using System;
using System.Collections.Generic;
using System.Linq;

namespace MathUtils.Collections
{
    public static class EnumerableExt
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            var c = a;
            a = b;
            b = c;
        }

        public static void SwapIf<T>(ref T a, ref T b, bool trigger)
        {
            if(! trigger) return;
            var c = a;
            a = b;
            b = c;
        }

        public static IEnumerable<T> Mutate<T>(this IEnumerable<T> source, IEnumerable<bool> mutateIfTrue,
            Func<T, T> mutator)
        {
            var sourceEnumerator = source.GetEnumerator();
            var mutateIfTrueEnumerator = mutateIfTrue.GetEnumerator();

            while (sourceEnumerator.MoveNext() && mutateIfTrueEnumerator.MoveNext())
            {
                yield return mutateIfTrueEnumerator.Current ? mutator(sourceEnumerator.Current) : sourceEnumerator.Current;
            }
        }

        public static bool HasSameElementsAs<T>(this IReadOnlyList<T> list, IReadOnlyList<T> otherList)
        {
            return list.Count == otherList.Count && list.HasSameElementsRepeatsAllowed(otherList);
        }

        public static bool HasSameElementsRepeatsAllowed<T>(this IReadOnlyList<T> list, IReadOnlyList<T> otherList)
        {
            return !list.Except(otherList).Any() && !otherList.Except(list).Any();
        }

        public static IEnumerable<T[]> Chunk<T>(this IEnumerable<T> enumerT, int chunkSize)
        {
            var retChunk = new T[chunkSize];
            var chunkDex = 0;
            foreach (var t in enumerT)
            {
                retChunk[chunkDex++] = t;
                if (chunkDex != chunkSize) continue;
                yield return retChunk;
                retChunk = new T[chunkSize];
                chunkDex = 0;
            }
        }

        public static IEnumerable<IList<Tuple<TS, T>>> DoubleChunk<TS, T>(this IEnumerable<TS> enumerableS, IEnumerable<T> enumerableT, int chunkSize)
        {
            var enumeratorS = enumerableS.GetEnumerator();
            var enumeratorT = enumerableT.GetEnumerator();
            var retChunk = new List<Tuple<TS, T>>();
            while (true)
            {
                enumeratorS.MoveNext();
                enumeratorT.MoveNext();
                retChunk.Add(new Tuple<TS, T>(enumeratorS.Current, enumeratorT.Current));
                if (retChunk.Count != chunkSize) continue;
                yield return retChunk;
                retChunk = new List<Tuple<TS, T>>();
            }
        }

        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> items) where T : class
        {
            return items.Where(i => i != null);
        }

        public static K GetFirstNonMatching<K, V>(this IEnumerable<K> keys, IEnumerable<V> values, Func<V, K> keySelector)
        {
            var dict = values.ToDictionary(keySelector);
            foreach (var key in keys.Where(key => !dict.ContainsKey(key)))
            {
                return key;
            }
            return default(K);
        }

        public static IEnumerable<T> RoundRobin<T>(this IEnumerable<T> items, int startPosition)
        {
            var itemList = items.ToList();
            while (true)
            {
                yield return itemList[startPosition++ % itemList.Count];
            }
        }

        public static IEnumerable<Tuple<S, T>> OuterJoinWith<S, T>(this IEnumerable<S> sVals, IEnumerable<T> tVals)
            where S : class
            where T : class
        {
            var sList = sVals.ToList();
            var tList = tVals.ToList();

            var maxLen = Math.Max(sList.Count, tList.Count());

            for (int i = 0; i < maxLen; i++)
            {
                yield return new Tuple<S, T>
                    (
                       (i < sList.Count) ? sList[i] : null,
                       (i < tList.Count) ? tList[i] : null
                    );
            }
        }

        public static IEnumerable<Tuple<S, T>> JoinWith<S, T>(this IEnumerable<S> sVals, IEnumerable<T> tVals)
        {
            var sList = sVals.ToList();
            var tList = tVals.ToList();
            if (sList.Count != tList.Count)
            {
                throw new Exception("Enumerations are not the same length");
            }
            return sList.Select((s, i) => new Tuple<S, T>(s, tList[i]));
        }


    }


}
