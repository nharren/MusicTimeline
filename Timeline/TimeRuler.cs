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
            switch (TimeRulerUnit)
            {
                case TimeRulerUnit.Day:
                    return dates.Span().TotalDays * TimeUnitWidth;

                case TimeRulerUnit.Hour:
                    return dates.Span().TotalHours * TimeUnitWidth;

                case TimeRulerUnit.Minute:
                    return dates.Span().TotalMinutes * TimeUnitWidth;

                case TimeRulerUnit.Second:
                    return dates.Span().TotalSeconds * TimeUnitWidth;

                default:
                    return double.NaN;
            }
        }

        public double ToPixels(ExtendedDateTime start, ExtendedDateTime end)
        {
            switch (TimeRulerUnit)
            {
                case TimeRulerUnit.Day:
                    return (end - start).TotalDays * TimeUnitWidth;

                case TimeRulerUnit.Hour:
                    return (end - start).TotalHours * TimeUnitWidth;

                case TimeRulerUnit.Minute:
                    return (end - start).TotalMinutes * TimeUnitWidth;

                case TimeRulerUnit.Second:
                    return (end - start).TotalSeconds * TimeUnitWidth;

                default:
                    return double.NaN;
            }
        }

        public double ToPixels(int unitCount)
        {
            return unitCount * TimeUnitWidth;
        }

        public int ToUnitCount(double pixels)
        {
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

        public int ToUnitCount(ExtendedTimeSpan timeSpan)
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

        public TimeSpan ToTimeSpan(double pixels)
        {
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

        public TimeSpan ToTimeSpan(int timeUnitCount)
        {
            switch (TimeRulerUnit)
            {
                case TimeRulerUnit.Day:
                    return TimeSpan.FromDays(timeUnitCount);

                case TimeRulerUnit.Hour:
                    return TimeSpan.FromHours(timeUnitCount);

                case TimeRulerUnit.Minute:
                    return TimeSpan.FromMinutes(timeUnitCount);

                case TimeRulerUnit.Second:
                    return TimeSpan.FromSeconds(timeUnitCount);

                default:
                    return TimeSpan.Zero;
            }
        }
    }
}