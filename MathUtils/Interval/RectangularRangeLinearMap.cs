using System;

namespace MathUtils.Interval
{
    public class RectangularRangeLinearMap
    {
        public RectangularRangeLinearMap(RealIntervalLinearMap xMap, RealIntervalLinearMap yMap)
        {
            if (xMap == null) throw new ArgumentNullException("xMap");
            if (yMap == null) throw new ArgumentNullException("yMap");
            XMap = xMap;
            YMap = yMap;
        }

        public RealIntervalLinearMap XMap { get; private set; }
        public RealIntervalLinearMap YMap { get; private set; }
    }

}
