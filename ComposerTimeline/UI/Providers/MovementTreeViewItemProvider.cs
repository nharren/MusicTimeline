using NathanHarrenstein.Comparers;
using NathanHarrenstein.ComposerDatabase;
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
    public class MovementTreeViewItemProvider
    {
        public MovementTreeViewItem GetMovementTreeViewItem(Movement movement, TreeViewItemBase parent)
        {
            var movementTreeViewItem = new MovementTreeViewItem();

            movementTreeViewItem.Movement = movement;
            movementTreeViewItem.Parent = parent;
            movementTreeViewItem.Header = GetHeader(movement);
            movementTreeViewItem.IsExpanded = new Nullable<bool>(false);
            movementTreeViewItem.Command = GetCommand(movement);
            movementTreeViewItem.StarVisibility = GetStarVisibility(movement);

            return movementTreeViewItem;
        }

        private Visibility GetStarVisibility(Movement movement)
        {
            var popularProperty = movement.MovementProperties.FirstOrDefault(mp => mp.Key == "Popular");

            if (popularProperty != null && popularProperty.Value == "Yes")
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

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

        private string GetHeader(Movement movement)
        {
            var nameProperty = movement.MovementProperties.FirstOrDefault(mp => mp.Key == "Name");

            return nameProperty.Value;
        }
    }
}