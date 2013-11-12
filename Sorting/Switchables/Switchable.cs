﻿using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Bits;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Sorting.Switchables
{
    public interface ISwitchable<out T> : ISwitchable
    {
        T Item { get; }
    }

    public interface ISwitchable
    {
        int KeyCount { get; }
        Type SwitchableDataType { get; }
    }

    public static class Switchable
    {
        public static IEnumerable<ISwitchable<T>> MakeSwitchables<T>(this IRando random, int keyCount)
        {
            if (typeof (T) == typeof (uint))
            {
                return (IEnumerable<ISwitchable<T>>)random.NextUintByBits(keyCount).ToUintSwitchable(keyCount);
            }
            if (typeof(T) == typeof(ulong))
            {
                return (IEnumerable<ISwitchable<T>>)random.NextUlongByBits(keyCount).ToUlongSwitchable(keyCount);
            }
            if (typeof(T) == typeof(bool[]))
            {
                return (IEnumerable<ISwitchable<T>>)random.ToBoolEnumerator(0.5).Chunk(keyCount).Select(b => b.ToSwitchableBitArray());
            }
            if (typeof(T) == typeof(int[]))
            {
                return (IEnumerable<ISwitchable<T>>)random.ToRandomEnumerator().Select(r => r.ToSwitchableIntArray(keyCount));
            }
            throw new Exception("unhandled switchable type");
        }

        public static ISwitchable<int[]> ToSwitchableIntArray(this int[] ints)
        {
            return new SwitchableIntArrayImpl(ints);
        }

        public static ISwitchable<int[]> ToSwitchableIntArray(this IRando random, int keyCount)
        {
            return
                Enumerable.Range(0, keyCount)
                    .ToArray()
                    .FisherYatesShuffle(random)
                    .ToSwitchableIntArray();
        }

        public static ISwitchable<bool[]> ToSwitchableBitArray(this bool[] bits)
        {
            return new SwitchableBitArrayImpl(bits);
        }

        public static IEnumerable<ISwitchable<uint>> AllSwitchablesForKeyCount(int keyCount)
        {
            return Enumerable.Range(0, (int) Math.Pow(2, keyCount)).ToUintSwitchable(keyCount);
        }

        #region private helpers

        private static IEnumerable<ISwitchable<uint>> ToUintSwitchable(this IEnumerable<int> numbers, int keyCount)
        {
            return numbers.Select(t => new SwitchableUintImpl((uint)t, keyCount));
        }

        private static IEnumerable<ISwitchable<uint>> ToUintSwitchable(this IEnumerable<uint> numbers, int keyCount)
        {
            return numbers.Select(t => new SwitchableUintImpl(t, keyCount));
        }

        private static IEnumerable<ISwitchable<ulong>> ToUlongSwitchable(this IEnumerable<ulong> numbers, int keyCount)
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
        public override int GetHashCode()
        {
            return Item;
        }
    }

    class SwitchableUintImpl : SwitchableImpl<uint>
    {
        public SwitchableUintImpl(uint value, int keyCount)
            : base(value, keyCount)
        {
        }
        public override int GetHashCode()
        {
            return (int)Item;
        }
    }

    class SwitchableUlongImpl : SwitchableImpl<ulong>
    {
        public SwitchableUlongImpl(ulong value, int keyCount)
            : base(value, keyCount)
        {
            _hash = ((int)(Item >> 32)).DeZero(8675309) * ((int)Item).DeZero(92017);
        }

        private readonly int _hash;
        public override int GetHashCode()
        {
            return _hash;
        }
    }

    class SwitchableBitArrayImpl : SwitchableImpl<bool[]>
    {
        public SwitchableBitArrayImpl(bool[] bits)
            : base(bits, bits.Length)
        {
            _hash = Item.ToHash();
        }

        private readonly int _hash;
        public override int GetHashCode()
        {
            return _hash;
        }
    }

    class SwitchableIntArrayImpl : SwitchableImpl<int[]>
    {
        public SwitchableIntArrayImpl(int[] ints)
            : base(ints, ints.Length)
        {
            _hash = ints.ToHash(t=>t);
        }

        private readonly int _hash;
        public override int GetHashCode()
        {
            return _hash;
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

        public Type SwitchableDataType
        {
            get { return typeof(T); }
        }

        public T Item
        {
            get { return _item; }
        }

        protected bool Equals(SwitchableImpl<T> other)
        {
            return _keyCount == other._keyCount && EqualityComparer<T>.Default.Equals(_item, other._item);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SwitchableImpl<T>)obj);
        }

        public abstract override int GetHashCode();

    }

}
