using NathanHarrenstein.Controls;
using System;
using System.Collections.ObjectModel;
using System.ExtendedDateTimeFormat;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace NathanHarrenstein.Timeline
{
    public class EventDisplay : ItemsControl, IPan
    {
        public static readonly DependencyProperty EndProperty = DependencyProperty.Register("End", typeof(DateTime), typeof(EventDisplay));

        public static readonly DependencyProperty EventHeightProperty = DependencyProperty.Register("EventHeight", typeof(double), typeof(EventDisplay));

        public static readonly DependencyProperty EventSpacingProperty = DependencyProperty.Register("EventSpacing", typeof(double), typeof(EventDisplay));

        public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start", typeof(DateTime), typeof(EventDisplay));

        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(EventDisplay));

        private double horizontalOffset;

        private double verticalOffset;

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

        public double EventHeight
        {
            get
            {
                return (double)GetValue(EventHeightProperty);
            }
            set
            {
                SetValue(EventHeightProperty, value);
            }
        }

        public double EventSpacing
        {
            get
            {
                return (double)GetValue(EventSpacingProperty);
            }
            set
            {
                SetValue(EventSpacingProperty, value);
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

        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(TimeUnit), typeof(EventDisplay));



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

        public double VerticalOffset
        {
            get
            {
                return verticalOffset;
            }
            set
            {
                verticalOffset = value;
            }
        }

        protected Panel ItemsHost
        {
            get
            {
                return (Panel)typeof(MultiSelector).InvokeMember("ItemsHost", BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance, null, this, null);
            }
        }

        public Vector? GetCenteringVector(EventControl target)
        {
            if (target == null)                                                             // Only call during or after the arrange pass.
            {
                return null;
            }

            var eventIndex = -1;

            foreach (var eventControl in Items)
            {
                eventIndex++;

                if (eventControl == target)
                {
                    break;
                }
            }

            var eventControlLeftEdge = Ruler.ToPixels(Start.ToExtendedDateTime(), target.Dates.Start.Earliest());
            var eventControlHalfWidth = target.ActualWidth / 2;
            var viewportHalfWidth = ActualWidth / 2;

            var x = eventControlLeftEdge + eventControlHalfWidth - viewportHalfWidth;

            var eventControlTopEdge = eventIndex * (EventHeight + EventSpacing);
            var eventControlHalfHeight = target.ActualHeight / 2;
            var viewportHalfHeight = ActualHeight / 2;

            var y = eventControlTopEdge + eventControlHalfHeight - viewportHalfHeight;

            return new Vector(x - HorizontalOffset, y - VerticalOffset);
        }

        public void Pan(Vector delta)
        {
            HorizontalOffset += delta.X;
            VerticalOffset += delta.Y;

            ItemsHost.InvalidateMeasure();
        }

        public Vector ValidatePanVector(Vector delta)
        {
            var extentWidth = Ruler.ToPixels(Start.ToExtendedDateTime(), End.ToExtendedDateTime());
            var extentHeight = Items.Count * (EventSpacing + EventHeight);

            if (HorizontalOffset + delta.X < 0)                                                     // Panning too far left.
            {
                delta.X = -HorizontalOffset;                                                        // Set horizontal pan distance to the amount of space remaining before the left edge.
            }

            if (HorizontalOffset + delta.X > extentWidth - ActualWidth)                             // Panning too far right.
            {
                delta.X = extentWidth - (HorizontalOffset + ActualWidth);                           // Set horizontal pan distance to the amount of space remaining before the right edge.
            }

            if (VerticalOffset + delta.Y < 0)                                                       // Panning too far up.
            {
                delta.Y = -VerticalOffset;                                                          // Set vertical pan distance to the amount of space remaining before the top edge.
            }

            if ((VerticalOffset + ActualHeight) + delta.Y > extentHeight)                           // Panning too far down.
            {
                delta.Y = Math.Max(0, extentHeight - ((VerticalOffset + ActualHeight) + delta.Y));  // Set vertical pan distance to the amount of space remaining before the bottom edge.
            }

            return delta;
        }
    }
}