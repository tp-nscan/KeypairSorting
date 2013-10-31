using System;
using System.Collections.Generic;
using System.Linq;

namespace MathUtils.Interval
{
    public class IntInterval : IEquatable<IntInterval>
    {
        public static IntInterval WithBoundries(int minValue, int maxValue)
        {
            return  new IntInterval(minValue, maxValue);
        }

        public IntInterval(int minValue, int maxValue)
        {
            _min = minValue;
            _max = maxValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(IntInterval)) return false;
            return Equals((IntInterval)obj);
        }

        //Return an expanded range so the given point is inside it.
        public IntInterval Accomodate(int value)
        {
            return new IntInterval(Math.Min(Min, value), Math.Max(Max, value));
        }

        //Expand the range so the given points are inside it.
        public IntInterval Accomodate(IEnumerable<int> values)
        {
            var listValues = values.ToList();
            listValues.Add(Min);
            listValues.Add(Max);
            return new IntInterval(listValues.Min(), listValues.Max());
        }

        //Returns the smallest IntInteval that contains the this IntInterval and the arg. IntInterval
        public IntInterval Accomodate(IntInterval value)
        {
            return Accomodate(value.Min).Accomodate(value.Max);
        }

        public bool ClosureContains(int value)
        {
            return (value <= Max) && (value >= Min);
        }

        public bool OpenBoundryContains(int value)
        {
            return (value < Max) && (value > Min);
        }

        public IntInterval Copy()
        {
            return new IntInterval(Min, Max);
        }

        public IntInterval Intersect(IntInterval other)
        {
            var newMin = Math.Max(Min, other.Min);
            var newMax = Math.Min(Max, other.Max);
            return (newMin <= newMax) ? new IntInterval(newMin, newMax) : Empty;
        }

        public bool Intersects(IntInterval other)
        {
            var newMin = Math.Max(Min, other.Min);
            var newMax = Math.Min(Max, other.Max);
            return (newMin <= newMax);
        }

        private readonly int _min;
        public int Min
        {
            get { return _min; }
        }

        private readonly int _max;
        public int Max
        {
            get { return _max; }
        }

        public static IntInterval Empty
        {
            get { return new IntInterval(int.MaxValue, int.MinValue); }
        }

        public static IntInterval NonNegatives
        {
            get { return new IntInterval(0, int.MaxValue); }
        }

        public static IntInterval NonPositives
        {
            get { return new IntInterval(int.MinValue, 0); }
        }

        public static IntInterval Positives
        {
            get { return new IntInterval(1, int.MaxValue); }
        }

        public static IntInterval Negatives
        {
            get { return new IntInterval(int.MinValue, -1); }
        }

        public bool Equals(IntInterval other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other._min.Equals(_min) && other._max.Equals(_max);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable NonReadonlyFieldInGetHashCode
                return (_min.GetHashCode() * 397) ^ _max.GetHashCode();
                // ReSharper restore NonReadonlyFieldInGetHashCode
            }
        }

        public string BasicRangeFormat(string label, string format)
        {
            return string.Format("{0}: {1} - {2}", label, Min, Max);
        }

        public static bool operator ==(IntInterval left, IntInterval right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(IntInterval left, IntInterval right)
        {
            return !Equals(left, right);
        }
    }
}
