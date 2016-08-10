using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.EDTF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace NathanHarrenstein.Timeline
{
    public class Timeline : Control
    {
        public static readonly DependencyProperty BackgroundImageProperty = DependencyProperty.Register("BackgroundImage", typeof(ImageSource), typeof(Timeline));
        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register("Dates", typeof(ExtendedDateTimeInterval), typeof(Timeline));
        public static readonly DependencyProperty ErasProperty = DependencyProperty.Register("Eras", typeof(ICollection<ITimelineEra>), typeof(Timeline));
        public static readonly DependencyProperty EraTemplatesProperty = DependencyProperty.Register("EraTemplates", typeof(IList), typeof(Timeline));
        public static readonly DependencyProperty EventHeightProperty = DependencyProperty.Register("EventHeight", typeof(double), typeof(Timeline), new PropertyMetadata(26d));
        public static readonly DependencyProperty EventSpacingProperty = DependencyProperty.Register("EventSpacing", typeof(double), typeof(Timeline), new PropertyMetadata(1d));
        public static readonly DependencyProperty EventsProperty = DependencyProperty.Register("Events", typeof(ICollection<ITimelineEvent>), typeof(Timeline));
        public static readonly DependencyProperty EventTemplatesProperty = DependencyProperty.Register("EventTemplates", typeof(IList), typeof(Timeline));
        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(Timeline));
        public static readonly DependencyProperty LineStrokeProperty = DependencyProperty.Register("LineStroke", typeof(Brush), typeof(Timeline), new PropertyMetadata(Brushes.Black));
        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(TimeResolution), typeof(Timeline));
        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(Timeline));
        public static readonly DependencyProperty TimeForegroundProperty = DependencyProperty.Register("TimeForeground", typeof(Brush), typeof(Timeline), new PropertyMetadata(Brushes.Black));
        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(Timeline));


        public GradientStopCollection GradientStops
        {
            get
            {
                return (GradientStopCollection)GetValue(GradientStopsProperty);
            }

            set
            {
                SetValue(GradientStopsProperty, value);
            }
        }

        public static readonly DependencyProperty GradientStopsProperty = DependencyProperty.Register("GradientStops", typeof(GradientStopCollection), typeof(Timeline));



        static Timeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata(typeof(Timeline)));
        }

        public Timeline()
        {
            Eras = new ObservableCollection<ITimelineEra>();
            Events = new SortedSet<ITimelineEvent>(Comparer<ITimelineEvent>.Create((a,b) => a.Dates.Earliest().CompareTo(b.Dates.Earliest())));
            EventTemplates = new List<DataTemplate>();
            EraTemplates = new List<DataTemplate>();
        }

        public ImageSource BackgroundImage
        {
            get
            {
                return (ImageSource)GetValue(BackgroundImageProperty);
            }

            set
            {
                SetValue(BackgroundImageProperty, value);
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

        public ICollection<ITimelineEra> Eras
        {
            get
            {
                return (ICollection<ITimelineEra>)GetValue(ErasProperty);
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

        public ICollection<ITimelineEvent> Events
        {
            get
            {
                return (ICollection<ITimelineEvent>)GetValue(EventsProperty);
            }
            set
            {
                SetValue(EventsProperty, value);
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

        public double HorizontalOffset
        {
            get
            {
                return (double)GetValue(HorizontalOffsetProperty);
            }
            set
            {
                var panGrid = Template.FindName("PART_PanGrid", this) as ScrollingDirector;

                if (panGrid != null)
                {
                    panGrid.Scroll(new Vector(value - HorizontalOffset, 0));
                }
            }
        }

        public Brush LineStroke
        {
            get
            {
                return (Brush)GetValue(LineStrokeProperty);
            }

            set
            {
                SetValue(LineStrokeProperty, value);
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

        public Brush TimeForeground
        {
            get
            {
                return (Brush)GetValue(TimeForegroundProperty);
            }

            set
            {
                SetValue(TimeForegroundProperty, value);
            }
        }

        public double VerticalOffset
        {
            get
            {
                return (double)GetValue(VerticalOffsetProperty);
            }
            set
            {
                var panGrid = Template.FindName("PART_PanGrid", this) as ScrollingDirector;

                if (panGrid != null)
                {
                    panGrid.Scroll(new Vector(0, value - VerticalOffset));
                }
            }
        }
    }
}