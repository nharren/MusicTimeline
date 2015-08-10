using System;
using System.Collections.Generic;
using System.EDTF;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.Timeline
{
    public class EventPanel : Panel, IPan
    {
        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register("Dates", typeof(ExtendedDateTimeInterval), typeof(EventPanel));
        public static readonly DependencyProperty EventHeightProperty = DependencyProperty.Register("EventHeight", typeof(double), typeof(EventPanel));
        public static readonly DependencyProperty EventSpacingProperty = DependencyProperty.Register("EventSpacing", typeof(double), typeof(EventPanel));
        public static readonly DependencyProperty EventsProperty = DependencyProperty.Register("Events", typeof(IReadOnlyList<ITimelineEvent>), typeof(EventPanel));
        public static readonly DependencyProperty EventTemplatesProperty = DependencyProperty.Register("EventTemplates", typeof(List<DataTemplate>), typeof(EventPanel));
        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(EventPanel));
        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(TimeResolution), typeof(EventPanel));
        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(EventPanel));
        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(EventPanel));

        private FrameworkElement[] _cache;
        private bool _hasViewChanged = true;
        private double _preloadDistance;
        private List<int> _previouslyVisibleCacheIndexes = new List<int>();
        private List<int> _visibleCacheIndexes = new List<int>();

        public EventPanel()
        {
            ClipToBounds = true;
            SizeChanged += EventPanel_SizeChanged;
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

        public IReadOnlyList<ITimelineEvent> Events
        {
            get
            {
                return (IReadOnlyList<ITimelineEvent>)GetValue(EventsProperty);
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

        public List<DataTemplate> EventTemplates // We store the templates in this property as opposed to using a global DataTemplate so that the content is not automatically generated. The DataTemplates are applied only when needed, and the generated content is then stored in the cache.
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

        public double PreloadDistance
        {
            get
            {
                return _preloadDistance;
            }

            set
            {
                _preloadDistance = value;
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

        public Vector CoercePan(Vector delta)
        {
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

        public void Pan(Vector delta)
        {
            HorizontalOffset += delta.X;
            VerticalOffset += delta.Y;

            _hasViewChanged = true;
            _previouslyVisibleCacheIndexes = new List<int>(_visibleCacheIndexes);
            _visibleCacheIndexes.Clear();
            Children.Clear();
            InvalidateMeasure();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (var previouslyVisibleCacheIndex in _previouslyVisibleCacheIndexes)
            {
                _cache[previouslyVisibleCacheIndex].Arrange(new Rect());
            }

            foreach (var visibleCacheIndex in _visibleCacheIndexes)
            {
                _cache[visibleCacheIndex].Arrange(new Rect(
                    Ruler.ToPixels(Dates.Earliest() + Ruler.ToTimeSpan(HorizontalOffset), Events[visibleCacheIndex].Dates.Earliest()),
                    visibleCacheIndex * (EventHeight + EventSpacing) - VerticalOffset,
                    Ruler.ToPixels(Events[visibleCacheIndex].Dates),
                    EventHeight));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (!_hasViewChanged)
            {
                foreach (UIElement child in Children)
                {
                    child.Measure(availableSize);
                }

                return availableSize;
            }

            _hasViewChanged = false;

            if (_cache == null)
            {
                _cache = new FrameworkElement[Events.Count];
            }

            var viewportLeftTime = Dates.Earliest() + Ruler.ToTimeSpan(Math.Max(0d, HorizontalOffset - _preloadDistance));
            var viewportRightTime = viewportLeftTime + Ruler.ToTimeSpan(availableSize.Width + _preloadDistance * 2);

            var lowestPossibleIndex = (int)(Math.Max(VerticalOffset - _preloadDistance, 0) / (EventHeight + EventSpacing));
            var highestPossibleIndex = Math.Min((int)((VerticalOffset + availableSize.Height + _preloadDistance) / (EventHeight + EventSpacing)), Events.Count - 1);

            for (int i = lowestPossibleIndex; i <= highestPossibleIndex; i++)
            {
                var timelineEvent = Events[i];

                if (timelineEvent.Dates.Latest() >= viewportLeftTime && timelineEvent.Dates.Earliest() < viewportRightTime)
                {
                    FrameworkElement eventFrameworkElement = null;

                    if (_cache[i] == null)
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

                        _cache[i] = eventFrameworkElement;
                    }
                    else
                    {
                        eventFrameworkElement = _cache[i];
                    }

                    Children.Add(eventFrameworkElement);

                    eventFrameworkElement.Measure(availableSize);

                    _visibleCacheIndexes.Add(i);

                    if (_previouslyVisibleCacheIndexes.Contains(i))
                    {
                        _previouslyVisibleCacheIndexes.Remove(i);
                    }
                }
            }

            return availableSize;
        }

        private void EventPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _hasViewChanged = true;
            _previouslyVisibleCacheIndexes = new List<int>(_visibleCacheIndexes);
            _visibleCacheIndexes.Clear();
            Children.Clear();
            InvalidateMeasure();
        }
    }
}