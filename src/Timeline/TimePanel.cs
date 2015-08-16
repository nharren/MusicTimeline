using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.EDTF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NathanHarrenstein.Timeline
{
    public class TimePanel : Panel, IPan
    {
        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register("Dates", typeof(ExtendedDateTimeInterval), typeof(TimePanel), new PropertyMetadata(LayoutPropertyChanged));
        public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(TimePanel), new PropertyMetadata(new FontFamily("Calibri")));
        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register("FontSize", typeof(double), typeof(TimePanel), new PropertyMetadata(12d));
        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush), typeof(TimePanel), new PropertyMetadata(Brushes.Black));
        public static readonly DependencyProperty LabelOffsetProperty = DependencyProperty.Register("LabelOffset", typeof(double), typeof(TimePanel), new PropertyMetadata(10d));
        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(TimeResolution), typeof(TimePanel), new PropertyMetadata(LayoutPropertyChanged));
        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(TimePanel), new PropertyMetadata(TimePanel_PropertyChanged_Ruler));

        private static void TimePanel_PropertyChanged_Ruler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var timePanel = (TimePanel)d;
            timePanel._hasViewChanged = true;

            if (timePanel._labelOffsets != null)
            {
                timePanel._labelOffsets.Clear();
            }

            timePanel.UpdateLayout();
        }

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(TimePanel), new PropertyMetadata(Brushes.Black));

        private bool _hasViewChanged = true;
        private double _horizontalOffset;
        private Dictionary<int, double> _labelOffsets;

        public TimePanel()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                _hasViewChanged = false;
            }
        }

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
                return (FontFamily)GetValue(FontFamilyProperty);
            }

            set
            {
                SetValue(FontFamilyProperty, value);
            }
        }

        public double FontSize
        {
            get
            {
                return (double)GetValue(FontSizeProperty);
            }

            set
            {
                SetValue(FontSizeProperty, value);
            }
        }

        public Brush Foreground
        {
            get
            {
                return (Brush)GetValue(ForegroundProperty);
            }

            set
            {
                SetValue(ForegroundProperty, value);
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
                return (double)GetValue(LabelOffsetProperty);
            }

            set
            {
                SetValue(LabelOffsetProperty, value);
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

        public Brush Stroke
        {
            get
            {
                return (Brush)GetValue(StrokeProperty);
            }

            set
            {
                SetValue(StrokeProperty, value);
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
            InvalidateMeasure();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];

                child.Arrange(new Rect(_labelOffsets[i], (finalSize.Height - child.DesiredSize.Height) / 2, child.DesiredSize.Width, child.DesiredSize.Height));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_hasViewChanged)
            {
                // In determining the labels to display, it must be kept in mind that the spacings between
                // the labels might be variable depending on the current time unit. For instance, if the
                // timeline is measured in months, then the labels will not be equally spaced apart because
                // a month is anywhere from 28 to 31 days. The solution is to produce labels one at a time
                // while the total accumulated width is less than the viewport width.
                //
                // 1. First we first convert the current horizontal offset into a TimeSpan, and add
                // that to the timeline's start time to get the time of the viewport's left edge. This
                // time may be more precise than the timeline's current unit of measurement. For
                // example, the viewport left time may be defined down to the second, but the timeline's
                // unit is an hour. Therefore, we must round up to the nearest unit.
                //
                // 2. The difference between the rounded date and the precise date is the time between
                // the start of the viewport and the initial label. Converting this TimeSpan into pixels
                // will yield the x position of the initial label relative to the viewport's left edge.
                // We can store this value along with the label number for use during the arranging process.
                //
                // 3. Now we can create a label, add it to the panel, and measure it.
                //
                // 4. Next we increment the rounded date by whatever is set as the timeline's unit.
                //
                // 5. Repeat steps 2 to 4 until the rounded date is equal to or exceeds the date of the
                // viewport's right edge.

                Children.Clear();

                if (_labelOffsets == null)
                {
                    _labelOffsets = new Dictionary<int, double>();
                }
                else
                {
                    _labelOffsets.Clear();
                }

                ExtendedDateTime viewportLeftTime = Dates.Earliest() + Ruler.ToTimeSpan(_horizontalOffset);
                ExtendedDateTime viewportRightTime = Dates.Earliest() + Ruler.ToTimeSpan(_horizontalOffset + availableSize.Width);
                ExtendedDateTime labelTime = null;
                int labelIndex = 0;

                switch (Resolution)
                {
                    case TimeResolution.Century:
                        labelTime = new ExtendedDateTime(viewportLeftTime.Year - viewportLeftTime.Year % 100);
                        break;

                    case TimeResolution.Decade:
                        labelTime = new ExtendedDateTime(viewportLeftTime.Year - viewportLeftTime.Year % 10);
                        break;

                    case TimeResolution.Year:
                        labelTime = viewportLeftTime.ToRoundedPrecision(ExtendedDateTimePrecision.Year);
                        break;

                    case TimeResolution.Month:
                        labelTime = viewportLeftTime.ToRoundedPrecision(ExtendedDateTimePrecision.Month, false);
                        break;

                    case TimeResolution.Day:
                        labelTime = viewportLeftTime.ToRoundedPrecision(ExtendedDateTimePrecision.Day, false);
                        break;

                    case TimeResolution.Hour:
                        labelTime = viewportLeftTime.ToRoundedPrecision(ExtendedDateTimePrecision.Hour, false);
                        break;

                    case TimeResolution.Minute:
                        labelTime = viewportLeftTime.ToRoundedPrecision(ExtendedDateTimePrecision.Minute, false);
                        break;

                    case TimeResolution.Second:
                        labelTime = viewportLeftTime.ToRoundedPrecision(ExtendedDateTimePrecision.Second, false);
                        break;

                    default:
                        break;
                }

                while (labelTime < viewportRightTime)
                {
                    var label = new TextBlock();
                    label.Text = labelTime.ToString();
                    label.FontFamily = FontFamily;
                    label.FontSize = FontSize;
                    label.Foreground = Foreground;

                    switch (Resolution)
                    {
                        case TimeResolution.Century:
                            _labelOffsets.Add(labelIndex, Ruler.ToPixels(viewportLeftTime, labelTime) + LabelOffset);
                            labelTime = labelTime.AddYears(100);
                            break;

                        case TimeResolution.Decade:
                            _labelOffsets.Add(labelIndex, Ruler.ToPixels(viewportLeftTime, labelTime) + LabelOffset);
                            labelTime = labelTime.AddYears(10);
                            break;

                        case TimeResolution.Year:
                            _labelOffsets.Add(labelIndex, Ruler.ToPixels(viewportLeftTime, labelTime) + LabelOffset);
                            labelTime = labelTime.AddYears(1);
                            break;

                        case TimeResolution.Month:
                            _labelOffsets.Add(labelIndex, Ruler.ToPixels(viewportLeftTime, labelTime) + LabelOffset);
                            labelTime = labelTime.AddMonths(1);
                            break;

                        case TimeResolution.Day:
                            _labelOffsets.Add(labelIndex, Ruler.ToPixels(viewportLeftTime, labelTime) + LabelOffset);
                            labelTime = labelTime + TimeSpan.FromDays(1);
                            break;

                        case TimeResolution.Hour:
                            _labelOffsets.Add(labelIndex, Ruler.ToPixels(viewportLeftTime, labelTime) + LabelOffset);
                            labelTime = labelTime + TimeSpan.FromHours(1);
                            break;

                        case TimeResolution.Minute:
                            _labelOffsets.Add(labelIndex, Ruler.ToPixels(viewportLeftTime, labelTime) + LabelOffset);
                            labelTime = labelTime + TimeSpan.FromMinutes(1);
                            break;

                        case TimeResolution.Second:
                            _labelOffsets.Add(labelIndex, Ruler.ToPixels(viewportLeftTime, labelTime) + LabelOffset);
                            labelTime = labelTime + TimeSpan.FromSeconds(1);
                            break;

                        default:
                            break;
                    }

                    Children.Add(label);
                    label.Measure(availableSize);

                    labelIndex++;
                }

                _hasViewChanged = false;

                return availableSize;
            }

            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }

            return availableSize;
        }

        private static void LayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var timeDisplay = (TimePanel)d;

            timeDisplay._hasViewChanged = true;
            timeDisplay.InvalidateMeasure();
        }
    }
}