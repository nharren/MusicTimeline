using NathanHarrenstein.Controls;
using System;
using System.Collections.Generic;
using System.ExtendedDateTimeFormat;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NathanHarrenstein.Timeline
{
    public class TimeDisplay : Panel, IPan
    {
        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register("Dates", typeof(ExtendedDateTimeInterval), typeof(TimeDisplay), new PropertyMetadata(LayoutPropertyChanged));

        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(TimeUnit), typeof(TimeDisplay), new PropertyMetadata(LayoutPropertyChanged));

        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(TimeDisplay), new PropertyMetadata(LayoutPropertyChanged));

        private StackPanel[] cache;

        private string font;

        private double fontSize;

        private Brush foreground;

        private double horizontalOffset;

        private bool viewUpdated = true;

        private string labelFormat;

        private double labelOffset;

        private Pen linePen;

        private ExtendedDateTime preciseInitialDate;

        private ExtendedDateTime roundedInitialDate;

        private int roundedStartIndex;

        private int? timelineStartIndex;

        private double unit;

        private int viewportEndIndex;

        private int viewportStartIndex = 0;

        private List<int> visibleCacheIndexes = new List<int>();

        public ExtendedDateTimeInterval Dates
        {
            get
            {
                return (ExtendedDateTimeInterval)GetValue(DatesProperty);
            }
            set
            {
                SetValue(DatesProperty, value);
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

        private int TimelineStartIndex
        {
            get
            {
                if (timelineStartIndex == null)
                {
                    timelineStartIndex = Ruler.ToUnitCount(Dates.Earliest() - new ExtendedDateTime(0));
                }

                return timelineStartIndex.Value;
            }
        }

        public Vector CoercePan(Vector delta)
        {
            var extentWidth = Ruler.ToPixels(Dates);

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

        public void Pan(Vector delta)
        {
            HorizontalOffset += delta.X;

            viewUpdated = true;

            visibleCacheIndexes.Clear();
            Children.Clear();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];

                child.Arrange(new Rect(Ruler.ToPixels(visibleCacheIndexes[i] - viewportStartIndex), 0, child.DesiredSize.Width, child.DesiredSize.Height));

            }

            return base.ArrangeOverride(finalSize);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (viewUpdated)
            {
                preciseInitialDate = Dates.Earliest() + Ruler.ToTimeSpan(HorizontalOffset);

                var year = 0;

                switch (Resolution)
                {
                    case TimeUnit.Century:

                        year = preciseInitialDate.Year;
                        year -= year % 100;                                            // round year down to nearest century.
                        roundedInitialDate = new ExtendedDateTime(year);
                        break;

                    case TimeUnit.Decade:

                        year = preciseInitialDate.Year;
                        year -= year % 10;                                             // round year down to nearest decade.
                        roundedInitialDate = new ExtendedDateTime(year);
                        break;

                    case TimeUnit.Year:

                        roundedInitialDate = new ExtendedDateTime(preciseInitialDate.Year);
                        break;

                    case TimeUnit.Month:

                        roundedInitialDate = preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Month, roundUp: false);
                        break;

                    case TimeUnit.Day:

                        roundedInitialDate = preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Day, roundUp: false);
                        break;

                    case TimeUnit.Hour:

                        roundedInitialDate = preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Hour, roundUp: false);
                        break;

                    case TimeUnit.Minute:

                        roundedInitialDate = preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Minute, roundUp: false);
                        break;

                    case TimeUnit.Second:

                        roundedInitialDate = preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Second, roundUp: false);
                        break;

                    default:

                        break;
                }

                viewportStartIndex = Ruler.ToUnitCount(HorizontalOffset);                                                           // represents days/hours/minutes/seconds from the timeline start.
                viewportEndIndex = viewportStartIndex + Ruler.ToUnitCount(availableSize.Width);

                roundedStartIndex = viewportStartIndex + Ruler.ToUnitCount(roundedInitialDate - preciseInitialDate);                // calculate days/hours/minutes/seconds the initial date is from the timeline start.

                var currentDate = roundedInitialDate;
                var currentIndex = roundedStartIndex;
                var cacheIndex = Ruler.ToUnitCount(currentDate - Dates.Earliest());

                while (Ruler.ToPixels(preciseInitialDate, currentDate) < availableSize.Width)
                {
                    if (cache[cacheIndex] == null)
                    {
                        var stackPanel = new StackPanel();
                        stackPanel.Orientation = Orientation.Horizontal;

                        var line = new Line();
                        line.Stroke = LinePen.Brush;
                        line.Y2 = availableSize.Height;
                        line.StrokeThickness = 1;

                        var label = new TextBlock();
                        label.Text = currentDate.ToString();
                        label.FontFamily = new FontFamily(font);
                        label.VerticalAlignment = VerticalAlignment.Center;
                        label.FontSize = FontSize;
                        label.Foreground = foreground;
                        label.Margin = new Thickness(LabelOffset, 0, 0, 0);

                        stackPanel.Children.Add(line);
                        stackPanel.Children.Add(label);

                        Children.Add(stackPanel); 
                    }
                    else
                    {
                        Children.Add(cache[cacheIndex]);
                    }

                    visibleCacheIndexes.Add(currentIndex);

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

                    currentIndex = roundedStartIndex + Ruler.ToUnitCount(currentDate - roundedInitialDate);
                    cacheIndex = Ruler.ToUnitCount(currentDate - Dates.Earliest());
                }

                viewUpdated = false;

                InvalidateMeasure();
            }

            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }

            return base.MeasureOverride(availableSize);
        }

        private static void LayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var timeDisplay = (TimeDisplay)d;
            timeDisplay.ResetChildrenCapacity();
            timeDisplay.viewUpdated = true;
            timeDisplay.visibleCacheIndexes.Clear();
            timeDisplay.Children.Clear();
        }

        private void ResetChildrenCapacity()
        {
            if (Ruler == null)
            {
                return;
            }

 	         cache = new StackPanel[Ruler.ToUnitCount(Dates.Span())];
        }
    }
}