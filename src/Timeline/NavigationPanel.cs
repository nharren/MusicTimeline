using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.EDTF;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.Timeline
{
    public class NavigationPanel : Panel
    {
        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register("Dates", typeof(ExtendedDateTimeInterval), typeof(NavigationPanel));
        public static readonly DependencyProperty ErasProperty = DependencyProperty.Register("Eras", typeof(ObservableCollection<ITimelineEra>), typeof(NavigationPanel), new PropertyMetadata(new PropertyChangedCallback(ProcessErasChange)));
        public static readonly DependencyProperty EraTemplatesProperty = DependencyProperty.Register("EraTemplates", typeof(List<DataTemplate>), typeof(NavigationPanel), new PropertyMetadata(new PropertyChangedCallback(ProcessEraTemplatesChange)));
        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(TimeResolution), typeof(NavigationPanel));
        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(NavigationPanel));

        public NavigationPanel()
        {
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

        public ObservableCollection<ITimelineEra> Eras
        {
            get
            {
                return (ObservableCollection<ITimelineEra>)GetValue(ErasProperty);
            }
            set
            {
                SetValue(ErasProperty, value);
            }
        }

        public List<DataTemplate> EraTemplates
        {
            get
            {
                return (List<DataTemplate>)GetValue(EraTemplatesProperty);
            }
            set
            {
                SetValue(EraTemplatesProperty, value);
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

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Dates != null )
            {
                var pixelsUsed = 0d;
                var totalSeconds = Dates.Span().TotalSeconds;

                foreach (FrameworkElement child in Children)
                {
                    var era = (ITimelineEra)child.DataContext;
                    var width = Math.Round((era.Dates.Span().TotalSeconds / totalSeconds) * finalSize.Width);

                    child.Arrange(new Rect(
                        pixelsUsed,
                        0,
                        width,
                        finalSize.Height));

                    pixelsUsed += width;
                }
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }

            return base.MeasureOverride(availableSize);
        }

        private static void ProcessErasChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NavigationPanel navigationPanel = (NavigationPanel)d;
            navigationPanel.UpdateChildren();

            var oldEventsCollection = (ObservableCollection<ITimelineEra>)e.OldValue;
            var newEventsCollection = (ObservableCollection<ITimelineEra>)e.NewValue;

            if (oldEventsCollection != null)
            {
                oldEventsCollection.CollectionChanged -= navigationPanel.ProcessEraChange; 
            }

            if (newEventsCollection != null)
            {
                newEventsCollection.CollectionChanged += navigationPanel.ProcessEraChange; 
            }
        }

        private void ProcessEraChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateChildren();
        }

        private static void ProcessEraTemplatesChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NavigationPanel navigationPanel = (NavigationPanel)d;
            navigationPanel.UpdateChildren();
        }

        private void UpdateChildren()
        {
            Children.Clear();

            foreach (var era in Eras.OrderBy(e => e.Dates.Earliest()))
            {
                var eraType = era.GetType();

                var template = EraTemplates?.FirstOrDefault(et => ((Type)et.DataType) == eraType);
                var eraControl = template?.LoadContent() as FrameworkElement;

                if (template == null || eraControl == null)
                {
                    eraControl = new ContentControl
                    {
                        Content = era.ToString(),
                        
                    };
                }

                eraControl.UseLayoutRounding = true;
                eraControl.DataContext = era;

                Children.Add(eraControl);
            }
        }
    }
}