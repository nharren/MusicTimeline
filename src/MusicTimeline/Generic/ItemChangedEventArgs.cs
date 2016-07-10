using System;

namespace NathanHarrenstein.MusicTimeline.Generic
{
    public class ItemChangedEventArgs<T> : EventArgs
    {
        public ItemChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public T NewValue { get; private set; }
        public T OldValue { get; private set; }
    }
}