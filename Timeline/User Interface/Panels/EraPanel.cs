using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ExtendedDateTimeFormat;

namespace NathanHarrenstein.Timeline.UI.Panels
{
    public class EraPanel : Panel
    {
        private List<EraControl> visibleEras = new List<EraControl>();
        private List<EraControl> invisibleEras = new List<EraControl>();

        public EraPanel()
        {
            ClipToBounds = true;
        }

        public EraDisplay EraDisplay
        {
            get
            {
                return (EraDisplay)ItemsControl.GetItemsOwner(this);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement item in invisibleEras)
	        {
                item.Arrange(new Rect());
	        }

            foreach (var visibleEra in visibleEras)
            {
                var viewportLeftTime = EraDisplay.Start.ToExtendedDateTime() + EraDisplay.Ruler.ToTimeSpan(EraDisplay.HorizontalOffset);

                var eraLeftTime = visibleEra.Dates.Earliest();
                var eraRightTime = visibleEra.Dates.Latest();

                var eraWidth = EraDisplay.Ruler.ToPixels(eraLeftTime, eraRightTime);
                var eraHeight = finalSize.Height;

                var eraLeftViewportOffset = EraDisplay.Ruler.ToPixels(viewportLeftTime, eraLeftTime);

                visibleEra.Arrange(new Rect(eraLeftViewportOffset, 0, eraWidth, eraHeight));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            visibleEras.Clear();

            var viewportLeftTime = EraDisplay.Start.ToExtendedDateTime() + EraDisplay.Ruler.ToTimeSpan(EraDisplay.HorizontalOffset);
            var viewportRightTime = viewportLeftTime + EraDisplay.Ruler.ToTimeSpan(availableSize.Width);

            foreach (EraControl eraControl in Children)
            {
                var horizontallyVisible = eraControl.Dates.End.Latest() > viewportLeftTime && eraControl.Dates.Start.Earliest() < viewportRightTime;

                if (horizontallyVisible)
                {
                    eraControl.Measure(availableSize);

                    visibleEras.Add(eraControl);
                }
                else
                {
                    invisibleEras.Add(eraControl);
                }
            }

            return base.MeasureOverride(availableSize);
        }
    }
}
