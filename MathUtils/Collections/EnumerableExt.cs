﻿using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Bits;

namespace MathUtils.Collections
{
    public static class EnumerableExt
    {
        public static IEnumerable<T> ToEnumerable<T>(this T item)
        {
            yield return item;
        }

        public static IEnumerable<T> Filter<T>(this IReadOnlyList<T> items, Func<int, bool> selector)
        {
            return items.Where((t, i) => selector(i));
        }

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

        public static int ToHash<T>(this IEnumerable<T> items, Func<T, int> itemHash)
        {
            return items.Aggregate(1, (current, item) => (current*397) ^ itemHash(item).DeZero(42));
        }

        public static IEnumerable<T> GetMatchingItems<T>(this IEnumerable<T> ableA, IEnumerable<T> ableB)
        {
            var atorA = ableA.GetEnumerator();
            var atorB = ableB.GetEnumerator();
            while (atorA.MoveNext() && atorB.MoveNext())
            {
                if (atorA.Current.Equals(atorB.Current))
                {
                    yield return atorA.Current;
                }
            }
        }

        public static bool IsSameAs<T>(this IEnumerable<T> ableA, IEnumerable<T> ableB)
        {
            var atorA = ableA.ToList();
            var atorB = ableB.ToList();
            if (atorA.Count != atorB.Count)
            {
                return false;
            }

            for (var i = 0; i < atorA.Count; i++)
            {
                if (! atorA[i].Equals(atorB[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static IEnumerable<T> GetDifferentItems<T>(this IEnumerable<T> ableA, IEnumerable<T> ableB)
        {
            var atorA = ableA.GetEnumerator();
            var atorB = ableB.GetEnumerator();
            while (atorA.MoveNext() && atorB.MoveNext())
            {
                if (! atorA.Current.Equals(atorB.Current))
                {
                    yield return atorB.Current;
                }
            }
        }

        public static IEnumerable<T> Mutate<T>(this IEnumerable<T> source, IEnumerable<bool> mutateIfTrue, Func<T, T> mutator)
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

        public static IEnumerable<IReadOnlyList<T>> Slice<T>(this IEnumerable<T> enumerT, int chunkSize)
        {
            var retChunk = new List<T>();
            foreach (var t in enumerT)
            {
                retChunk.Add(t);
                if (retChunk.Count != chunkSize) continue;
                yield return retChunk;
                retChunk = new List<T>();
            }
            if (retChunk.Count > 0)
            {
                yield return retChunk;
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


        public static void Recombine<T>(IEnumerable<T> aIn, IEnumerable<T> bIn, IEnumerable<bool> swaps, out List<T> aOut, out List<T> bOut)
        {
            var aList = aIn.ToList();
            var bList = bIn.ToList();
            var swapList = swaps.ToList();
            aOut = new List<T>();
            bOut = new List<T>();

            var ct = aList.Count;
            if (ct < 2)
            {
                throw new Exception("arrays must be length 2 or more");
            }
            if ((bList.Count != ct) || (swapList.Count != ct))
            {
                throw new Exception("arrays are not the same length");
            }
            for (var i = 0; i < ct; i++)
            {
                EnumerableExt.SwapIf(ref aList, ref bList, swapList[i]);
                aOut.Add(aList[i]);
                bOut.Add(bList[i]);
            }
        }

        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> original)
        {
            var origList = original as IList<T> ?? original.ToList();

            while (true)
            {
                for (var j = 0; j < origList.Count(); j++)
                {
                    yield return origList[j];
                }
            }
        }

        //public static IEnumerable<T> Weave<T>(this IEnumerable<T> original)
        //{
        //    var origList = original as IList<T> ?? original.ToList();

        //    for (var i = 0; i < 10; i++)
        //    {
        //        for (var j = 0; (i+j) < origList.Count(); j+=10)
        //        {
        //            yield return origList[i+j];
        //        }
        //    }
        //}


        //public static IEnumerable<T> Weave<T>(this IEnumerable<T> original)
        //{
        //    var origList = original as IList<T> ?? original.ToList();

        //    for (var i = 0; i < origList.Count()/2; i++)
        //    {
        //        yield return origList[i];
        //        yield return origList[origList.Count() - i -1];
        //    }
        //}

        public static IEnumerable<T> Weave<T>(this IEnumerable<T> original)
        {
            var origList = original as IList<T> ?? original.ToList();
            var halfSz = origList.Count()/2;
            for (var i = 0; i < halfSz; i++)
            {
                yield return origList[halfSz + i];
                yield return origList[halfSz - i - 1];
            }
        }

        public static IEnumerable<ulong> ToUlongs(this IEnumerable<uint> uints)
        {
            var ready = false;
            ulong ulongRet = 0;
            foreach (var u in uints)
            {
                if (ready)
                {
                    ulongRet <<= 32;
                    yield return ulongRet + u;
                    ready = false;
                }
                else
                {
                    ulongRet = u;
                    ready = true;
                }
            }
        }

        public static IEnumerable<uint> ToUints(this IEnumerable<ulong> ulongs)
        {
            foreach (var u in ulongs)
            {
                var shift = u >> 32;
                yield return (uint)shift;
                yield return (uint) u;
            }
        }

        public static string PaddedReport(this IEnumerable<int> intList, int padding)
        {
            return intList.Aggregate(string.Empty, (o, n) => o + n.ToString().PadLeft(padding));
        }
    }


}
