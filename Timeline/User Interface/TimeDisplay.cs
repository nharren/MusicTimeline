using NathanHarrenstein.Controls;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace NathanHarrenstein.Timeline
{
    public class TimeDisplay : FrameworkElement, IPan
    {
        private string font;
        private double fontSize;
        private Brush foreground;
        private double horizontalOffset;
        private string labelFormat;
        private double labelOffset;
        private Pen linePen;
        private double unit;

        public static readonly DependencyProperty EndProperty = DependencyProperty.Register("End", typeof(double), typeof(TimeDisplay), new PropertyMetadata());
        public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start", typeof(double), typeof(TimeDisplay), new PropertyMetadata());

        public double End
        {
            get
            {
                return (double)GetValue(EndProperty);
            }
            set
            {
                SetValue(EndProperty, value);
            }
        }

        public double Start
        {
            get
            {
                return (double)GetValue(StartProperty);
            }
            set
            {
                SetValue(StartProperty, value);
            }
        }

        public TimeDisplay()
        {
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
                InvalidateVisual();
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

        public double TimeUnitWidth { get; set; }

        public Vector CanPan(Vector requestedDelta)
        {
            var indicatorCount = (End - Start) / Unit;
            var contentWidth = indicatorCount * TimeUnitWidth;
            var maximumHorizontalOffset = contentWidth - ActualWidth;
            var requestedHorizontalOffset = HorizontalOffset + requestedDelta.X;

            if (requestedHorizontalOffset <= maximumHorizontalOffset)
            {
                return requestedDelta;
            }
            else
            {
                var maximumHorizontalDelta = maximumHorizontalOffset - horizontalOffset;
                var comprimisedDelta = new Vector(maximumHorizontalDelta, requestedDelta.Y);

                return comprimisedDelta;
            }
        }

        public void Pan(Vector delta)
        {
            HorizontalOffset += delta.X;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var precisePosition = HorizontalOffset / TimeUnitWidth;
            var roundedPosition = (int)Math.Ceiling(precisePosition);
            var positionOffsetWidth = (roundedPosition - precisePosition) * TimeUnitWidth;
            var positionIncrement = 0;

            if (roundedPosition > precisePosition)
            {
                DrawLabel(positionIncrement - 1, roundedPosition, positionOffsetWidth, drawingContext);
            }

            while (positionIncrement * TimeUnitWidth < ActualWidth)
            {
                DrawLine(positionIncrement, positionOffsetWidth, drawingContext);
                DrawLabel(positionIncrement, roundedPosition, positionOffsetWidth, drawingContext);

                positionIncrement++;
            }
        }

        private void DrawLabel(int positionIncrement, int roundedPosition, double partialPositionWidth, DrawingContext drawingContext)
        {
            drawingContext.DrawText(new FormattedText((Start + (roundedPosition + positionIncrement) * Unit).ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(Font), FontSize, Foreground), new Point(positionIncrement * TimeUnitWidth + partialPositionWidth + LabelOffset, ActualHeight * 0.5 - FontSize * 0.6));
        }

        private void DrawLine(int positionIncrement, double partialPositionWidth, DrawingContext drawingContext)
        {
            var pointX = positionIncrement * TimeUnitWidth + partialPositionWidth;

            drawingContext.DrawLine(LinePen, new Point(pointX, 0), new Point(pointX, ActualHeight));
        }
    }
}