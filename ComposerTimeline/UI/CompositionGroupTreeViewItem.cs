using NathanHarrenstein.ComposerDatabase;
using NathanHarrenstein.ComposerTimeline.UI.Providers;
using NathanHarrenstein.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NathanHarrenstein.ComposerTimeline.UI
{
    public class CompositionGroupTreeViewItem : TreeViewItemBase
    {
        public CompositionGroup CompositionGroup { get; set; }

        protected override void LoadChildren()
        {
            var orderedCompositions = CompositionGroup.Compositions.OrderBy(c =>
            {
                var nameProperty = c.CompositionProperties.FirstOrDefault(cp => cp.Key == "Name");

                return nameProperty.Value;
            });

            Children = orderedCompositions.Select(c =>
            {
                var compositionTreeViewItemProvider = new CompositionTreeViewItemProvider();

                return compositionTreeViewItemProvider.GetCompositionTreeViewItem(c, this);
            });
        }

        protected override void UnloadChildren()
        {
            Children = null;
        }
    }
}