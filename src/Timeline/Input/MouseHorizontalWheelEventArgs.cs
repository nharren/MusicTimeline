using System;

namespace NathanHarrenstein.Timeline.Input
{
    internal class MouseHorizontalWheelEventArgs : EventArgs
    {
        private readonly int _delta;
        private readonly int _x;
        private readonly int _y;

        public MouseHorizontalWheelEventArgs(int x, int y, int delta)
        {
            _x = x;
            _y = y;
            _delta = delta;
        }

        public int Delta
        {
            get
            {
                return _delta;
            }
        }

        public int X
        {
            get
            {
                return _x;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
        }
    }
}