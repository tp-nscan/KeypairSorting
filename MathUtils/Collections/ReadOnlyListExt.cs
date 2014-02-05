﻿using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Rand;

namespace MathUtils.Collections
{
    public static class ReadOnlyListExt
    {
        public static bool AreOutOfOrder<T>(this IReadOnlyList<T> list, Func<T, int> indexer)
        {
            return list.Where((t, i) => indexer(t) != i).Any();
        }

        public static IEnumerable<IReadOnlyList<T>> Chop<T>(this IReadOnlyList<T> loaf, int thickness)
        {
            var position = 0;
            while (position < loaf.Count)
            {
                yield return loaf.Skip(position).Take(thickness).ToList();
                position += thickness;
            }
        }

        public static IReadOnlyList<T> VectorSum<T>(this IEnumerable<IReadOnlyList<T>> listOfList, Func<T, T, T> adder)
        {
            var retVal = new T[0];
            return listOfList.Aggregate(retVal, (current, list) => current.VectorSum(list, adder));
        }

        public static IReadOnlyList<T> VectorSum<T>(this IReadOnlyList<T> lhs, IReadOnlyList<T> rhs, Func<T, T, T> adder)
        {
            var retVal = new T[lhs.Count];
            for (var i = 0; i < lhs.Count; i++)
            {
                retVal[i] = adder(lhs[i], rhs[i]);
            }
            return retVal;
        }

        public static IEnumerable<T> ElementsAfter<T>(this IReadOnlyList<T> list, int index)
        {
            for (var curDex = index + 1; curDex < list.Count; curDex++)
            {
                yield return list[curDex];
            }
        }

        public static T[] VectorSum<T>(this T[] lhs, IReadOnlyList<T> rhs, Func<T, T, T> adder)
        {
            if (lhs.Length == 0) { lhs = new T[rhs.Count];}
            for (var i = 0; i < rhs.Count; i++)
            {
                lhs[i] = adder(lhs[i], rhs[i]);
            }
            return lhs;
        }

        public static IEnumerable<Tuple<int, T, T>> GetDiffs<T>(this IReadOnlyList<T> listA, IReadOnlyList<T> listB)
        {
            for (var i = 0; i < listA.Count; i++)
            {
                if (! listA[i].Equals(listB[i]))
                {
                    yield return new Tuple<int, T, T>(i, listA[i], listB[i]);
                }
            }
        }

        public static IEnumerable<Tuple<T, T>> PairRandomly<T>(this IReadOnlyList<T> items, IRando rando)
        {
            var workingList = items.ToList();
            if (workingList.Count % 2 == 1)
            {
                throw new ArgumentException("An odd number of items was sent to PairRandomly");
            }
            while (workingList.Any())
            {
                var nextIndex = rando.NextInt() % workingList.Count();
                var itemA = workingList[nextIndex];
                workingList.RemoveAt(nextIndex);
                nextIndex = rando.NextInt() % workingList.Count();
                var itemB = workingList[nextIndex];
                workingList.RemoveAt(nextIndex);

                yield return new Tuple<T, T>(itemA, itemB);
            }
        }
             
        public static IReadOnlyList<double> VectorSumDouble(this IEnumerable<IReadOnlyList<double>> listOfList)
        {
            var retVal = new double[0];
            return listOfList.Aggregate(retVal, (current, list) => current.VectorSumDouble(list));
        }

        public static double[] VectorSumDouble(this double[] lhs, IReadOnlyList<double> rhs)
        {
            if (lhs.Length == 0) { lhs = new double[rhs.Count]; }
            for (var i = 0; i < rhs.Count; i++)
            {
                lhs[i] += rhs[i];
            }
            return lhs;
        }


        public static IReadOnlyList<int> VectorSumInts(this IEnumerable<IReadOnlyList<int>> listOfList)
        {
            var retVal = new int[0];
            return listOfList.Aggregate(retVal, (current, list) => current.VectorSumInt(list));
        }

        public static int[] VectorSumInt(this int[] lhs, IReadOnlyList<int> rhs)
        {
            if (lhs.Length == 0) { lhs = new int[rhs.Count]; }
            for (var i = 0; i < rhs.Count; i++)
            {
                lhs[i] += rhs[i];
            }
            return lhs;
        }

        public static bool IsOrdered<T>(this IEnumerable<T> source)
        {
            var comparer = Comparer<T>.Default;
            var previous = default(T);
            var first = true;

            foreach (var element in source)
            {
                if (!first && comparer.Compare(previous, element) > 0)
                {
                    return false;
                }
                first = false;
                previous = element;
            }
            return true;
        }

        public static IEnumerable<T> PickMembers<T>(this IReadOnlyList<T> drawPool, IEnumerable<uint> choiceIndexes)
        {
            return choiceIndexes.Select(draw => drawPool[(int) draw]);
        }

        public static T[] FisherYatesShuffle<T>(this IReadOnlyList<T> origList, IRando rando)
        {
            var arrayLength = origList.Count;
            var retArray = origList.ToArray();
            for (var i = arrayLength - 1; i > 0; i--)
            {
                var j = rando.NextInt(i + 1);
                var temp = retArray[i];
                retArray[i] = retArray[j];
                retArray[j] = temp;
            }
            return retArray;
        }

        public static T[] FisherYatesPartialShuffle<T>(this IReadOnlyList<T> origList, IRando rando, double mixingRate)
        {
            var arrayLength = origList.Count;
            var retArray = origList.ToArray();
            for (var i = arrayLength - 1; i > 0; i--)
            {
                if(rando.NextDouble() > mixingRate) continue;
                var j = rando.NextInt(i + 1);
                var temp = retArray[i];
                retArray[i] = retArray[j];
                retArray[j] = temp;
            }
            return retArray;
        }

        public static IReadOnlyList<T> Insert<T>
            (
                this IReadOnlyList<T> original, 
                IEnumerable<bool> doInsertion,
                Func<T, T> inserter
            )
        {
            var tRet = new T[original.Count];
            var doInsertionEnumer = doInsertion.GetEnumerator();
            var origDex = 0;
            var retDex = 0;
            while (retDex < original.Count)
            {
                doInsertionEnumer.MoveNext();
                tRet[retDex++] = doInsertionEnumer.Current
                    ? inserter(original[origDex])
                    : original[origDex++];
            }
            return tRet;
        }

        public static IReadOnlyList<T> Delete<T>
            (
                this IReadOnlyList<T> original, 
                IEnumerable<bool> doDeletion,
                Func<T> deleter
            )
        {
            var tRet = new T[original.Count];
            var doDeletionEnumer = doDeletion.GetEnumerator();
            var origDex = 0;
            var retDex = 0;
            while (origDex < original.Count)
            {
                doDeletionEnumer.MoveNext();
                if (doDeletionEnumer.Current)
                {
                    origDex++;
                }
                else
                {
                    tRet[retDex++] = original[origDex++];
                }
            }

            while (retDex < original.Count)
            {
                tRet[retDex++] = deleter();
            }
            return tRet;
        }

        public static IEnumerable<T> MutateInsertDelete<T>
        (
            this IReadOnlyList<T> original,
            IEnumerable<bool> doMutation,
            IEnumerable<bool> doInsertion,
            IEnumerable<bool> doDeletion,
            Func<T, T> mutator,
            Func<T, T> inserter,
            Func<T, T> paddingFunc = null
        )
        {
            var doMutationEnumer = doMutation.GetEnumerator();
            var doInsertionEnumer = doInsertion.GetEnumerator();
            var doDeletionEnumer = doDeletion.GetEnumerator();
            var origDex = 0;
            var retDex = 0;

            while ((origDex < original.Count) && (retDex < original.Count))
            {
                doMutationEnumer.MoveNext();
                doInsertionEnumer.MoveNext();
                doDeletionEnumer.MoveNext();

                var possiblyMutatedValue = doMutationEnumer.Current ? mutator(original[origDex]) : original[origDex] ;

                if (doInsertionEnumer.Current)
                {
                    if (doDeletionEnumer.Current)
                    {
                        // insertion and deletion cancel each other
                    }
                    else
                    {
                        retDex++;
                        yield return inserter(original[origDex]);
                        if (retDex < original.Count)
                        {
                            retDex++;
                            origDex++;
                            yield return possiblyMutatedValue;
                        }
                    }
                }
                else
                {
                    if (doDeletionEnumer.Current)
                    {
                        origDex++;
                    }
                    else
                    {
                        retDex++;
                        origDex++;
                        yield return possiblyMutatedValue;
                    }
                }
            }

            while (retDex < original.Count)
            {
                if (paddingFunc != null)
                {
                    retDex++;
                    yield return paddingFunc(default(T));
                }
            }
        }

        public static IEnumerable<T> ReadRange<T>(this IReadOnlyList<T> list, int start, int count)
        {
            for (var i = start; i < start + count; i++)
            {
                yield return list[i];
            }
        }
    }
}
