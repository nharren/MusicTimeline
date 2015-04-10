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

        public EventPanel()
        {
            ClipToBounds = true;
        }

        public EventDisplay EventDisplay
        {
            get
            {
                return (EventDisplay)ItemsControl.GetItemsOwner(this);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (var eventControl in invisibleEvents)
            {
                eventControl.Arrange(new Rect());
            }

            foreach (var visibleEvent in visibleEvents)
            {
                var eventTopExtentOffset = visibleEvent.Key * (EventDisplay.EventHeight + EventDisplay.EventSpacing);                                // visibleEvent.Key is the index of the event.

                var viewportLeftTime = EventDisplay.Start.ToExtendedDateTime() + EventDisplay.Ruler.ToTimeSpan(EventDisplay.HorizontalOffset);

                var eventleftTime = visibleEvent.Value.Dates.Earliest();
                var eventRightTime = visibleEvent.Value.Dates.Latest();

                var eventLeftViewportOffset = EventDisplay.Ruler.ToPixels(viewportLeftTime, eventleftTime);
                var eventTopViewportOffset = eventTopExtentOffset - EventDisplay.VerticalOffset;

                var eventWidth = EventDisplay.Ruler.ToPixels(eventleftTime, eventRightTime);
                var eventHeight = EventDisplay.EventHeight;

                visibleEvent.Value.Arrange(new Rect(eventLeftViewportOffset, eventTopViewportOffset, eventWidth, eventHeight)); 
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            visibleEvents.Clear();
            invisibleEvents.Clear();

            var viewportLeftTime = EventDisplay.Start.ToExtendedDateTime() + EventDisplay.Ruler.ToTimeSpan(EventDisplay.HorizontalOffset);
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
                eventBottomExtentOffset += EventDisplay.EventHeight;
            }

            return base.MeasureOverride(availableSize);
        }
    }
}