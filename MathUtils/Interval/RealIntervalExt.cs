using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace MathUtils.Interval
{
    public static class RealIntervalExt
    {
        public static IEnumerable<double> Points(this RealInterval realInterval)
        {
            if (! double.IsNaN(realInterval.Min))
            {
                yield return realInterval.Min;
            }
            if (!double.IsNaN(realInterval.Max))
            {
                yield return realInterval.Max;
            }
        }
        public static double Span(this RealInterval ri)
        {
            if(ri==null)
            {
                return Double.NaN;
            }
            if (Double.IsNaN(ri.Min) || Double.IsNaN(ri.Max))
            {
                return Double.NaN;
            }
            return ri.Max - ri.Min;
        }

        public static double Center(this RealInterval ri)
        {
            return (ri.Min + ri.Max) / 2;
        }

        public static RealInterval RangeX(this Rect rect)
        {
            return new RealInterval(minValue: rect.Left, maxValue: rect.Right);
        }

        public static RealInterval RangeY(this Rect rect)
        {
            return new RealInterval(minValue: rect.Bottom, maxValue: rect.Top);
        }

        public static IEnumerable<RealInterval> SplitToEvenIntervals(this RealInterval realInterval, int segmentCount)
        {
            for(int i=0; i<segmentCount; i++)
            {
                yield return new RealInterval
                    (
                        realInterval.GetTicValue(i, segmentCount), 
                        realInterval.GetTicValue(i + 1, segmentCount)
                    );
            }
        }

        public static RealInterval Pad(this RealInterval realInterval, double leftSide, double rightSide)
        {
            return  new RealInterval(realInterval.Min-leftSide, realInterval.Max+rightSide);
        }

        public static IEnumerable<RealInterval> ToPaddedRealIntevals(this IEnumerable<double> values)
        {
            double? lastlastValue = null;
            double? lastValue = null;

            foreach (var value in values)
            {
                if(lastValue.HasValue)
                {
                    yield return (lastlastValue.HasValue) ? 
                        new RealInterval(
                            (lastlastValue.Value + lastValue.Value) / 2, 
                            (value + lastValue.Value)/2 )
                        :
                        new RealInterval(
                            lastValue.Value - (value - lastValue.Value) / 2, 
                            (value + lastValue.Value)/2 );

                    lastlastValue = lastValue;
                }

                lastValue = value;
            }

            if (lastlastValue.HasValue)
            {
                yield return
                        new RealInterval(
                            (lastlastValue.Value + lastValue.Value)/2,
                            lastValue.Value + (lastValue.Value - lastlastValue.Value) / 2);
            }
        }

        public static IEnumerable<double> GetTicsInOrder(this RealInterval realInterval, int segmentCount)
        {
            for(int i=0; i<segmentCount+1; i++)
            {
                yield return realInterval.GetTicValue(i, segmentCount);
            }
        }

        public static IEnumerable<Rect> SandwichProduct(this RealInterval horVal, IEnumerable<RealInterval> vertVals)
        {
            return vertVals.Select(mzBand => new Rect(horVal.Min, mzBand.Min, horVal.Span(), mzBand.Span()));
        }

        public static IEnumerable<Rect> PillarProduct(this RealInterval vertVal, IEnumerable<RealInterval> horVals)
        {
            return horVals.Select(mzBand => new Rect(mzBand.Min, vertVal.Min, mzBand.Span(), vertVal.Span()));
        }

        public static double GetTicValue(this RealInterval realInterval, int tic, int segmentCount)
        {
            return realInterval.Min + (realInterval.Span() * tic) / (segmentCount);
        }

        public static float GetTicValue(float firstValue, float lastValue, int tic, int segmentCount)
        {
            return firstValue + ((lastValue - firstValue) * tic) / (segmentCount);
        }

        public static RealInterval Union(this IEnumerable<RealInterval> dRs)
        {
            if (dRs == null)
            {
                return RealInterval.Empty;
            }

            var list = dRs.ToList();
            return new RealInterval(minValue: list.Min(T => T.Min), maxValue: list.Max(T => T.Max));
        }

        /// <summary>
        /// Zooms out for factor > 1
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static RealInterval ZoomBy(this RealInterval interval, double factor)
        {
            factor = Math.Abs(factor);
            return new RealInterval
                (
                    interval.Center() - interval.Span() * factor / 2,
                    interval.Center() + interval.Span() * factor / 2
               );
        }

        public static Rect ZoomBy(this Rect rect, double xZoom, double yZoom)
        {
            var xInterval = new RealInterval(rect.Left, rect.Right).ZoomBy(xZoom);
            var yInterval = new RealInterval(rect.Top, rect.Bottom).ZoomBy(yZoom);
            return new Rect(xInterval.Min, yInterval.Min, xInterval.Span(), yInterval.Span());
        }

        public static RealInterval AdjustBy(this RealInterval interval, double minDelta, double maxDelta)
        {
            return new RealInterval
                (
                    interval.Min + minDelta,
                    interval.Max + maxDelta
               );
        }

        public static RealInterval Offset(this RealInterval realInterval, double delta)
        {
            return new RealInterval(realInterval.Min + delta, realInterval.Max + delta);
        }


        #region Formatting, serialization

        public static string BracketFormat(this RealInterval realInterval, string numberFormat)
        {
            return String.Format("[{0}-{1}]", realInterval.Min.ToString(numberFormat),
                                 realInterval.Max.ToString(numberFormat));
        }

        public static RealInterval Parse(string value)
        {
            try
            {
                var pcs = value.Split(",".ToCharArray());
                return new RealInterval(double.Parse(pcs[0]), double.Parse(pcs[1]));
            }
            catch (Exception)
            {
                throw new ArgumentException(String.Format("{0} could not be parsed to RealInterval", value));
            }
        }


        public static string BasicRangeFormat(this RealInterval realInterval, string label, string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                return string.Format("{0}: {1} - {2}", label, realInterval.Min, realInterval.Max);
            }
            return string.Format("{0}: {1} - {2}", label, realInterval.Min.ToString(format), realInterval.Max.ToString(format));
        }

        public static XElement ToXml(this RealInterval realInterval)
        {
            return new XElement
                (
                    "RealInterval",
                    new XElement("Min", realInterval.Min),
                    new XElement("Max", realInterval.Max)
                );
        }

        public static RealInterval ToRealInterval(this XElement xml)
        {
            return new RealInterval(
                // ReSharper disable PossibleNullReferenceException
                double.Parse(xml.Element("Min").Value),
                double.Parse(xml.Element("Max").Value));
            // ReSharper restore PossibleNullReferenceException
        }

        #endregion


    }

}
