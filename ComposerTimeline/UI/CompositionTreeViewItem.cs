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
    public class CompositionTreeViewItem : TreeViewItemBase
    {
        public Composition Composition { get; set; }

        protected override void LoadChildren()
        {
            var orderedMovements = Composition.Movements.OrderBy(m =>
            {
                var numberProperty = m.MovementProperties.FirstOrDefault(mp => mp.Key == "Number");

                return numberProperty.Value;
            });

            Children = orderedMovements.Select(m =>
            {
                var movementTreeViewitemProvier = new MovementTreeViewItemProvider();

                return movementTreeViewitemProvier.GetMovementTreeViewItem(m, this);
            });
        }

        protected override void UnloadChildren()
        {
            Children = null;
        }
    }
}