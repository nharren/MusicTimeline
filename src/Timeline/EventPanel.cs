using NathanHarrenstein.Timeline.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.EDTF;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NathanHarrenstein.Timeline
{
    public class EventPanel : Panel, IScroll
    {
        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register("Dates", typeof(ExtendedDateTimeInterval), typeof(EventPanel));
        public static readonly DependencyProperty EventHeightProperty = DependencyProperty.Register("EventHeight", typeof(double), typeof(EventPanel));
        public static readonly DependencyProperty EventSpacingProperty = DependencyProperty.Register("EventSpacing", typeof(double), typeof(EventPanel));
        public static readonly DependencyProperty EventsProperty = DependencyProperty.Register("Events", typeof(ICollection<ITimelineEvent>), typeof(EventPanel), new PropertyMetadata(new PropertyChangedCallback(EventPanel_EventsChanged)));
        public static readonly DependencyProperty EventTemplatesProperty = DependencyProperty.Register("EventTemplates", typeof(List<DataTemplate>), typeof(EventPanel));
        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(EventPanel));
        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(TimeResolution), typeof(EventPanel));
        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(EventPanel));
        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(EventPanel));

        private FrameworkElement[] cache;
        private bool hasViewChanged = true;
        private double preloadingDistance;
        private List<int> previouslyVisibleCacheIndexes = new List<int>();
        private List<int> visibleCacheIndexes = new List<int>();

        public EventPanel()
        {
            ClipToBounds = true;

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                hasViewChanged = false;
            }
            else
            {
                SizeChanged += EventPanel_SizeChanged;
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

        /// <remarks>
        /// Data templates are stored here so that the content is not automatically generated. 
        /// The data templates are applied only when needed, and then stored in a cache.
        /// </remarks>
        public List<DataTemplate> EventTemplates
        {
            get
            {
                return (List<DataTemplate>)GetValue(EventTemplatesProperty);
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
                SetValue(HorizontalOffsetProperty, value);
            }
        }

        /// <summary>
        /// The distance from the viewport in pixels at which event content is generated.
        /// </summary>
        public double PreloadingDistance
        {
            get
            {
                return preloadingDistance;
            }

            set
            {
                preloadingDistance = value;
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

        public double VerticalOffset
        {
            get
            {
                return (double)GetValue(VerticalOffsetProperty);
            }

            set
            {
                SetValue(VerticalOffsetProperty, value);
            }
        }

        public Vector ReviseScrollingDisplacement(Vector delta)
        {
            if (Ruler == null || Events == null)
            {
                return delta;
            }

            var extentWidth = Ruler.ToPixels(Dates);
            var extentHeight = Events.Count() * (EventSpacing + EventHeight);

            if (HorizontalOffset + delta.X < 0)                                                     // Panning too far left. Set horizontal pan distance to the amount of space remaining before the left edge.
            {
                delta.X = -HorizontalOffset;
            }

            if (HorizontalOffset + ActualWidth + delta.X > extentWidth)                             // Panning too far right. Set horizontal pan distance to the amount of space remaining before the right edge.
            {
                delta.X = extentWidth - HorizontalOffset - ActualWidth;
            }

            if (VerticalOffset + delta.Y < 0)                                                       // Panning too far up. Set vertical pan distance to the amount of space remaining before the top edge.
            {
                delta.Y = -VerticalOffset;
            }

            if (VerticalOffset + ActualHeight + delta.Y > extentHeight)                           // Panning too far down. Set vertical pan distance to the amount of space remaining before the bottom edge.
            {
                delta.Y = extentHeight - VerticalOffset - ActualHeight;
            }

            return delta;
        }

        public Vector? GetCenteringVector(ITimelineEvent target)
        {
            //if (target == null)                                                             // Only call during or after the arrange pass.
            //{
            //    return null;
            //}

            //var eventIndex = 0;

            //foreach (var timelineEvent in Events)
            //{
            //    if (timelineEvent == target)
            //    {
            //        break;
            //    }

            //    eventIndex++;
            //}

            //var eventControlLeftEdge = Ruler.ToPixels(Dates.Earliest(), target.Dates.Start.Earliest());
            //var eventControlHalfWidth = target.ActualWidth / 2;
            //var viewportHalfWidth = ActualWidth / 2;

            //var x = eventControlLeftEdge + eventControlHalfWidth - viewportHalfWidth;

            //var eventControlTopEdge = eventIndex * (EventHeight + EventSpacing);
            //var eventControlHalfHeight = target.ActualHeight / 2;
            //var viewportHalfHeight = ActualHeight / 2;

            //var y = eventControlTopEdge + eventControlHalfHeight - viewportHalfHeight;

            return new Vector(); // new Vector(x - HorizontalOffset, y - VerticalOffset);
        }

        public void Scroll(Vector delta)
        {
            HorizontalOffset += delta.X;
            VerticalOffset += delta.Y;

            hasViewChanged = true;
            previouslyVisibleCacheIndexes = new List<int>(visibleCacheIndexes);
            visibleCacheIndexes.Clear();
            Children.Clear();
            InvalidateMeasure();
        }



        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (var previouslyVisibleCacheIndex in previouslyVisibleCacheIndexes)
            {
                cache[previouslyVisibleCacheIndex].Arrange(new Rect());
            }

            foreach (var visibleCacheIndex in visibleCacheIndexes)
            {
                cache[visibleCacheIndex].Arrange(new Rect(
                    Ruler.ToPixels(Dates.Earliest() + Ruler.ToTimeSpan(HorizontalOffset), Events.ElementAt(visibleCacheIndex).Dates.Earliest()),
                    visibleCacheIndex * (EventHeight + EventSpacing) - VerticalOffset,
                    Ruler.ToPixels(Events.ElementAt(visibleCacheIndex).Dates),
                    EventHeight));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Events == null || Dates == null || Ruler == null)
            {
                return availableSize;
            }

            if (!hasViewChanged)
            {
                foreach (UIElement child in Children)
                {
                    child.Measure(availableSize);
                }

                return availableSize;
            }

            hasViewChanged = false;

            if (cache == null)
            {
                cache = new FrameworkElement[Events.Count];
            }

            var viewportLeftTime = Dates.Earliest() + Ruler.ToTimeSpan(Math.Max(0d, HorizontalOffset - preloadingDistance));
            var viewportRightTime = viewportLeftTime + Ruler.ToTimeSpan(availableSize.Width + preloadingDistance * 2);

            var lowestPossibleIndex = (int)(Math.Max(VerticalOffset - preloadingDistance, 0) / (EventHeight + EventSpacing));
            var highestPossibleIndex = Math.Min((int)((VerticalOffset + availableSize.Height + preloadingDistance) / (EventHeight + EventSpacing)), Events.Count - 1);

            for (int i = lowestPossibleIndex; i <= highestPossibleIndex; i++)
            {
                var timelineEvent = Events.ElementAt(i);

                if (timelineEvent.Dates.Latest() >= viewportLeftTime && timelineEvent.Dates.Earliest() < viewportRightTime)
                {
                    FrameworkElement eventFrameworkElement = null;

                    if (cache[i] == null)
                    {
                        var eventType = timelineEvent.GetType();
                        var template = EventTemplates.FirstOrDefault(et => (Type)et.DataType == eventType);

                        if (template == null || (eventFrameworkElement = template.LoadContent() as FrameworkElement) == null)
                        {
                            var eventContentControl = new ContentControl();
                            eventContentControl.Content = timelineEvent.ToString();

                            eventFrameworkElement = eventContentControl;
                        }

                        eventFrameworkElement.DataContext = timelineEvent;

                        cache[i] = eventFrameworkElement;
                    }
                    else
                    {
                        eventFrameworkElement = cache[i];
                    }

                    Children.Add(eventFrameworkElement);

                    eventFrameworkElement.Measure(availableSize);

                    visibleCacheIndexes.Add(i);

                    if (previouslyVisibleCacheIndexes.Contains(i))
                    {
                        previouslyVisibleCacheIndexes.Remove(i);
                    }
                }
            }

            return availableSize;
        }

       

        private static void EventPanel_EventsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((EventPanel)d).ClearCache();
        }

        private void ClearCache()
        {
            cache = null;
            hasViewChanged = true;
            InvalidateMeasure();
        }



        private void EventPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            hasViewChanged = true;
            previouslyVisibleCacheIndexes = new List<int>(visibleCacheIndexes);
            visibleCacheIndexes.Clear();
            Children.Clear();
            InvalidateMeasure();
        }
    }
}