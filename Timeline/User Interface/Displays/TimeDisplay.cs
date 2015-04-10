using NathanHarrenstein.Controls;
using System;
using System.ExtendedDateTimeFormat;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace NathanHarrenstein.Timeline
{
    public class TimeDisplay : FrameworkElement, IPan
    {
        public static readonly DependencyProperty EndProperty = DependencyProperty.Register("End", typeof(DateTime), typeof(TimeDisplay));

        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(TimeUnit), typeof(TimeDisplay));

        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(TimeDisplay));

        public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start", typeof(DateTime), typeof(TimeDisplay));

        private string font;

        private double fontSize;

        private Brush foreground;

        private double horizontalOffset;

        private string labelFormat;

        private double labelOffset;

        private Pen linePen;

        private double unit;

        public DateTime End
        {
            get
            {
                return (DateTime)GetValue(EndProperty);
            }
            set
            {
                SetValue(EndProperty, value);
            }
        }

        public string Font
        {
            get
            {
                return font;
            }
            set
            {
                font = value;
            }
        }

        public double FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                fontSize = value;
            }
        }

        public Brush Foreground
        {
            get
            {
                return foreground;
            }
            set
            {
                foreground = value;
            }
        }

        public double HorizontalOffset
        {
            get
            {
                return horizontalOffset;
            }
            set
            {
                horizontalOffset = value;
            }
        }

        public string LabelFormat
        {
            get
            {
                return labelFormat;
            }
            set
            {
                labelFormat = value;
            }
        }

        public double LabelOffset
        {
            get
            {
                return labelOffset;
            }
            set
            {
                labelOffset = value;
            }
        }

        public Pen LinePen
        {
            get
            {
                return linePen;
            }
            set
            {
                linePen = value;
            }
        }

        public TimeUnit Resolution
        {
            get
            {
                return (TimeUnit)GetValue(ResolutionProperty);
            }
            set
            {
                SetValue(ResolutionProperty, value);
            }
        }

        public TimeRuler Ruler
        {
            get
            {
                return (TimeRuler)GetValue(RulerProperty);
            }
            set
            {
                SetValue(RulerProperty, value);
            }
        }

        public DateTime Start
        {
            get
            {
                return (DateTime)GetValue(StartProperty);
            }
            set
            {
                SetValue(StartProperty, value);
            }
        }

        public double Unit
        {
            get
            {
                return unit;
            }
            set
            {
                unit = value;
            }
        }

        public void Pan(Vector delta)
        {
            HorizontalOffset += delta.X;

            InvalidateVisual();
        }

        public Vector ValidatePanVector(Vector delta)
        {
            var extentWidth = Ruler.ToPixels(Start.ToExtendedDateTime(), End.ToExtendedDateTime());

            if (HorizontalOffset + delta.X < 0)                                                   // Panning too far left.
            {
                delta.X = -HorizontalOffset;                                                      // Set horizontal pan distance to the amount of space remaining before the left edge.
            }

            if (HorizontalOffset + delta.X > extentWidth - ActualWidth)                           // Panning too far right.
            {
                delta.X = extentWidth - (HorizontalOffset + ActualWidth);                         // Set horizontal pan distance to the amount of space remaining before the right edge.
            }

            return delta;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var viewportLeftExtentTimeSpan = Ruler.ToTimeSpan(HorizontalOffset);

            var preciseInitialDate = Start.ToExtendedDateTime() + viewportLeftExtentTimeSpan;
            var roundedInitialDate = (ExtendedDateTime)null;
            var year = 0;

            switch (Resolution)
            {
                case TimeUnit.Century:

                    year = preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Year, roundUp: true).Year;

                    if (year % 100 != 0)                        // round year up to nearest century.
                    {
                        while (year % 100 != 0)
                        {
                            year++;
                        }
                    }

                    roundedInitialDate = new ExtendedDateTime(year);

                    break;

                case TimeUnit.Decade:

                    year = preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Year, roundUp: true).Year;

                    if (year % 10 != 0)                        // round year up to nearest decade.
                    {
                        while (year % 10 != 0)
                        {
                            year++;
                        }
                    }

                    roundedInitialDate = new ExtendedDateTime(year);

                    break;

                case TimeUnit.Year:
                    roundedInitialDate = preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Year, roundUp: true);
                    break;
                case TimeUnit.Month:
                    roundedInitialDate = preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Month, roundUp: true);
                    break;
                case TimeUnit.Day:
                    roundedInitialDate = preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Day, roundUp: true);
                    break;
                case TimeUnit.Hour:
                    roundedInitialDate = preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Hour, roundUp: true);
                    break;
                case TimeUnit.Minute:
                    roundedInitialDate = preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Minute, roundUp: true);
                    break;
                case TimeUnit.Second:
                    roundedInitialDate = preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Second, roundUp: true);
                    break;
                default:
                    break;
            }

            if (roundedInitialDate > preciseInitialDate)
            {
                switch (Resolution)
                {
                    case TimeUnit.Century:
                        DrawLabel(ExtendedDateTimeCalculator.SubtractYears(roundedInitialDate, 100), preciseInitialDate, drawingContext);
                        break;
                    case TimeUnit.Decade:
                        DrawLabel(ExtendedDateTimeCalculator.SubtractYears(roundedInitialDate, 10), preciseInitialDate, drawingContext);
                        break;
                    case TimeUnit.Year:
                        DrawLabel(ExtendedDateTimeCalculator.SubtractYears(roundedInitialDate, 1), preciseInitialDate, drawingContext);
                        break;
                    case TimeUnit.Month:
                        DrawLabel(ExtendedDateTimeCalculator.SubtractMonths(roundedInitialDate, 1), preciseInitialDate, drawingContext);
                        break;
                    case TimeUnit.Day:
                        DrawLabel(roundedInitialDate - TimeSpan.FromDays(1), preciseInitialDate, drawingContext);
                        break;
                    case TimeUnit.Hour:
                        DrawLabel(roundedInitialDate - TimeSpan.FromHours(1), preciseInitialDate, drawingContext);
                        break;
                    case TimeUnit.Minute:
                        DrawLabel(roundedInitialDate - TimeSpan.FromMinutes(1), preciseInitialDate, drawingContext);
                        break;
                    case TimeUnit.Second:
                        DrawLabel(roundedInitialDate - TimeSpan.FromSeconds(1), preciseInitialDate, drawingContext);
                        break;
                    default:
                        break;
                }
            }

            var currentDate = roundedInitialDate;

            while (Ruler.ToPixels(preciseInitialDate, currentDate) < ActualWidth)
            {
                DrawLine(currentDate, preciseInitialDate, drawingContext);
                DrawLabel(currentDate, preciseInitialDate, drawingContext);

                switch (Resolution)
                {
                    case TimeUnit.Century:
                        currentDate = ExtendedDateTimeCalculator.AddYears(currentDate, 100);
                        break;
                    case TimeUnit.Decade:
                        currentDate = ExtendedDateTimeCalculator.AddYears(currentDate, 10);
                        break;
                    case TimeUnit.Year:
                        currentDate = ExtendedDateTimeCalculator.AddYears(currentDate, 1);
                        break;
                    case TimeUnit.Month:
                        currentDate = ExtendedDateTimeCalculator.AddMonths(currentDate, 1);
                        break;
                    case TimeUnit.Day:
                        currentDate += TimeSpan.FromDays(1);
                        break;
                    case TimeUnit.Hour:
                        currentDate += TimeSpan.FromHours(1);
                        break;
                    case TimeUnit.Minute:
                        currentDate += TimeSpan.FromMinutes(1);
                        break;
                    case TimeUnit.Second:
                        currentDate += TimeSpan.FromSeconds(1);
                        break;
                    default:
                        break;
                }
            }
        }

        private void DrawLabel(ExtendedDateTime currentExtendedDateTime, ExtendedDateTime initialExtendedDateTime, DrawingContext drawingContext)
        {
            var formattedText = new FormattedText(currentExtendedDateTime.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(Font), FontSize, Foreground);   // The extended datetime is sourced from the rounded initial date which is already at the desired resolution.

            var origin = new Point(Ruler.ToPixels(initialExtendedDateTime, currentExtendedDateTime) + LabelOffset, ActualHeight * 0.5 - FontSize * 0.6);

            drawingContext.DrawText(formattedText, origin);
        }

        private void DrawLine(ExtendedDateTime currentExtendedDateTime, ExtendedDateTime initialExtendedDateTime, DrawingContext drawingContext)
        {
            var pointX = Ruler.ToPixels(initialExtendedDateTime, currentExtendedDateTime);

            drawingContext.DrawLine(LinePen, new Point(pointX, 0), new Point(pointX, ActualHeight));
        }
    }
}