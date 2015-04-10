using Database;
using NathanHarrenstein.ComposerTimeline.UI.Providers;
using NathanHarrenstein.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NathanHarrenstein.ComposerTimeline.UI
{
    public class CompositionCollectionTreeViewItem : TreeViewItemBase
    {
        public CompositionCollection CompositionCollection { get; set; }

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