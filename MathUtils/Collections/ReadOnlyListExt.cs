using System;
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

        public static T[] VectorSum<T>(this T[] lhs, IReadOnlyList<T> rhs, Func<T, T, T> adder)
        {
            if (lhs.Length == 0) { lhs = new T[rhs.Count];}
            for (var i = 0; i < rhs.Count; i++)
            {
                lhs[i] = adder(lhs[i], rhs[i]);
            }
            return lhs;
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

        public static IEnumerable<T> PickMembers<T>(this IReadOnlyList<T> drawPool, IEnumerable<int> choiceIndexes)
        {
            return choiceIndexes.Select(draw => drawPool[draw]);
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

        public static IReadOnlyList<T> MutateInsertDelete<T>
        (
            this IReadOnlyList<T> original,
            IEnumerable<bool> doMutation,
            IEnumerable<bool> doInsertion,
            IEnumerable<bool> doDeletion,
            Func<T, T> mutator,
            Func<T, T> inserter,
            Func<T, T> deleter
        )
        {
            var tRet = new T[original.Count];
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
                        tRet[retDex++] = inserter(original[origDex]);
                        if (retDex < original.Count)
                        {
                            tRet[retDex++] = possiblyMutatedValue;
                            origDex++;
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
                        tRet[retDex++] = possiblyMutatedValue;
                        origDex++;
                    }
                }
            }

            while (retDex < original.Count)
            {
                tRet[retDex++] = deleter(default(T));
            }
            return tRet;
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

        public static IEnumerable<T> ReadRange<T>(this IReadOnlyList<T> list, int start, int count)
        {
            for (var i = start; i < start + count; i++)
            {
                yield return list[i];
            }
        }
    }
}
