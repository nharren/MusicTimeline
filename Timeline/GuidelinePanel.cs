using System;
using System.Collections.Generic;
using System.EDTF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NathanHarrenstein.Timeline
{
    public class GuidelinePanel : Panel, IPan
    {
        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register("Dates", typeof(ExtendedDateTimeInterval), typeof(GuidelinePanel));
        public static readonly DependencyProperty MajorBrushProperty = DependencyProperty.Register("MajorBrush", typeof(Brush), typeof(GuidelinePanel), new PropertyMetadata(Brushes.Black));
        public static readonly DependencyProperty MajorFrequencyProperty = DependencyProperty.Register("MajorFrequency", typeof(int), typeof(GuidelinePanel), new PropertyMetadata(10));
        public static readonly DependencyProperty MinorBrushProperty = DependencyProperty.Register("MinorBrush", typeof(Brush), typeof(GuidelinePanel), new PropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(TimeResolution), typeof(GuidelinePanel));
        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(GuidelinePanel));
        private bool _hasViewChanged = true;
        private Dictionary<int, double> _lineOffsets;
        private double _horizontalOffset;

        public GuidelinePanel()
        {
            SizeChanged += GuidelinePanel_SizeChanged;
        }

        private void GuidelinePanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _hasViewChanged = true;
            InvalidateMeasure();
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

        public Brush MajorBrush
        {
            get
            {
                return (Brush)GetValue(MajorBrushProperty);
            }

            set
            {
                SetValue(MajorBrushProperty, value);
            }
        }

        public int MajorFrequency
        {
            get
            {
                return (int)GetValue(MajorFrequencyProperty);
            }

            set
            {
                SetValue(MajorFrequencyProperty, value);
            }
        }

        public Brush MinorBrush
        {
            get
            {
                return (Brush)GetValue(MinorBrushProperty);
            }

            set
            {
                SetValue(MinorBrushProperty, value);
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

        public Vector CoercePan(Vector delta)
        {
            var extentWidth = Ruler.ToPixels(Dates);

            if (_horizontalOffset + delta.X < 0)                                                   // Panning too far left.
            {
                delta.X = -_horizontalOffset;                                                      // Set horizontal pan distance to the amount of space remaining before the left edge.
            }

            if (_horizontalOffset + delta.X > extentWidth - ActualWidth)                           // Panning too far right.
            {
                delta.X = extentWidth - (_horizontalOffset + ActualWidth);                         // Set horizontal pan distance to the amount of space remaining before the right edge.
            }

            return delta;
        }

        public void Pan(Vector delta)
        {
            _horizontalOffset += delta.X;

            _hasViewChanged = true;
            InvalidateMeasure();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];

                child.Arrange(new Rect(_lineOffsets[i], 0, child.DesiredSize.Width, child.DesiredSize.Height));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_hasViewChanged)
            {
                // In determining the number of guidelines to display, it must be kept in mind that
                // the spacings between the lines might be variable depending on the
                // current time unit. For instance, if the timeline is measured in months, then the
                // lines will not be equally spaced apart because a month is anywhere from 28 to 31 days.
                // The solution is to produce lines one at a time while the total accumulated width is
                // less than the viewport width.
                //
                // 1. First we first convert the current horizontal offset into a TimeSpan, and add
                // that to the timeline's start time to get the time of the viewport's left edge. This
                // time may be more precise than the timeline's current unit of measurement. For
                // example, the viewport left time may be defined down to the second, but the timeline's
                // unit is an hour. Therefore, we must round up to the nearest unit.
                //
                // 2. The difference between the rounded date and the precise date is the time between
                // the start of the viewport and the initial line. Converting this TimeSpan into pixels
                // will yield the x position of the initial line relative to the viewport's left edge.
                // We can store this value along with the line number for use during the arranging process.
                //
                // 3. Now we can create a guideline, add it to the panel, and measure it.
                //
                // 4. Next we increment the rounded date by whatever is set as the timeline's unit.
                //
                // 5. Repeat steps 2 to 4 until the rounded date is equal to or exceeds the date of the
                // viewport's right edge.

                Children.Clear();

                if (_lineOffsets == null)
                {
                    _lineOffsets = new Dictionary<int, double>();
                }
                else
                {
                    _lineOffsets.Clear();
                }

                ExtendedDateTime viewportLeftTime = Dates.Earliest() + Ruler.ToTimeSpan(_horizontalOffset);
                ExtendedDateTime viewportRightTime = Dates.Earliest() + Ruler.ToTimeSpan(_horizontalOffset + availableSize.Width);
                ExtendedDateTime guidelineTime = null;
                int guidelineIndex = 0;

                switch (Resolution)
                {
                    case TimeResolution.Century:
                        guidelineTime = new ExtendedDateTime(viewportLeftTime.Year - viewportLeftTime.Year % 100 + 100);
                        break;

                    case TimeResolution.Decade:
                        guidelineTime = new ExtendedDateTime(viewportLeftTime.Year - viewportLeftTime.Year % 10 + 10);
                        break;

                    case TimeResolution.Year:
                        guidelineTime = viewportLeftTime.ToPrecision(ExtendedDateTimePrecision.Year, true);
                        break;

                    case TimeResolution.Month:
                        guidelineTime = viewportLeftTime.ToPrecision(ExtendedDateTimePrecision.Month, true);
                        break;

                    case TimeResolution.Day:
                        guidelineTime = viewportLeftTime.ToPrecision(ExtendedDateTimePrecision.Day, true);
                        break;

                    case TimeResolution.Hour:
                        guidelineTime = viewportLeftTime.ToPrecision(ExtendedDateTimePrecision.Hour, true);
                        break;

                    case TimeResolution.Minute:
                        guidelineTime = viewportLeftTime.ToPrecision(ExtendedDateTimePrecision.Minute, true);
                        break;

                    case TimeResolution.Second:
                        guidelineTime = viewportLeftTime.ToPrecision(ExtendedDateTimePrecision.Second, true);
                        break;

                    default:
                        break;
                }

                while (guidelineTime < viewportRightTime)
                {
                    var guideline = new Line();
                    guideline.Y2 = availableSize.Height;
                    guideline.StrokeThickness = 1;
                    guideline.UseLayoutRounding = true;
                    guideline.SnapsToDevicePixels = true;

                    switch (Resolution)
                    {
                        case TimeResolution.Century:
                            _lineOffsets.Add(guidelineIndex, Ruler.ToPixels(viewportLeftTime, guidelineTime));
                            guideline.Stroke = guidelineTime.Year % (100 * MajorFrequency) == 0 ? MajorBrush : MinorBrush;
                            guidelineTime = guidelineTime.AddYears(100);
                            break;

                        case TimeResolution.Decade:
                            _lineOffsets.Add(guidelineIndex, Ruler.ToPixels(viewportLeftTime, guidelineTime));
                            guideline.Stroke = guidelineTime.Year % (10 * MajorFrequency) == 0 ? MajorBrush : MinorBrush;
                            guidelineTime = guidelineTime.AddYears(10);
                            break;

                        case TimeResolution.Year:
                            _lineOffsets.Add(guidelineIndex, Ruler.ToPixels(viewportLeftTime, guidelineTime));
                            guideline.Stroke = guidelineTime.Year % MajorFrequency == 0 ? MajorBrush : MinorBrush;
                            guidelineTime = guidelineTime.AddYears(1);
                            break;

                        case TimeResolution.Month:
                            _lineOffsets.Add(guidelineIndex, Ruler.ToPixels(viewportLeftTime, guidelineTime));
                            guideline.Stroke = guidelineTime.Month % MajorFrequency == 0 ? MajorBrush : MinorBrush;
                            guidelineTime = guidelineTime.AddMonths(1);
                            break;

                        case TimeResolution.Day:
                            _lineOffsets.Add(guidelineIndex, Ruler.ToPixels(viewportLeftTime, guidelineTime));
                            guideline.Stroke = guidelineTime.Day % MajorFrequency == 0 ? MajorBrush : MinorBrush;
                            guidelineTime = guidelineTime + TimeSpan.FromDays(1);
                            break;

                        case TimeResolution.Hour:
                            _lineOffsets.Add(guidelineIndex, Ruler.ToPixels(viewportLeftTime, guidelineTime));
                            guideline.Stroke = guidelineTime.Hour % MajorFrequency == 0 ? MajorBrush : MinorBrush;
                            guidelineTime = guidelineTime + TimeSpan.FromHours(1);
                            break;

                        case TimeResolution.Minute:
                            _lineOffsets.Add(guidelineIndex, Ruler.ToPixels(viewportLeftTime, guidelineTime));
                            guideline.Stroke = guidelineTime.Minute % MajorFrequency == 0 ? MajorBrush : MinorBrush;
                            guidelineTime = guidelineTime + TimeSpan.FromMinutes(1);
                            break;

                        case TimeResolution.Second:
                            _lineOffsets.Add(guidelineIndex, Ruler.ToPixels(viewportLeftTime, guidelineTime));
                            guideline.Stroke = guidelineTime.Second % MajorFrequency == 0 ? MajorBrush : MinorBrush;
                            guidelineTime = guidelineTime + TimeSpan.FromSeconds(1);
                            break;

                        default:
                            break;
                    }

                    Children.Add(guideline);
                    guideline.Measure(availableSize);

                    guidelineIndex++;
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
    }
}