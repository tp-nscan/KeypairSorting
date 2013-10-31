using System;

namespace MathUtils.Interval
{
    public class RealIntervalLinearMap
    {
        public RealIntervalLinearMap(RealInterval domain, RealInterval range)
        {
            Domain = domain;
            Range = range;
        }

        public RealInterval Domain { get; private set; }
        public RealInterval Range { get; private set; }

        public RealInterval MapRealInterval(RealInterval value)
        {
            if(Double.IsNaN(Slope))
            {
                return RealInterval.Empty;
            }
            return new RealInterval(MapDouble(value.Min), MapDouble(value.Max));
        }

        public double MapDouble(double value)
        {
            if (Double.IsNaN(Slope))
            {
                return Double.NaN;
            }
            return Range.Min + (value - Domain.Min)*Slope;
        }

        double Slope
        {
            get
            {
                if (Math.Abs(Domain.Span() - 0) < Double.Epsilon)
                {
                    return Double.NaN;
                }
                return Range.Span() / Domain.Span();
            }
        }
    }
}
