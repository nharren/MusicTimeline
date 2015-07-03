using NathanHarrenstein.MusicDb;
using NathanHarrenstein.MusicTimeline.Providers;
using System.Linq;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class CompositionCollectionTreeViewItem : TreeViewItemBase
    {
        private CompositionCollection _compositionCollection;

        public CompositionCollection CompositionCollection
        {
            get
            {
                return _compositionCollection;
            }

            set
            {
                _compositionCollection = value;
            }
        }

        protected override void LoadChildren()
        {
            Children = CompositionCollection.Compositions
                .OrderBy(c => c.Name)
                .Select(c => CompositionTreeViewItemProvider.GetCompositionTreeViewItem(c, this));
        }

        protected override void UnloadChildren()
        {
            Children = null;
        }
    }
}