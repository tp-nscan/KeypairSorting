using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Sorting.Switchables
{
    public static class Switchable
    {
        public static ISwitchable<IReadOnlyList<uint>> ToSwitchableIntArray(this IReadOnlyList<uint> ints)
        {
            return new SwitchableIntArrayImpl(ints);
        }

        public static ISwitchable<IReadOnlyList<uint>> ToSwitchableIntArray(this IRando random, int keyCount)
        {
            return
                Enumerable.Range(0, keyCount)
                    .Select(T => (uint) T)
                    .ToArray()
                    .FisherYatesShuffle(random)
                    .ToSwitchableIntArray();
        }

        public static IEnumerable<ISwitchable<IReadOnlyList<uint>>> ToSwitchableIntArrays(this IRando random, int keyCount)
        {
            return random.ToRandomEnumerator().Select(r => r.ToSwitchableIntArray(keyCount));
        }

        public static ISwitchable<IReadOnlyList<bool>> ToSwitchableBitArray(this bool[] bits)
        {
            return new SwitchableBitArrayImpl(bits);
        }

        public static IEnumerable<ISwitchable<IReadOnlyList<bool>>> ToSwitchableBitArrays(this IRando random, int keyCount)
        {
            return random.ToBoolEnumerator(0.5).Chunk(keyCount).Select(T => T.ToSwitchableBitArray());
        }

        public static IEnumerable<ISwitchable<uint>> ToUintSwitchables(this IRando rando, int keyCount)
        {
            return rando.ToRandomUintFlags(keyCount).ToUintSwitchable<uint>(keyCount);
        }

        public static IEnumerable<ISwitchable<ulong>> ToUlongSwitchables(this IRando rando, int keyCount)
        {
            return rando.ToRandomUlongFlags(keyCount).ToUlongSwitchable<ulong>(keyCount);
        }

        public static IEnumerable<ISwitchable<uint>> AllUintEnumerablesForKeyCount(int keyCount)
        {
            return Enumerable.Range(0, (int) Math.Pow(2, keyCount)).ToUintSwitchable<uint>(keyCount);
        }

        #region private helpers

        private static IEnumerable<ISwitchable<uint>> ToUintSwitchable<T>(this IEnumerable<int> numbers, int keyCount)
        {
            return numbers.Select(t => new SwitchableUintImpl((uint)t, keyCount));
        }

        private static IEnumerable<ISwitchable<uint>> ToUintSwitchable<T>(this IEnumerable<uint> numbers, int keyCount)
        {
            return numbers.Select(t => new SwitchableUintImpl(t, keyCount));
        }

        private static IEnumerable<ISwitchable<ulong>> ToUlongSwitchable<T>(this IEnumerable<ulong> numbers, int keyCount)
        {
            return numbers.Select(t => new SwitchableUlongImpl(t, keyCount));
        }

        #endregion
    }

    class SwitchableUshortImpl : SwitchableImpl<ushort>
    {
        public SwitchableUshortImpl(ushort value, int keyCount)
            : base(value, keyCount)
        {
        }

        public override SwitchableDataType SwitchableType
        {
            get { return SwitchableDataType.Ushort; }
        }
    }

    class SwitchableUintImpl : SwitchableImpl<uint>
    {
        public SwitchableUintImpl(uint value, int keyCount)
            : base(value, keyCount)
        {
        }

        public override SwitchableDataType SwitchableType
        {
            get { return SwitchableDataType.UInt; }
        }
    }

    class SwitchableUlongImpl : SwitchableImpl<ulong>
    {
        public SwitchableUlongImpl(ulong value, int keyCount)
            : base(value, keyCount)
        {
        }

        public override SwitchableDataType SwitchableType
        {
            get { return SwitchableDataType.ULong; }
        }
    }

    class SwitchableBitArrayImpl : SwitchableImpl<IReadOnlyList<bool>>
    {
        public SwitchableBitArrayImpl(IReadOnlyList<bool> bits)
            : base(bits, bits.Count)
        {
        }

        public override SwitchableDataType SwitchableType
        {
            get { return SwitchableDataType.BitArray; }
        }
    }

    class SwitchableIntArrayImpl : SwitchableImpl<IReadOnlyList<uint>>
    {
        public SwitchableIntArrayImpl(IReadOnlyList<uint> ints)
            : base(ints, ints.Count)
        {

        }

        public override SwitchableDataType SwitchableType
        {
            get { return SwitchableDataType.IntArray; }
        }
    }

    abstract class SwitchableImpl<T> : ISwitchable<T>
    {
        private readonly int _keyCount;
        private readonly T _item;

        protected SwitchableImpl(T item, int keyCount)
        {
            _item = item;
            _keyCount = keyCount;
        }

        public int KeyCount
        {
            get { return _keyCount; }
        }

        public abstract SwitchableDataType SwitchableType
        {
            get;
        }

        public T Item
        {
            get { return _item; }
        }
    }
}
