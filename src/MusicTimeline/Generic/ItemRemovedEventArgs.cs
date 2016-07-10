using System;

namespace NathanHarrenstein.MusicTimeline.Generic
{
    public class ItemRemovedEventArgs<T> : EventArgs
    {
        public ItemRemovedEventArgs(T removedItem)
        {
            RemovedItem = removedItem;
        }

        public T RemovedItem { get; private set; }
    }
}