using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.Timeline
{
    public class Timeline : Control
    {
        public static readonly DependencyProperty EndProperty = DependencyProperty.Register("End", typeof(DateTime), typeof(Timeline));

        public static readonly DependencyProperty ErasProperty = DependencyProperty.Register("Eras", typeof(IList), typeof(Timeline));

        public static readonly DependencyProperty EventsProperty = DependencyProperty.Register("Events", typeof(IList), typeof(Timeline));

        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(TimeUnit), typeof(Timeline));

        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(Timeline));

        public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start", typeof(DateTime), typeof(Timeline));

        static Timeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata(typeof(Timeline)));
        }

        public Timeline()
        {
            Eras = new List<EraControl>();
            Events = new List<EventControl>();
        }

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
    }
}