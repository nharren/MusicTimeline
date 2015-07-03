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
        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(TimeResolution), typeof(EventPanel));
        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(EventPanel));

        private FrameworkElement[] _cache;
        private double _horizontalOffset;
        private double _verticalOffset;
        private bool _hasViewChanged = true;
        private List<int> _previouslyVisibleCacheIndexes = new List<int>();
        private List<int> _visibleCacheIndexes = new List<int>();

        public EventPanel()
        {
            ClipToBounds = true;
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

        public List<DataTemplate> EventTemplates                                                            // We store the templates in this property as opposed to using a global DataTemplate so that the content is not automatically generated. The DataTemplates are applied only when needed, and the generated content is then stored in the cache.
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
                return _horizontalOffset;
            }
            set
            {
                _horizontalOffset = value;
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
                return _verticalOffset;
            }
            set
            {
                _verticalOffset = value;
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

            if (HorizontalOffset + delta.X > extentWidth - ActualWidth)                             // Panning too far right. Set horizontal pan distance to the amount of space remaining before the right edge.
            {
                delta.X = extentWidth - (HorizontalOffset + ActualWidth);
            }

            if (VerticalOffset + delta.Y < 0)                                                       // Panning too far up. Set vertical pan distance to the amount of space remaining before the top edge.
            {
                delta.Y = -VerticalOffset;
            }

            if ((VerticalOffset + ActualHeight) + delta.Y > extentHeight)                           // Panning too far down. Set vertical pan distance to the amount of space remaining before the bottom edge.
            {
                delta.Y = Math.Max(0, extentHeight - ((VerticalOffset + ActualHeight) + delta.Y));
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
                var visibleEvent = _cache[visibleCacheIndex];

                var eventTopExtentOffset = visibleCacheIndex * (EventHeight + EventSpacing);

                var viewportLeftTime = Dates.Earliest() + Ruler.ToTimeSpan(HorizontalOffset);

                var eventLeftViewportOffset = Ruler.ToPixels(viewportLeftTime, Events[visibleCacheIndex].Dates.Earliest());
                var eventTopViewportOffset = eventTopExtentOffset - VerticalOffset;

                var eventWidth = Ruler.ToPixels(Events[visibleCacheIndex].Dates);
                var eventHeight = EventHeight;

                visibleEvent.Arrange(new Rect(eventLeftViewportOffset, eventTopViewportOffset, eventWidth, eventHeight));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_cache == null)
            {
                _cache = new FrameworkElement[Events.Count];
            }

            if (_hasViewChanged)
            {
                var viewportLeftTime = Dates.Earliest() + Ruler.ToTimeSpan(HorizontalOffset);
                var viewportRightTime = viewportLeftTime + Ruler.ToTimeSpan(availableSize.Width);

                var highestPossiblyVisibleEventIndex = (int)Math.Floor(VerticalOffset / (EventHeight + EventSpacing));
                var lowestPossiblyVisibleEventIndex = Math.Min((int)Math.Floor((VerticalOffset + availableSize.Height) / (EventHeight + EventSpacing)), Events.Count - 1);

                for (int i = highestPossiblyVisibleEventIndex; i <= lowestPossiblyVisibleEventIndex; i++)
                {
                    var timelineEvent = Events[i];

                    var horizontallyVisible = timelineEvent.Dates.Latest() > viewportLeftTime && timelineEvent.Dates.Earliest() < viewportRightTime;

                    if (horizontallyVisible)
                    {
                        if (_cache[i] == null)
                        {
                            var eventType = timelineEvent.GetType();

                            var template = EventTemplates.FirstOrDefault(et => (Type)et.DataType == eventType);

                            if (template != null)
                            {
                                var eventControl = template.LoadContent() as FrameworkElement;

                                if (eventControl != null)
                                {
                                    eventControl.DataContext = timelineEvent;

                                    _cache[i] = eventControl;

                                    Children.Add(eventControl);
                                }
                                else
                                {
                                    Children.Add(new ContentControl { Content = timelineEvent.ToString() });
                                }
                            }
                            else
                            {
                                Children.Add(new ContentControl { Content = timelineEvent.ToString() });
                            }
                        }
                        else
                        {
                            Children.Add(_cache[i]);
                        }

                        _visibleCacheIndexes.Add(i);

                        if (_previouslyVisibleCacheIndexes.Contains(i))
                        {
                            _previouslyVisibleCacheIndexes.Remove(i);
                        }
                    }
                }

                _hasViewChanged = false;

                InvalidateMeasure();
            }

            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }

            return base.MeasureOverride(availableSize);
        }
    }
}