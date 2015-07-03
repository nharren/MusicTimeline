using NathanHarrenstein.MusicDb;
using NathanHarrenstein.MusicTimeline.Providers;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class CompositionTreeViewItem : TreeViewItemBase
    {
        private Composition _composition;

        public Composition Composition
        {
            get
            {
                return _composition;
            }

            set
            {
                _composition = value;
            }
        }

        protected override void LoadChildren()
        {
            Children = Composition.Movements
                .OrderBy(m => m.Number)
                .Select(m => MovementTreeViewItemProvider.GetMovementTreeViewItem(m, this));
        }

        protected override void UnloadChildren()
        {
            Children = null;
        }
    }
}