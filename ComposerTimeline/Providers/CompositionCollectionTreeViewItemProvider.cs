using Database;
using System.Windows;

namespace NathanHarrenstein.ComposerTimeline.Providers
{
    public static class CompositionCollectionTreeViewItemProvider
    {
        public static CompositionCollectionTreeViewItem GetCompositionCollectionTreeViewItem(CompositionCollection compositionCollection, TreeViewItemBase parent)
        {
            var compositionCollectionTreeViewItem = new CompositionCollectionTreeViewItem();

            compositionCollectionTreeViewItem.CompositionCollection = compositionCollection;
            compositionCollectionTreeViewItem.Parent = parent;
            compositionCollectionTreeViewItem.Header = compositionCollection.Name;
            compositionCollectionTreeViewItem.IsExpanded = compositionCollection.Compositions.Count > 0 ? new bool?(false) : null;
            compositionCollectionTreeViewItem.CanExpand = compositionCollection.Compositions.Count > 0 ? true : false;
            // compositionCollectionTreeViewItem.Command = GetCommand(compositionCollection);                                    REQUIREMENTS: Set Audio Directory; Retrieve ID from track.
            compositionCollectionTreeViewItem.StarVisibility = compositionCollection.IsPopular ? Visibility.Visible : Visibility.Collapsed;

            return compositionCollectionTreeViewItem;
        }

        /*
        private static ICommand GetCommand(CompositionCollection compositionCollection)
        {
            Action<object> play = o =>
            {
                var fileName = Path.GetTempFileName() + Guid.NewGuid().ToString() + ".m3u8";

                var audioPaths = compositionCollection.Compositions.SelectMany(c =>
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
                return compositionCollection.Compositions.Any(c =>
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
        */
    }
}