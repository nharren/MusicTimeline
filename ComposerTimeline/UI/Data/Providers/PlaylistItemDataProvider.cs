using NathanHarrenstein.ComposerDatabase;
using NathanHarrenstein.ComposerTimeline.Controls;
using NathanHarrenstein.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NathanHarrenstein.ComposerTimeline.UI.Data.Providers
{
    public class PlaylistItemDataProvider
    {
        public PlaylistItemData GetPlaylistItemData(string path, PlaylistData owner)
        {
            var playlistItemData = new PlaylistItemData();

            playlistItemData.Owner = owner;
            playlistItemData.DeleteCommand = GetDeleteCommand(playlistItemData);
            playlistItemData.MoveDownCommand = GetMoveDownCommand(playlistItemData);
            playlistItemData.MoveUpCommand = GetMoveUpCommand(playlistItemData);
            playlistItemData.Name = GetName(path);
            playlistItemData.Path = path;

            return playlistItemData;
        }

        private string GetName(string path)
        {
            var dataProvider = new DataProvider();

            var compositionPropertes = dataProvider.CompositionProperties;
            var movementProperties = dataProvider.MovementProperties;

            var compositionAudioProperty = compositionPropertes.FirstOrDefault(cp => cp.Value == path);
            var movementAudioProperty = movementProperties.FirstOrDefault(mp => mp.Value == path);

            if (compositionAudioProperty != null)
            {
                var composition = compositionAudioProperty.Composition;
                var compositionNameProperty = composition.CompositionProperties.FirstOrDefault(cp => cp.Key == "Name");
                var compositionName = compositionNameProperty.Value;

                var composer = composition.Composer;
                var composerNameProperty = composer.ComposerProperties.FirstOrDefault(cp => cp.Key == "Name");
                var composerName = composerNameProperty.Value;

                return composerName + " — " + compositionName;
            }
            else if(movementAudioProperty != null)
            {
                var movement = movementAudioProperty.Movement;
                var movementNameProperty = movement.MovementProperties.FirstOrDefault(mp => mp.Key == "Name");
                var movementName = movementNameProperty.Value;

                var movementNumberProperty = movement.MovementProperties.FirstOrDefault(mp => mp.Key == "Number");
                var movementNumber = movementNumberProperty.Value;

                var composition = movement.Composition;
                var compositionNameProperty = composition.CompositionProperties.FirstOrDefault(cp => cp.Key == "Name");
                var compositionName = compositionNameProperty.Value;

                var composer = composition.Composer;
                var composerNameProperty = composer.ComposerProperties.FirstOrDefault(cp => cp.Key == "Name");
                var composerName = composerNameProperty.Value;

                return composerName + " — " + compositionName + ": " + movementNumberProperty + ". " + movementName;
            }

            throw new NotImplementedException("Only Compositions and Movements are currently supported as Playlist Items.");

        }

        private DelegateCommand GetMoveUpCommand(PlaylistItemData playlistItemData)
        {
            Action<object> moveUp = o =>
            {
                var playlistItemIndex = playlistItemData.Owner.PlaylistItems.IndexOf(playlistItemData);
                var playlistPath = Environment.CurrentDirectory + @"\Files\Playlists\" + playlistItemData.Owner.Name + ".m3u8";
                var playlist = File.ReadAllLines(playlistPath, Encoding.UTF8).ToList();

                playlist.RemoveAt(playlistItemIndex);
                playlist.Insert(playlistItemIndex - 1, playlistPath);

                File.WriteAllLines(playlistPath, playlist);

                playlistItemData.Owner.PlaylistItems.Move(playlistItemIndex, playlistItemIndex - 1);

                playlistItemData.MoveDownCommand.RaiseCanExecuteChanged();
                playlistItemData.MoveUpCommand.RaiseCanExecuteChanged();
            };

            Predicate<object> canMoveUp = o =>
            {
                var playlist = playlistItemData.Owner;

                return playlist.PlaylistItems.IndexOf(playlistItemData) > 0;
            };

            return new DelegateCommand(moveUp, canMoveUp);
        }

        private DelegateCommand GetMoveDownCommand(PlaylistItemData playlistItemData)
        {
            Action<object> moveDown = o =>
            {
                var playlistItemIndex = playlistItemData.Owner.PlaylistItems.IndexOf(playlistItemData);
                var playlistPath = Environment.CurrentDirectory + @"\Files\Playlists\" + playlistItemData.Owner.Name + ".m3u8";
                var playlist = File.ReadAllLines(playlistPath, Encoding.UTF8).ToList();

                playlist.RemoveAt(playlistItemIndex);
                playlist.Insert(playlistItemIndex + 1, playlistPath);

                File.WriteAllLines(playlistPath, playlist);

                playlistItemData.Owner.PlaylistItems.Move(playlistItemIndex, playlistItemIndex + 1);

                playlistItemData.MoveDownCommand.RaiseCanExecuteChanged();
                playlistItemData.MoveUpCommand.RaiseCanExecuteChanged();
            };

            Predicate<object> canMoveDown = o =>
            {
                var playlist = playlistItemData.Owner;

                return playlist.PlaylistItems.IndexOf(playlistItemData) < playlist.PlaylistItems.Count - 1;
            };

            return new DelegateCommand(moveDown, canMoveDown);
        }

        private DelegateCommand GetDeleteCommand(PlaylistItemData playlistItemData)
        {
            Action<object> delete = o =>
            {
                var playlistItemIndex = playlistItemData.Owner.PlaylistItems.IndexOf(playlistItemData);
                var playlistPath = Environment.CurrentDirectory + @"\Files\Playlists\" + playlistItemData.Owner.Name + ".m3u8";
                var playlist = File.ReadAllLines(playlistPath, Encoding.UTF8).ToList();

                playlist.RemoveAt(playlistItemIndex);

                File.WriteAllLines(playlistPath, playlist);

                playlistItemData.Owner.PlaylistItems.Remove(playlistItemData);

                playlistItemData.MoveDownCommand.RaiseCanExecuteChanged();
                playlistItemData.MoveUpCommand.RaiseCanExecuteChanged();
            };

            return new DelegateCommand(delete);
        }
    }
}
