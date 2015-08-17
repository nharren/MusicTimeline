using System.Windows;

namespace NathanHarrenstein.Timeline
{
    public class PanEventArgs : RoutedEventArgs
    {
        private readonly Vector _delta;

        public PanEventArgs(Vector delta) : base()
        {
            _delta = delta;
        }

        public PanEventArgs(Vector delta, RoutedEvent routedEvent) : base(routedEvent)
        {
            _delta = delta;
        }

        public PanEventArgs(Vector delta, RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
            _delta = delta;
        }

        public Vector Delta
        {
            get
            {
                return _delta;
            }
        }
    }
}