using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.Timeline
{
    public delegate void ScrollRequestEventHandler(object sender, ScrollingEventArgs e);

    public class ScrollingDirector : Grid
    {
        public static readonly RoutedEvent ScrollRequestEvent = EventManager.RegisterRoutedEvent("ScrollRequest", RoutingStrategy.Bubble, typeof(ScrollRequestEventHandler), typeof(ScrollingDirector));

        public ScrollingDirector()
        {
            EventManager.RegisterClassHandler(typeof(ScrollingDirector), ScrollRequestEvent, new ScrollRequestEventHandler(ScrollingDirector_ScrollRequested));
        }

        public event ScrollRequestEventHandler ScrollRequest
        {
            add
            {
                AddHandler(ScrollRequestEvent, value);
            }
            remove
            {
                RemoveHandler(ScrollRequestEvent, value);
            }
        }

        public void Scroll(Vector displacement)
        {
            displacement = ReviseScrollingDisplacement(displacement);

            foreach (var child in Children)
            {
                var scrollingChild = child as IScroll;

                if (scrollingChild != null)
                {
                    scrollingChild.Scroll(displacement);
                }
            }
        }

        private Vector ReviseScrollingDisplacement(Vector displacement)
        {
            foreach (var child in Children)
            {
                var scrollingChild = child as IScroll;

                if (scrollingChild != null)
                {
                    displacement = scrollingChild.ReviseScrollingDisplacement(displacement);
                }
            }

            return displacement;
        }

        private void ScrollingDirector_ScrollRequested(object sender, ScrollingEventArgs e)
        {
            Scroll(e.Delta);
        }
    }
}