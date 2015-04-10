using NathanHarrenstein.Controls;
using System;
using System.Windows;
using System.Windows.Media;

namespace NathanHarrenstein.Timeline
{
    public class GuidelineDisplay : FrameworkElement, IPan
    {
        private double horizontalOffset;
        private double lineWidth;
        private int majorFrequency;
        private Pen majorPen;
        private Pen minorPen;

        public double LineWidth
        {
            get
            {
                return lineWidth;
            }
            set
            {
                lineWidth = value;
            }
        }

        public int MajorFrequency
        {
            get
            {
                return majorFrequency;
            }
            set
            {
                majorFrequency = value;
            }
        }

        public Pen MajorPen
        {
            get
            {
                return majorPen;
            }
            set
            {
                majorPen = value;
            }
        }

        public Pen MinorPen
        {
            get
            {
                return minorPen;
            }
            set
            {
                minorPen = value;
            }
        }

        private double HorizontalOffset
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

        public Vector ValidatePanVector(Vector delta)
        {
            if (HorizontalOffset + delta.X >= 0)
            {
                return delta;
            }
            else
            {
                return new Vector(-HorizontalOffset, delta.Y);
            }
        }

        public void Pan(Vector delta)
        {
            HorizontalOffset += delta.X;

            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var linePosition = HorizontalOffset / LineWidth;
            var initialLine = (int)Math.Ceiling(linePosition);
            var initialSpace = (initialLine - linePosition) * LineWidth;
            var lineIndex = 0;

            while (lineIndex * LineWidth + initialSpace < ActualWidth)
            {
                var lineX = lineIndex * LineWidth + initialSpace;

                if (MajorFrequency != 0 && (initialLine + lineIndex) % MajorFrequency == 0)
                {
                    drawingContext.DrawLine(MajorPen, new Point(lineX, 0), new Point(lineX, ActualHeight));
                }
                else
                {
                    drawingContext.DrawLine(MinorPen, new Point(lineX, 0), new Point(lineX, ActualHeight));
                }

                lineIndex++;
            }
        }
    }
}