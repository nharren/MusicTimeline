using System;
using System.Collections.Generic;
using System.ExtendedDateTimeFormat;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NathanHarrenstein.Timeline.UI.Panels
{
    public class EventPanel : Panel
    {
        private Dictionary<int, EventControl> visibleEvents = new Dictionary<int, EventControl>();
        private List<EventControl> invisibleEvents = new List<EventControl>();
        private EventControl[] cache;

        public EventPanel()
        {
            ClipToBounds = true;
            ViewUpdated = true;
        }

        public EventDisplay EventDisplay
        {
            get
            {
                return (EventDisplay)ItemsControl.GetItemsOwner(this);
            }
        }

        internal bool ViewUpdated { get; set; }
        internal List<int> VisibleCacheIndexes { get; set; }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (var eventControl in invisibleEvents)
            {
                eventControl.Arrange(new Rect());
            }

            foreach (var visibleEvent in visibleEvents)
            {
                var eventTopExtentOffset = visibleEvent.Key * (EventDisplay.EventHeight + EventDisplay.EventSpacing);                                // visibleEvent.Key is the index of the event.

                var viewportLeftTime = EventDisplay.Dates.Earliest() + EventDisplay.Ruler.ToTimeSpan(EventDisplay.HorizontalOffset);

                var eventLeftViewportOffset = EventDisplay.Ruler.ToPixels(viewportLeftTime, visibleEvent.Value.Dates.Earliest());
                var eventTopViewportOffset = eventTopExtentOffset - EventDisplay.VerticalOffset;

                var eventWidth = EventDisplay.Ruler.ToPixels(visibleEvent.Value.Dates);
                var eventHeight = EventDisplay.EventHeight;

                visibleEvent.Value.Arrange(new Rect(eventLeftViewportOffset, eventTopViewportOffset, eventWidth, eventHeight)); 
            }

            return finalSize;
        }

        public DataTemplate EventTemplate { get; set; }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (cache == null)
            {
                cache = new EventControl[EventDisplay.Items.Count];
            }

            if (ViewUpdated)
            {
                var highestVisibleCacheIndex = (int)Math.Floor(EventDisplay.HorizontalOffset / (EventDisplay.EventHeight + EventDisplay.EventSpacing));

                if (cache[highestVisibleCacheIndex] == null)
                {
                    var eventControl = EventTemplate.LoadContent() as FrameworkElement;
                }
            }


            
            var viewportLeftTime = EventDisplay.Dates.Earliest() + EventDisplay.Ruler.ToTimeSpan(EventDisplay.HorizontalOffset);
            var viewportRightTime = viewportLeftTime + EventDisplay.Ruler.ToTimeSpan(availableSize.Width);

            var eventIndex = 0;
            var eventTopExtentOffset = 0d;
            var eventBottomExtentOffset = EventDisplay.EventHeight;

            foreach (EventControl eventControl in Children)
            {
                var horizontallyVisible = eventControl.Dates.Latest() > viewportLeftTime && eventControl.Dates.Earliest() < viewportRightTime;
                var verticallyVisible = eventTopExtentOffset < EventDisplay.VerticalOffset + availableSize.Height && eventBottomExtentOffset > EventDisplay.VerticalOffset;

                if (horizontallyVisible && verticallyVisible)
                {
                    eventControl.Measure(availableSize);

                    visibleEvents[eventIndex] = eventControl;
                }
                else
                {
                    invisibleEvents.Add(eventControl);
                }

                eventIndex++;
                eventTopExtentOffset += EventDisplay.EventHeight + EventDisplay.EventSpacing;
                eventBottomExtentOffset += EventDisplay.EventHeight + EventDisplay.EventSpacing + EventDisplay.EventHeight;
            }

            return base.MeasureOverride(availableSize);
        }
    }
}