using NathanHarrenstein.MusicTimeline.Generic;
using NAudio.Wave;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class Playlist : IList<PlaylistItem>, IDisposable
    {
        private LinkedListNode<PlaylistItem> currentNode;
        private LinkedList<PlaylistItem> internalPlaylist;

        private bool isDisposed;
        private WaveStream currentStream;

        public Playlist()
        {
            internalPlaylist = new LinkedList<PlaylistItem>();
        }

        ~Playlist()
        {
            Dispose(false);
        }

        public event EventHandler<ItemChangedEventArgs<PlaylistItem>> CurrentItemChanged;
        public event EventHandler<ItemAddedEventArgs<PlaylistItem>> ItemAdded;
        public event EventHandler<ItemRemovedEventArgs<PlaylistItem>> ItemRemoved;

        public int Count
        {
            get
            {
                return internalPlaylist.Count;
            }
        }

        public PlaylistItem CurrentItem
        {
            get
            {
                return currentNode?.Value;
            }
        }

        public WaveStream CurrentStream
        {
            get
            {
                return currentStream;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public PlaylistItem this[int index]
        {
            get
            {
                if (index < 0 || index >= internalPlaylist.Count)
                {
                    throw new IndexOutOfRangeException();
                }

                return internalPlaylist.ElementAt(index);
            }

            set
            {
                if (index < 0 || index >= internalPlaylist.Count)
                {
                    throw new IndexOutOfRangeException();
                }

                Insert(index, value);
            }
        }

        public void Add(PlaylistItem playlistItem)
        {
            internalPlaylist.AddLast(playlistItem);

            OnItemAdded(new ItemAddedEventArgs<PlaylistItem>(playlistItem));

            if (internalPlaylist.Count == 1)
            {
                currentNode = internalPlaylist.Last;
                currentStream = currentNode.Value.GetStream();

                OnCurrentItemChanged(new ItemChangedEventArgs<PlaylistItem>(null, CurrentItem));
            }
        }

        public void Clear()
        {
            var playlistItems = internalPlaylist.ToArray();

            foreach (var playlistItem in playlistItems)
            {
                internalPlaylist.Remove(playlistItem);

                OnItemRemoved(new ItemRemovedEventArgs<PlaylistItem>(playlistItem));
            }

            currentStream.Dispose();

            var oldItem = CurrentItem;

            currentNode = null;

            if (oldItem != null)
            {
                OnCurrentItemChanged(new ItemChangedEventArgs<PlaylistItem>(oldItem, null)); 
            }
        }

        public bool Contains(PlaylistItem item)
        {
            return internalPlaylist.Contains(item);
        }

        public void CopyTo(PlaylistItem[] array, int arrayIndex)
        {
            internalPlaylist.CopyTo(array, arrayIndex);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerator<PlaylistItem> GetEnumerator()
        {
            return internalPlaylist.GetEnumerator();
        }

        public void Goto(int index)
        {
            var currentNode = internalPlaylist.First;

            for (int i = 0; i < index; i++)
            {
                currentNode = currentNode.Next;
            }

            var oldItem = this.currentNode.Value;

            this.currentNode = currentNode;

            OnCurrentItemChanged(new ItemChangedEventArgs<PlaylistItem>(oldItem, currentNode.Value));
        }

        public bool HasNext()
        {
            if (currentNode == null || currentNode.Next == null)
            {
                return false;
            }

            return true;
        }

        public bool HasPrevious()
        {
            if (currentNode == null || currentNode.Previous == null)
            {
                return false;
            }

            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)internalPlaylist).GetEnumerator();
        }

        public int IndexOf(PlaylistItem item)
        {
            var increment = 0;

            foreach (var playlistItem in internalPlaylist)
            {
                if (playlistItem == item)
                {
                    return increment;
                }

                increment++;
            }

            throw new InvalidOperationException("The index could not be found for the Mp3PlaylistItem. Check to make sure it is has been added to the playlist.");
        }

        public void Insert(int index, PlaylistItem item)
        {
            var currentNode = internalPlaylist.First;

            for (int i = 0; i < index; i++)
            {
                currentNode = currentNode.Next;
            }

            internalPlaylist.AddBefore(currentNode, item);

            OnItemAdded(new ItemAddedEventArgs<PlaylistItem>(item));
        }

        public void Refresh()
        {
            if (currentStream != null)
            {
                currentStream.Dispose();
            }

            currentStream = currentNode.Value.GetStream();
        }

        public bool Next()
        {
            if (currentNode.Next == null)
            {
                return false;
            }

            if (currentStream != null)
            {
                currentStream.Dispose();
            }

            currentNode = currentNode.Next;
            currentStream = currentNode.Value.GetStream();

            OnCurrentItemChanged(new ItemChangedEventArgs<PlaylistItem>(currentNode.Previous.Value, CurrentItem));

            return true;
        }

        public bool Previous()
        {
            if (currentNode.Previous == null)
            {
                return false;
            }

            if (currentStream != null)
            {
                currentStream.Dispose();
            }

            currentNode = currentNode.Previous;

            OnCurrentItemChanged(new ItemChangedEventArgs<PlaylistItem>(currentNode.Next.Value, CurrentItem));

            return true;
        }

        public bool Remove(PlaylistItem item)
        {
            var oldNode = currentNode;
            var result = internalPlaylist.Remove(item);

            OnItemRemoved(new ItemRemovedEventArgs<PlaylistItem>(item));

            currentNode = currentNode.Next ?? currentNode.Previous;

            if (currentStream != null)
            {
                currentStream.Dispose();
            }

            currentStream = currentNode.Value.GetStream();

            OnCurrentItemChanged(new ItemChangedEventArgs<PlaylistItem>(currentNode.Previous.Value, CurrentItem));

            return result;
        }

        public void RemoveAt(int index)
        {
            var currentNode = internalPlaylist.First;

            for (int i = 0; i < index; i++)
            {
                currentNode = currentNode.Next;
            }

            Remove(currentNode.Value);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    if (currentStream != null)
                    {
                        currentStream.Dispose();
                    }
                }               

                isDisposed = true;
            }
        }

        protected virtual void OnCurrentItemChanged(ItemChangedEventArgs<PlaylistItem> e)
        {
            if (CurrentItemChanged != null)
            {
                CurrentItemChanged(this, e);
            }
        }

        protected virtual void OnItemAdded(ItemAddedEventArgs<PlaylistItem> e)
        {
            if (ItemAdded != null)
            {
                ItemAdded(this, e);
            }
        }

        protected virtual void OnItemRemoved(ItemRemovedEventArgs<PlaylistItem> e)
        {
            if (ItemRemoved != null)
            {
                ItemRemoved(this, e);
            }
        }
    }
}