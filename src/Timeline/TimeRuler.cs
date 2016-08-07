using System;
using System.EDTF;

namespace NathanHarrenstein.Timeline
{
    public class TimeRuler
    {
        public TimeRulerUnit TimeRulerUnit { get; set; }

        public double TimeUnitWidth { get; set; }

        public double ToPixels(ExtendedDateTimeInterval dates)
        {
            if (dates == null)
            {
                throw new ArgumentNullException("dates");
            }

            return ToPixels(dates.Span());
        }

        public double ToPixels(ExtendedDateTime start, ExtendedDateTime end)
        {
            if (start == null)
            {
                throw new ArgumentNullException("start");
            }

            if (end == null)
            {
                throw new ArgumentNullException("end");
            }

            return ToPixels(end - start);
        }

        public double ToPixels(TimeSpan timeSpan)
        {
            if (timeSpan == null)
            {
                throw new ArgumentNullException("timeSpan");
            }

            switch (TimeRulerUnit)
            {
                case TimeRulerUnit.Day:
                    return timeSpan.TotalDays * TimeUnitWidth;

                case TimeRulerUnit.Hour:
                    return timeSpan.TotalHours * TimeUnitWidth;

                case TimeRulerUnit.Minute:
                    return timeSpan.TotalMinutes * TimeUnitWidth;

                case TimeRulerUnit.Second:
                    return timeSpan.TotalSeconds * TimeUnitWidth;

                default:
                    return double.NaN;
            }
        }

        public double ToPixels(int unitCount)
        {
            return unitCount * TimeUnitWidth;
        }

        public TimeSpan ToTimeSpan(double pixels)
        {
            if (double.IsNaN(TimeUnitWidth) || TimeUnitWidth == 0.0)
            {
                throw new InvalidOperationException("Could not calculate TimeSpan because TimeUnitWidth was an invalid number.");
            }

            switch (TimeRulerUnit)
            {
                case TimeRulerUnit.Day:
                    return TimeSpan.FromDays(pixels / TimeUnitWidth);

                case TimeRulerUnit.Hour:
                    return TimeSpan.FromHours(pixels / TimeUnitWidth);

                case TimeRulerUnit.Minute:
                    return TimeSpan.FromMinutes(pixels / TimeUnitWidth);

                case TimeRulerUnit.Second:
                    return TimeSpan.FromSeconds(pixels / TimeUnitWidth);

                default:
                    return TimeSpan.Zero;
            }
        }

        public TimeSpan ToTimeSpan(int timeUnits)
        {
            switch (TimeRulerUnit)
            {
                case TimeRulerUnit.Day:
                    return TimeSpan.FromDays(timeUnits);

                case TimeRulerUnit.Hour:
                    return TimeSpan.FromHours(timeUnits);

                case TimeRulerUnit.Minute:
                    return TimeSpan.FromMinutes(timeUnits);

                case TimeRulerUnit.Second:
                    return TimeSpan.FromSeconds(timeUnits);

                default:
                    return TimeSpan.Zero;
            }
        }

        public int ToTimeUnits(double pixels)
        {
            if (TimeUnitWidth == 0 || double.IsNaN(TimeUnitWidth))
            {
                throw new InvalidOperationException("Could not calculate TimeUnits because TimeUnitWidth was an invalid number.");
            }

            switch (TimeRulerUnit)
            {
                case TimeRulerUnit.Day:
                    return (int)Math.Floor(pixels / TimeUnitWidth);

                case TimeRulerUnit.Hour:
                    return (int)Math.Floor(pixels / TimeUnitWidth);

                case TimeRulerUnit.Minute:
                    return (int)Math.Floor(pixels / TimeUnitWidth);

                case TimeRulerUnit.Second:
                    return (int)Math.Floor(pixels / TimeUnitWidth);

                default:
                    return default(int);
            }
        }

        public int ToTimeUnits(ExtendedTimeSpan timeSpan)
        {
            switch (TimeRulerUnit)
            {
                case TimeRulerUnit.Day:
                    return (int)Math.Floor(timeSpan.TotalDays);

                case TimeRulerUnit.Hour:
                    return (int)Math.Floor(timeSpan.TotalHours);

                case TimeRulerUnit.Minute:
                    return (int)Math.Floor(timeSpan.TotalMinutes);

                case TimeRulerUnit.Second:
                    return (int)Math.Floor(timeSpan.TotalSeconds);

                default:
                    return default(int);
            }
        }
    }
}