using NathanHarrenstein.MusicDb;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class MovementTreeViewItem : TreeViewItemBase
    {
        private Movement _movement;

        public Movement Movement
        {
            get
            {
                return _movement;
            }

            set
            {
                _movement = value;
            }
        }
    }
}