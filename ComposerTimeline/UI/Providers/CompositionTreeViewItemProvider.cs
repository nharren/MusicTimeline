﻿using NathanHarrenstein.Comparers;
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
    public class CompositionTreeViewItemProvider
    {
        public CompositionTreeViewItem GetCompositionTreeViewItem(Composition composition, TreeViewItemBase parent)
        {
            var compositionTreeViewItem = new CompositionTreeViewItem();

            compositionTreeViewItem.Composition = composition;
            compositionTreeViewItem.Parent = parent;
            compositionTreeViewItem.Header = GetHeader(composition);
            compositionTreeViewItem.IsExpanded = composition.Movements.Count > 0 ? new Nullable<bool>(false) : null;
            compositionTreeViewItem.CanExpand = composition.Movements.Count > 0 ? true : false;
            compositionTreeViewItem.Command = GetCommand(composition);
            compositionTreeViewItem.StarVisibility = GetStarVisibility(composition);

            return compositionTreeViewItem;
        }

        private Visibility GetStarVisibility(Composition composition)
        {
            var popularProperty = composition.CompositionProperties.FirstOrDefault(cp => cp.Key == "Popular");

            if (popularProperty != null && popularProperty.Value == "Yes")
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        private ICommand GetCommand(Composition composition)
        {
            Action<object> play = o =>
            {
                var fileName = Path.GetTempFileName() + Guid.NewGuid().ToString() + ".m3u8";

                var audioPaths = Enumerable.Empty<string>();

                var movementsWithAudio = composition.Movements.Where(m =>
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

                        if (movementNumberProperty != null || !string.IsNullOrWhiteSpace(movementNumberProperty.Value))
                        {
                            return movementNumberProperty.Value;
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }, new NaturalComparer());

                    audioPaths = orderedMovements.Select(m =>
                    {
                        var audioProperty = m.MovementProperties.FirstOrDefault(mp => mp.Key == "Audio");

                        return audioProperty.Value;
                    });
                }
                else
                {
                    var compositionAudioProperty = composition.CompositionProperties.FirstOrDefault(cp => cp.Key == "Audio");

                    if (compositionAudioProperty != null && !string.IsNullOrWhiteSpace(compositionAudioProperty.Value))
                    {
                        audioPaths = new string[] { compositionAudioProperty.Value };
                    }
                }

                File.WriteAllLines(fileName, audioPaths);

                Process.Start(fileName);
            };

            Predicate<object> canPlay = o =>
            {
                var movementsWithAudio = composition.Movements.Where(m =>
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
                    var compositionAudioProperty = composition.CompositionProperties.FirstOrDefault(cp => cp.Key == "Audio");

                    if (compositionAudioProperty != null && !string.IsNullOrWhiteSpace(compositionAudioProperty.Value))
                    {
                        return true;
                    }
                }

                return false;
            };

            return new DelegateCommand(play, canPlay);
        }

        private string GetHeader(Composition composition)
        {
            var nameProperty = composition.CompositionProperties.FirstOrDefault(cp => cp.Key == "Name");

            return nameProperty.Value;
        }
    }
}