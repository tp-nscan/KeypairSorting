using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using Sorting.KeyPairs;
using Sorting.Switchables;

namespace Sorting.SwitchFunctionSets
{
    public interface IKeyPairSwitchSet<T> : IKeyPairSwitchSet
    {
        bool IsSorted(T item);
        Func<T, Tuple<T, bool>> SwitchFunction(KeyPair keyPair);
    }

    public interface IKeyPairSwitchSet
    {
        int KeyCount { get; }
        Type SwitchableDataType { get; }
    }

    public static class KeyPairSwitchSet
    {

        static readonly List<IKeyPairSwitchSet> KeyPairSwitchSets = new List<IKeyPairSwitchSet>();

        public static IKeyPairSwitchSet<T> Make<T>(int keyCount)
        {
            var switchableDataType = typeof (T);

            var cached = KeyPairSwitchSets.SingleOrDefault
                (
                    k => (k.KeyCount == keyCount) && 
                        (k.SwitchableDataType == switchableDataType)
                );

            if (cached != null)
            {
                return (IKeyPairSwitchSet<T>) cached;
            }

            IKeyPairSwitchSet created;

            if (switchableDataType == typeof (ushort))
            {
                created = new UshortSwitchSet(keyCount);
            }

            else if (switchableDataType == typeof (uint))
            {
                created = new UintSwitchSet(keyCount);
            }

            else if (switchableDataType == typeof(ulong))
            {
                return (IKeyPairSwitchSet<T>) new UlongSwitchSet(keyCount);
            }

            else if (switchableDataType == typeof (int[]))
            {
                created = new IntArraySwitchSet(keyCount);
            }

            else if (switchableDataType == typeof (bool[]))
            {
                created = new BitArraySwitchSet(keyCount);
            }

            else throw new Exception(string.Format("unhandled switchable type: {0}", switchableDataType.Name));

            KeyPairSwitchSets.Add(created);

            return (IKeyPairSwitchSet<T>) created;
        }
    }
}
