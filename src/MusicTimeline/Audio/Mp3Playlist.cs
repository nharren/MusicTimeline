using NathanHarrenstein.MusicTimeline.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class Mp3Playlist : IList<Mp3PlaylistItem>, IDisposable
    {
        private LinkedListNode<Mp3PlaylistItem> currentNode;
        private LinkedList<Mp3PlaylistItem> internalMp3Playlist;

        private bool isDisposed;

        public Mp3Playlist()
        {
            internalMp3Playlist = new LinkedList<Mp3PlaylistItem>();
        }

        ~Mp3Playlist()
        {
            Dispose(false);
        }

        public event EventHandler<ItemChangedEventArgs<Mp3PlaylistItem>> CurrentItemChanged;
        public event EventHandler<ItemAddedEventArgs<Mp3PlaylistItem>> ItemAdded;
        public event EventHandler<ItemRemovedEventArgs<Mp3PlaylistItem>> ItemRemoved;

        public int Count
        {
            get
            {
                return internalMp3Playlist.Count;
            }
        }

        public Mp3PlaylistItem CurrentItem
        {
            get
            {
                return currentNode?.Value;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public Mp3PlaylistItem this[int index]
        {
            get
            {
                if (index < 0 || index >= internalMp3Playlist.Count)
                {
                    throw new IndexOutOfRangeException();
                }

                return internalMp3Playlist.ElementAt(index);
            }

            set
            {
                if (index < 0 || index >= internalMp3Playlist.Count)
                {
                    throw new IndexOutOfRangeException();
                }

                Insert(index, value);
            }
        }

        public void Add(Mp3PlaylistItem mp3PlaylistItem)
        {
            internalMp3Playlist.AddLast(mp3PlaylistItem);

            OnItemAdded(new ItemAddedEventArgs<Mp3PlaylistItem>(mp3PlaylistItem));

            if (internalMp3Playlist.Count == 1)
            {
                currentNode = internalMp3Playlist.Last;

                OnCurrentItemChanged(new ItemChangedEventArgs<Mp3PlaylistItem>(null, CurrentItem));
            }
        }

        public void Clear()
        {
            var playlistItems = internalMp3Playlist.ToArray();

            foreach (var playlistItem in playlistItems)
            {
                playlistItem.Stream.Dispose();

                internalMp3Playlist.Remove(playlistItem);

                OnItemRemoved(new ItemRemovedEventArgs<Mp3PlaylistItem>(playlistItem));
            }

            var oldItem = CurrentItem;

            currentNode = null;

            if (oldItem != null)
            {
                OnCurrentItemChanged(new ItemChangedEventArgs<Mp3PlaylistItem>(oldItem, null)); 
            }
        }

        public bool Contains(Mp3PlaylistItem item)
        {
            return internalMp3Playlist.Contains(item);
        }

        public void CopyTo(Mp3PlaylistItem[] array, int arrayIndex)
        {
            internalMp3Playlist.CopyTo(array, arrayIndex);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerator<Mp3PlaylistItem> GetEnumerator()
        {
            return internalMp3Playlist.GetEnumerator();
        }

        public void Goto(int index)
        {
            var currentNode = internalMp3Playlist.First;

            for (int i = 0; i < index; i++)
            {
                currentNode = currentNode.Next;
            }

            var oldItem = this.currentNode.Value;

            this.currentNode = currentNode;

            OnCurrentItemChanged(new ItemChangedEventArgs<Mp3PlaylistItem>(oldItem, currentNode.Value));
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
            return ((IEnumerable)internalMp3Playlist).GetEnumerator();
        }

        public int IndexOf(Mp3PlaylistItem item)
        {
            var increment = 0;

            foreach (var mp3PlaylistItem in internalMp3Playlist)
            {
                if (mp3PlaylistItem == item)
                {
                    return increment;
                }

                increment++;
            }

            throw new InvalidOperationException("The index could not be found for the Mp3PlaylistItem. Check to make sure it is has been added to the playlist.");
        }

        public void Insert(int index, Mp3PlaylistItem item)
        {
            var currentNode = internalMp3Playlist.First;

            for (int i = 0; i < index; i++)
            {
                currentNode = currentNode.Next;
            }

            internalMp3Playlist.AddBefore(currentNode, item);

            OnItemAdded(new ItemAddedEventArgs<Mp3PlaylistItem>(item));
        }

        public bool Next()
        {
            if (currentNode.Next == null)
            {
                return false;
            }

            currentNode = currentNode.Next;

            OnCurrentItemChanged(new ItemChangedEventArgs<Mp3PlaylistItem>(currentNode.Previous.Value, CurrentItem));

            return true;
        }

        public bool Previous()
        {
            if (currentNode.Previous == null)
            {
                return false;
            }

            currentNode = currentNode.Previous;

            OnCurrentItemChanged(new ItemChangedEventArgs<Mp3PlaylistItem>(currentNode.Next.Value, CurrentItem));

            return true;
        }

        public bool Remove(Mp3PlaylistItem item)
        {
            var oldNode = currentNode;

            if (item == oldNode.Value)
            {
                currentNode = oldNode.Next;

                OnCurrentItemChanged(new ItemChangedEventArgs<Mp3PlaylistItem>(oldNode.Value, CurrentItem));
            }

            var result = internalMp3Playlist.Remove(item);

            OnItemRemoved(new ItemRemovedEventArgs<Mp3PlaylistItem>(item));

            return result;
        }

        public void RemoveAt(int index)
        {
            var currentNode = internalMp3Playlist.First;

            for (int i = 0; i < index; i++)
            {
                currentNode = currentNode.Next;
            }

            internalMp3Playlist.Remove(currentNode);

            OnItemRemoved(new ItemRemovedEventArgs<Mp3PlaylistItem>(currentNode.Value));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                }

                foreach (var playlistItem in internalMp3Playlist)
                {
                    playlistItem.Stream.Dispose();
                }

                isDisposed = true;
            }
        }

        protected virtual void OnCurrentItemChanged(ItemChangedEventArgs<Mp3PlaylistItem> e)
        {
            if (CurrentItemChanged != null)
            {
                CurrentItemChanged(this, e);
            }
        }

        protected virtual void OnItemAdded(ItemAddedEventArgs<Mp3PlaylistItem> e)
        {
            if (ItemAdded != null)
            {
                ItemAdded(this, e);
            }
        }

        protected virtual void OnItemRemoved(ItemRemovedEventArgs<Mp3PlaylistItem> e)
        {
            if (ItemRemoved != null)
            {
                ItemRemoved(this, e);
            }
        }
    }
}