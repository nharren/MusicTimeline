using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ExtendedDateTimeFormat;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.Timeline
{
    public class Timeline : Control
    {
        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register("Dates", typeof(ExtendedDateTimeInterval), typeof(Timeline));

        public static readonly DependencyProperty ErasProperty = DependencyProperty.Register("Eras", typeof(IList), typeof(Timeline));

        public static readonly DependencyProperty EventHeightProperty = DependencyProperty.Register("EventHeight", typeof(double), typeof(Timeline), new PropertyMetadata(26d));

        public static readonly DependencyProperty EventsProperty = DependencyProperty.Register("Events", typeof(IList), typeof(Timeline));

        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(TimeUnit), typeof(Timeline));

        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(Timeline));

        static Timeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata(typeof(Timeline)));
        }

        public Timeline()
        {
            Eras = new List<EraControl>();
            Events = new List<EventControl>();
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

        public IList Eras
        {
            get
            {
                return (IList)GetValue(ErasProperty);
            }
            set
            {
                SetValue(ErasProperty, value);
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
        public IList Events
        {
            get
            {
                return (IList)GetValue(EventsProperty);
            }
            set
            {
                SetValue(EventsProperty, value);
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
    }
}