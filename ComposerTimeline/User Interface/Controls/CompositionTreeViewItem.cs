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
    public class CompositionTreeViewItem : TreeViewItemBase
    {
        public Composition Composition { get; set; }

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