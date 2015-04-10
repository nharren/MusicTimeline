using NathanHarrenstein.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ExtendedDateTimeFormat;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace NathanHarrenstein.Timeline
{
    public class EraDisplay : ItemsControl, IPan
    {
        public static readonly DependencyProperty EndProperty = DependencyProperty.Register("End", typeof(DateTime), typeof(EraDisplay));

        public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start", typeof(DateTime), typeof(EraDisplay));

        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(EraDisplay));

        private double horizontalOffset;

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

        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(TimeUnit), typeof(EraDisplay));



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

        protected Panel ItemsHost
        {
            get
            {
                return (Panel)typeof(MultiSelector).InvokeMember("ItemsHost", BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance, null, this, null);
            }
        }

        public void Pan(Vector delta)
        {
            HorizontalOffset += delta.X;

            ItemsHost.InvalidateMeasure();
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
    }
}