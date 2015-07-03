using System;
using System.Collections.Generic;
using System.EDTF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NathanHarrenstein.Timeline
{
    public class TimePanel : Panel, IPan
    {
        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register("Dates", typeof(ExtendedDateTimeInterval), typeof(TimePanel), new PropertyMetadata(LayoutPropertyChanged));
        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(TimeResolution), typeof(TimePanel), new PropertyMetadata(LayoutPropertyChanged));
        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(TimePanel), new PropertyMetadata(LayoutPropertyChanged));

        private FrameworkElement[] _cache;
        private FontFamily _fontFamily;
        private double _fontSize;
        private Brush _foreground;
        private double _horizontalOffset;
        private bool _hasViewChanged = true;
        private double _labelOffset;
        private Brush _stroke;
        private ExtendedDateTime _preciseInitialDate;
        private ExtendedDateTime _roundedInitialDate;
        private int _roundedStartIndex;
        private int? _timelineStartIndex;
        private int _viewportEndIndex;
        private int _viewportStartIndex = 0;
        private List<int> _visibleCacheIndexes = new List<int>();

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

        public FontFamily FontFamily
        {
            get
            {
                return _fontFamily;
            }
            set
            {
                _fontFamily = value;
            }
        }

        public double FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                _fontSize = value;
            }
        }

        public Brush Foreground
        {
            get
            {
                return _foreground;
            }
            set
            {
                _foreground = value;
            }
        }

        public double HorizontalOffset
        {
            get
            {
                return _horizontalOffset;
            }
            set
            {
                _horizontalOffset = value;
            }
        }

        public double LabelOffset
        {
            get
            {
                return _labelOffset;
            }
            set
            {
                _labelOffset = value;
            }
        }

        public TimeResolution Resolution
        {
            get
            {
                return (TimeResolution)GetValue(ResolutionProperty);
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

        private int TimelineStartIndex
        {
            get
            {
                if (_timelineStartIndex == null)
                {
                    _timelineStartIndex = Ruler.ToUnitCount(Dates.Earliest() - new ExtendedDateTime(0));
                }

                return _timelineStartIndex.Value;
            }
        }

        public Brush Stroke
        {
            get
            {
                return _stroke;
            }

            set
            {
                _stroke = value;
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

            _hasViewChanged = true;

            _visibleCacheIndexes.Clear();
            Children.Clear();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];

                child.Arrange(new Rect(Ruler.ToPixels(_visibleCacheIndexes[i] - _viewportStartIndex), 0, child.DesiredSize.Width, child.DesiredSize.Height));
            }

            return base.ArrangeOverride(finalSize);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_hasViewChanged)
            {
                _preciseInitialDate = Dates.Earliest() + Ruler.ToTimeSpan(HorizontalOffset);

                var year = 0;

                switch (Resolution)
                {
                    case TimeResolution.Century:
                        year = _preciseInitialDate.Year;
                        year -= year % 100;                                            // round year down to nearest century.
                        _roundedInitialDate = new ExtendedDateTime(year);
                        break;

                    case TimeResolution.Decade:
                        year = _preciseInitialDate.Year;
                        year -= year % 10;                                             // round year down to nearest decade.
                        _roundedInitialDate = new ExtendedDateTime(year);
                        break;

                    case TimeResolution.Year:
                        _roundedInitialDate = new ExtendedDateTime(_preciseInitialDate.Year);
                        break;

                    case TimeResolution.Month:
                        _roundedInitialDate = _preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Month, roundUp: false);
                        break;

                    case TimeResolution.Day:
                        _roundedInitialDate = _preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Day, roundUp: false);
                        break;

                    case TimeResolution.Hour:
                        _roundedInitialDate = _preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Hour, roundUp: false);
                        break;

                    case TimeResolution.Minute:
                        _roundedInitialDate = _preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Minute, roundUp: false);
                        break;

                    case TimeResolution.Second:
                        _roundedInitialDate = _preciseInitialDate.ToPrecision(ExtendedDateTimePrecision.Second, roundUp: false);
                        break;

                    default:
                        break;
                }

                _viewportStartIndex = Ruler.ToUnitCount(HorizontalOffset);                                                              // represents days/hours/minutes/seconds from the timeline start.
                _viewportEndIndex = _viewportStartIndex + Ruler.ToUnitCount(availableSize.Width);
                _roundedStartIndex = _viewportStartIndex + Ruler.ToUnitCount(_roundedInitialDate - _preciseInitialDate);                // calculate days/hours/minutes/seconds the initial date is from the timeline start.

                var currentDate = (ExtendedDateTime)_roundedInitialDate.Clone();
                var currentIndex = _roundedStartIndex;
                var cacheIndex = Ruler.ToUnitCount(currentDate - Dates.Earliest());

                while (Ruler.ToPixels(_preciseInitialDate, currentDate) < availableSize.Width)
                {
                    if (cacheIndex < _cache.Length && _cache[cacheIndex] == null)
                    {
                        var stackPanel = new StackPanel();
                        stackPanel.Orientation = Orientation.Horizontal;

                        var line = new Line();
                        line.Stroke = _stroke;
                        line.Y2 = availableSize.Height;
                        line.StrokeThickness = 1;

                        var label = new TextBlock();
                        label.Text = currentDate.ToString();
                        label.FontFamily = _fontFamily;
                        label.VerticalAlignment = VerticalAlignment.Center;
                        label.FontSize = FontSize;
                        label.Foreground = _foreground;
                        label.Margin = new Thickness(LabelOffset, 0, 0, 0);

                        stackPanel.Children.Add(line);
                        stackPanel.Children.Add(label);

                        Children.Add(stackPanel);
                    }
                    else
                    {
                        Children.Add(_cache[cacheIndex]);
                    }

                    _visibleCacheIndexes.Add(currentIndex);

                    switch (Resolution)
                    {
                        case TimeResolution.Century:
                            currentDate = currentDate.AddYears(100);
                            break;

                        case TimeResolution.Decade:
                            currentDate = currentDate.AddYears(10);
                            break;

                        case TimeResolution.Year:
                            currentDate = currentDate.AddYears(1);
                            break;

                        case TimeResolution.Month:
                            currentDate = currentDate.AddMonths(1);
                            break;

                        case TimeResolution.Day:
                            currentDate += TimeSpan.FromDays(1);
                            break;

                        case TimeResolution.Hour:
                            currentDate += TimeSpan.FromHours(1);
                            break;

                        case TimeResolution.Minute:
                            currentDate += TimeSpan.FromMinutes(1);
                            break;

                        case TimeResolution.Second:
                            currentDate += TimeSpan.FromSeconds(1);
                            break;

                        default:
                            break;
                    }

                    currentIndex = _roundedStartIndex + Ruler.ToUnitCount(currentDate - _roundedInitialDate);
                    cacheIndex = Ruler.ToUnitCount(currentDate - Dates.Earliest());
                }

                _hasViewChanged = false;

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
            var timeDisplay = (TimePanel)d;
            timeDisplay.ResetChildrenCapacity();
            timeDisplay._hasViewChanged = true;
            timeDisplay._visibleCacheIndexes.Clear();
            timeDisplay.Children.Clear();
        }

        private void ResetChildrenCapacity()
        {
            if (Ruler == null)
            {
                return;
            }

            _cache = new StackPanel[Ruler.ToUnitCount(Dates.Span())];
        }
    }
}