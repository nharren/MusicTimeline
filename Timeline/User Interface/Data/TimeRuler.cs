using System;
using System.ExtendedDateTimeFormat;

namespace NathanHarrenstein.Timeline
{
    public class TimeRuler
    {
        public TimeRulerUnit TimeRulerUnit { get; set; }

        public double TimeUnitWidth { get; set; }

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

        public double ToPixels(int timeUnitCount)
        {
            return timeUnitCount * TimeUnitWidth;
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