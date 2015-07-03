using System.Collections;
using System.Collections.Generic;
using System.EDTF;
using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.Timeline
{
    public class Timeline : Control
    {
        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register("Dates", typeof(ExtendedDateTimeInterval), typeof(Timeline));
        public static readonly DependencyProperty ErasProperty = DependencyProperty.Register("Eras", typeof(IList), typeof(Timeline));
        public static readonly DependencyProperty EraTemplatesProperty = DependencyProperty.Register("EraTemplates", typeof(IList), typeof(Timeline));
        public static readonly DependencyProperty EventHeightProperty = DependencyProperty.Register("EventHeight", typeof(double), typeof(Timeline), new PropertyMetadata(26d));
        public static readonly DependencyProperty EventsProperty = DependencyProperty.Register("Events", typeof(IList), typeof(Timeline));
        public static readonly DependencyProperty EventTemplatesProperty = DependencyProperty.Register("EventTemplates", typeof(IList), typeof(Timeline));
        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(TimeResolution), typeof(Timeline));
        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(Timeline));

        static Timeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata(typeof(Timeline)));
        }

        public Timeline()
        {
            Eras = new List<ITimelineEra>();
            Events = new List<ITimelineEvent>();
            EventTemplates = new List<DataTemplate>();
            EraTemplates = new List<DataTemplate>();
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

        public IList EraTemplates
        {
            get
            {
                return (IList)GetValue(EraTemplatesProperty);
            }
            set
            {
                SetValue(EraTemplatesProperty, value);
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

        public IList EventTemplates
        {
            get
            {
                return (IList)GetValue(EventTemplatesProperty);
            }
            set
            {
                SetValue(EventTemplatesProperty, value);
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
    }
}