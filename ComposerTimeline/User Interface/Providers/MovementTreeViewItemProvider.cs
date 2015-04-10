using NathanHarrenstein.Comparers;
using Database;
using NathanHarrenstein.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NathanHarrenstein.ComposerTimeline.UI.Providers
{
    public static class MovementTreeViewItemProvider
    {
        public static MovementTreeViewItem GetMovementTreeViewItem(Movement movement, TreeViewItemBase parent)
        {
            var movementTreeViewItem = new MovementTreeViewItem();

            movementTreeViewItem.Movement = movement;
            movementTreeViewItem.Parent = parent;
            movementTreeViewItem.Header = movement.Name;
            movementTreeViewItem.IsExpanded = new Nullable<bool>(false);
            // movementTreeViewItem.Command = GetCommand(movement);                                                    REQUIREMENTS: Set Audio Directory; Retrieve ID from track.
            movementTreeViewItem.StarVisibility = movement.IsPopular ? Visibility.Visible : Visibility.Collapsed;

            return movementTreeViewItem;
        }

        /*
        private ICommand GetCommand(Movement movement)
        {
            Action<object> play = o =>
            {
                var audioProperty = movement.MovementProperties.FirstOrDefault(mp => mp.Key == "Audio");

                if (audioProperty != null && !string.IsNullOrWhiteSpace(audioProperty.Value))
                {
                    Process.Start(audioProperty.Value);
                }
                else
                {
                    return;
                }
            };

            Predicate<object> canPlay = o =>
            {
                var audioProperty = movement.MovementProperties.FirstOrDefault(mp => mp.Key == "Audio");

                if (audioProperty != null && !string.IsNullOrWhiteSpace(audioProperty.Value))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            };

            return new DelegateCommand(play, canPlay);
        }
        */
    }
}