using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.Timeline
{
    public delegate void PanRequestedEventHandler(object sender, PanEventArgs e);

    public class PanGrid : Grid
    {
        public static readonly RoutedEvent PanRequestedEvent = EventManager.RegisterRoutedEvent("PanRequested", RoutingStrategy.Bubble, typeof(PanRequestedEventHandler), typeof(PanGrid));

        public PanGrid()
        {
            EventManager.RegisterClassHandler(typeof(PanGrid), PanRequestedEvent, new PanRequestedEventHandler(PanGrid_PanRequested));
        }

        public event PanRequestedEventHandler PanRequested
        {
            add
            {
                AddHandler(PanRequestedEvent, value);
            }
            remove
            {
                RemoveHandler(PanRequestedEvent, value);
            }
        }

        public void Pan(Vector delta)
        {
            foreach (var child in Children)
            {
                var panningChild = child as IPan;

                if (panningChild != null)
                {
                    var coercedPanDelta = panningChild.CoercePan(delta);

                    if (coercedPanDelta != delta)
                    {
                        delta = coercedPanDelta;
                    }
                }
            }

            foreach (var child in Children)
            {
                var panningChild = child as IPan;

                if (panningChild != null)
                {
                    panningChild.Pan(delta);
                }
            }
        }

        private void PanGrid_PanRequested(object sender, PanEventArgs e)
        {
            Pan(e.Delta);
        }
    }
}