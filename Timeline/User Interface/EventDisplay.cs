using NathanHarrenstein.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace NathanHarrenstein.Timeline
{
    public class EventDisplay : FrameworkElement, IPan
    {
        public static readonly DependencyProperty EndProperty = DependencyProperty.Register("End", typeof(double), typeof(EventDisplay));
        public static readonly DependencyProperty EraSettingsProperty = DependencyProperty.Register("EraSettings", typeof(List<EraSettings>), typeof(EventDisplay));
        public static readonly DependencyProperty ErasProperty = DependencyProperty.Register("Eras", typeof(List<Era>), typeof(EventDisplay));
        public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start", typeof(double), typeof(EventDisplay));

        private double eventHeight;
        private double eventRadiusX;
        private double eventRadiusY;
        private double eventSpacing;
        private string font;
        private double fontSize;
        private Brush foreground;
        private double horizontalOffset;
        private double labelOffset;
        private double verticalOffset;

        public EventDisplay()
        {
            SetValue(ErasProperty, new List<Era>());
            SetValue(EraSettingsProperty, new List<EraSettings>());

            ClipToBounds = true;
        }

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

        public List<EraSettings> EraSettings
        {
            get
            {
                return (List<EraSettings>)GetValue(EraSettingsProperty);
            }
            set
            {
                SetValue(EraSettingsProperty, value);
            }
        }

        public List<Era> Eras
        {
            get
            {
                return (List<Era>)GetValue(ErasProperty);
            }
            set
            {
                SetValue(ErasProperty, value);
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

        public double EventHeight
        {
            get
            {
                return eventHeight;
            }
            set
            {
                eventHeight = value;
            }
        }

        public double EventRadiusX
        {
            get
            {
                return eventRadiusX;
            }
            set
            {
                eventRadiusX = value;
            }
        }

        public double EventRadiusY
        {
            get
            {
                return eventRadiusY;
            }
            set
            {
                eventRadiusY = value;
            }
        }

        public double EventSpacing
        {
            get
            {
                return eventSpacing;
            }
            set
            {
                eventSpacing = value;
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
                InvalidateVisual();
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

        public double TimeUnitWidth { get; set; }

        public double VerticalOffset
        {
            get
            {
                return verticalOffset;
            }
            set
            {
                verticalOffset = value;
                InvalidateVisual();
            }
        }

        public Vector CanPan(Vector delta)
        {
            var coercedX = delta.X;
            var coercedY = delta.Y;

            if (HorizontalOffset + coercedX < 0)
            {
                coercedX = -HorizontalOffset;
            }

            if (HorizontalOffset + coercedX > End * TimeUnitWidth - ActualWidth)
            {
                coercedX = End * TimeUnitWidth - ActualWidth - HorizontalOffset;
            }

            if (VerticalOffset + coercedY < 0)
            {
                coercedY = -VerticalOffset;
            }

            var eventCount = Eras.SelectMany(era => era.Events).Count();
            var extentHeight = eventCount * (EventSpacing + EventHeight);
            var visibleBottom = VerticalOffset + ActualHeight;
            var pansPastExtentHeight = VerticalOffset > 0 && visibleBottom + delta.Y > extentHeight;

            if (pansPastExtentHeight)
            {
                coercedY = 0;
            }

            delta.X = coercedX;
            delta.Y = coercedY;

            return delta;
        }

        public void Pan(Vector delta)
        {
            HorizontalOffset += delta.X;
            VerticalOffset += delta.Y;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var sortedEras = Eras.OrderByDescending(era => era.Start);
            var leftEdgeTime = Start + HorizontalOffset / TimeUnitWidth;
            var rightEdgeTime = leftEdgeTime + ActualWidth / TimeUnitWidth;
            var rendering = false;
            var eventIndex = 0;

            using (drawingContext)
            {
                foreach (var era in Eras)
                {
                    var visible = era.Start < rightEdgeTime + 100 && era.End >= leftEdgeTime - 100;

                    if (visible)
                    {
                        rendering = true;

                        foreach (var eventItem in era.Events)
                        {
                            var eventTop = eventIndex * (EventHeight + EventSpacing);
                            var eventBottom = eventTop + EventHeight;
                            var bottomEdge = VerticalOffset + ActualHeight;
                            var topEdge = VerticalOffset;

                            var horizontallyVisible = eventItem.End >= leftEdgeTime;
                            var verticallyVisible = eventTop < bottomEdge && eventBottom >= topEdge;

                            if (horizontallyVisible && verticallyVisible)
                            {
                                DrawEvent(eventItem, eventIndex, leftEdgeTime, era, drawingContext);
                                DrawLabel(eventItem, eventIndex, leftEdgeTime, era, drawingContext);
                            }

                            eventIndex++;
                        }
                    }
                    else if (rendering)
                    {
                        break;
                    }
                    else
                    {
                        eventIndex += era.Events.Count;
                    }
                }
            }
        }

        private void DrawEvent(Event eventItem, int eventIndex, double timePosition, Era era, DrawingContext drawingContext)
        {
            var brush = (Brush)Brushes.Black;

            foreach (var eraSettings in EraSettings)
            {
                if (eraSettings.Era == era.Name)
                {
                    brush = eraSettings.Brush;

                    break;
                }
            }

            var verticalPosition = eventIndex * (EventHeight + EventSpacing);

            var pointX = (eventItem.Start - timePosition) * TimeUnitWidth;
            var pointY = verticalPosition - VerticalOffset;
            var point = new Point(pointX, pointY);

            var width = (eventItem.End - eventItem.Start) * TimeUnitWidth;
            var height = EventHeight;
            var size = new Size(width, height);

            var rect = new Rect(point, size);

            drawingContext.DrawRoundedRectangle(brush, null, rect, EventRadiusX, EventRadiusY);
        }

        private void DrawLabel(Event eventItem, int eventIndex, double timePosition, Era era, DrawingContext drawingContext)
        {
            var labelText = eventItem.Name;
            var labelFont = new Typeface(Font);
            var label = new FormattedText(labelText, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, labelFont, FontSize, Foreground);

            var verticalPosition = eventIndex * (EventHeight + EventSpacing);

            var pointX = (eventItem.Start - timePosition) * TimeUnitWidth + LabelOffset;
            var pointY = verticalPosition - VerticalOffset + (EventHeight - FontSize) * 0.33;
            var point = new Point(pointX, pointY);

            drawingContext.DrawText(label, point);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            Console.WriteLine("Mouse Clicked");
            base.OnMouseDown(e);
        }
    }
}