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
    public class CompositionGroupTreeViewItemProvider
    {
        public CompositionGroupTreeViewItem GetCompositionGroupTreeViewItem(CompositionGroup compositionGroup, TreeViewItemBase parent)
        {
            var compositionGroupTreeViewItem = new CompositionGroupTreeViewItem();

            compositionGroupTreeViewItem.CompositionGroup = compositionGroup;
            compositionGroupTreeViewItem.Parent = parent;
            compositionGroupTreeViewItem.Header = GetHeader(compositionGroup);
            compositionGroupTreeViewItem.IsExpanded = compositionGroup.Compositions.Count > 0 ? new Nullable<bool>(false) : null;
            compositionGroupTreeViewItem.CanExpand = compositionGroup.Compositions.Count > 0 ? true : false;
            compositionGroupTreeViewItem.Command = GetCommand(compositionGroup);
            compositionGroupTreeViewItem.StarVisibility = GetStarVisibility(compositionGroup);

            return compositionGroupTreeViewItem;
        }

        private Visibility GetStarVisibility(CompositionGroup compositionGroup)
        {
            var popularProperty = compositionGroup.CompositionGroupProperties.FirstOrDefault(cgp => cgp.Key == "Popular");

            if (popularProperty != null && popularProperty.Value == "Yes")
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        private ICommand GetCommand(CompositionGroup compositionGroup)
        {
            Action<object> play = o =>
            {
                var fileName = Path.GetTempFileName() + Guid.NewGuid().ToString() + ".m3u8";

                var audioPaths = compositionGroup.Compositions.SelectMany(c =>
                {
                    var movementsWithAudio = c.Movements.Where(m =>
                    {
                        var audioProperty = m.MovementProperties.FirstOrDefault(mp => mp.Key == "Audio");

                        if (audioProperty == null || string.IsNullOrWhiteSpace(audioProperty.Value))
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    });

                    if (movementsWithAudio.Count() > 0)
                    {
                        var orderedMovements = movementsWithAudio.OrderBy(m =>
                        {
                            var movementNumberProperty = m.MovementProperties.FirstOrDefault(mp => mp.Key == "Number");

                            if (movementNumberProperty != null && !string.IsNullOrWhiteSpace(movementNumberProperty.Value))
                            {
                                return movementNumberProperty.Value;
                            }
                            else
                            {
                                return string.Empty;
                            }
                        }, new NaturalComparer());

                        return orderedMovements.Select(m =>
                        {
                            var audioProperty = m.MovementProperties.FirstOrDefault(mp => mp.Key == "Audio");

                            return audioProperty.Value;
                        });
                    }
                    else
                    {
                        var compositionAudioProperty = c.CompositionProperties.FirstOrDefault(cp => cp.Key == "Audio");

                        if (compositionAudioProperty != null && !string.IsNullOrWhiteSpace(compositionAudioProperty.Value))
                        {
                            return new string[] { compositionAudioProperty.Value };
                        }
                    }

                    return null;
                });

                File.WriteAllLines(fileName, audioPaths);

                Process.Start(fileName);
            };

            Predicate<object> canPlay = o =>
            {
                return compositionGroup.Compositions.Any(c =>
                {
                    var movementsWithAudio = c.Movements.Where(m =>
                    {
                        var audioProperty = m.MovementProperties.FirstOrDefault(mp => mp.Key == "Audio");

                        if (audioProperty == null || string.IsNullOrWhiteSpace(audioProperty.Value))
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    });

                    if (movementsWithAudio.Count() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        var compositionAudioProperty = c.CompositionProperties.FirstOrDefault(cp => cp.Key == "Audio");

                        if (compositionAudioProperty != null && !string.IsNullOrWhiteSpace(compositionAudioProperty.Value))
                        {
                            return true;
                        }
                    }

                    return false;
                });
            };

            return new DelegateCommand(play, canPlay);
        }

        private string GetHeader(CompositionGroup compositionGroup)
        {
            var nameProperty = compositionGroup.CompositionGroupProperties.FirstOrDefault(cgp => cgp.Key == "Name");

            return nameProperty.Value;
        }
    }
}