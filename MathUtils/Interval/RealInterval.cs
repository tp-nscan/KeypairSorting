using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MathUtils.Interval
{
    public class RealInterval : IEquatable<RealInterval>
    {
        public RealInterval(double minValue, double maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentException("min value must be less than max value");
            }
            _min = minValue;
            _max = maxValue;
        }

        private readonly double _min;
        public double Min
        {
            get { return _min; }
        }

        private readonly double _max;
        public double Max
        {
            get { return _max; }
        }


        //Expand the range so the given point is inside it.
        public RealInterval Accomodate(IEnumerable<double> values)
        {
            var valList = this.Points().ToList();
            valList.AddRange(values);

            if (valList.Count < 2) { return Empty; }

            return new RealInterval(valList.Min(), valList.Max());
        }

        //Expand the range so the given point is inside it.
        public RealInterval Accomodate(IEnumerable<RealInterval> values)
        {
            return Accomodate(values.SelectMany(T => T.Points()));
        }

        //Expand the range so the given point is inside it.
        public RealInterval Accomodate(double value)
        {
            var valList = this.Points().ToList();
            valList.Add(value);

            if (valList.Count < 2) { return Empty; }

            return new RealInterval(valList.Min(), valList.Max());
        }

        //Expand the range so the given point is inside it.
        public RealInterval Accomodate(RealInterval value)
        {
            return Accomodate(value.Points());
        }

        public bool Contains(double value)
        {
            return (value <= Max) && (value >= Min);
        }

        public RealInterval Copy()
        {
            return new RealInterval(Min, Max);
        }

        public RealInterval Intersect(RealInterval other)
        {
            var newMin = Math.Max(Min, other.Min);
            var newMax = Math.Min(Max, other.Max);
            return (newMin <= newMax) ? new RealInterval(newMin, newMax) : Empty;
        }

        public bool Intersects(RealInterval other)
        {
            return (Intersect(other) != Empty);
        }

        #region specific instances

        private static readonly RealInterval all = new RealInterval(double.NegativeInfinity, double.PositiveInfinity);
        public static RealInterval All
        {
            get { return all.Copy(); }
        }

        static readonly RealInterval empty = new RealInterval(double.NaN, double.NaN);
        public static RealInterval Empty
        {
            get { return empty.Copy(); }
        }

        static readonly RealInterval zeroRange = new RealInterval(0, 0);
        public static RealInterval ZeroRange
        {
            get { return zeroRange.Copy(); }
        }

        static readonly RealInterval unitRange = new RealInterval(0, 1);
        public static RealInterval UnitRange
        {
            get { return unitRange.Copy(); }
        }

        static readonly RealInterval threeLogsRange = new RealInterval(1.0, 1000.0);
        public static RealInterval ThreeLogsRange
        {
            get { return threeLogsRange.Copy(); }
        }

        static readonly RealInterval fiveLogsRange = new RealInterval(1.0, 100000.0);
        public static RealInterval FiveLogsRange
        {
            get { return fiveLogsRange.Copy(); }
        }

        static readonly RealInterval positiveNumbers = new RealInterval(0, Double.PositiveInfinity);
        public static RealInterval PositiveNumbers
        {
            get { return positiveNumbers.Copy(); }
        }

        #endregion

        #region IEquatable impl

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(RealInterval)) return false;
            return Equals((RealInterval)obj);
        }

        public bool Equals(RealInterval other)
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

        public static bool operator ==(RealInterval left, RealInterval right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RealInterval left, RealInterval right)
        {
            return !Equals(left, right);
        }

        #endregion

        public override string ToString()
        {
            return Min.ToString(CultureInfo.InvariantCulture) + "," + Max.ToString(CultureInfo.InvariantCulture);
        }
    }

}
