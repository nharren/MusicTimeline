using NathanHarrenstein.ComposerTimeline.Controls;
using NathanHarrenstein.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NathanHarrenstein.ComposerTimeline.UI.Data.Providers
{
    public class PlaylistDataProvider
    {
        public PlaylistData GetPlaylistData(string name, ListItemData owner)
        {
            var playlistData = new PlaylistData();

            playlistData.Name = name;
            playlistData.Owner = owner;
            playlistData.PlayAllCommand = GetPlayAllCommand(playlistData);
            playlistData.DeleteCommand = GetDeleteCommand(playlistData);
            playlistData.PlaylistItems = GetPlaylistItems(playlistData);

            return playlistData;
        }

        private ObservableCollection<PlaylistItemData> GetPlaylistItems(PlaylistData playlistData)
        {
            var playlistPath = Environment.CurrentDirectory + @"\Files\Playlists\" + playlistData.Name + ".m3u8";
            var playlistItems = new ObservableCollection<PlaylistItemData>();

            foreach (var playlistItemPath in File.ReadAllLines(playlistPath))
	        {
                var playlistItemDataProvider = new PlaylistItemDataProvider();
                var playlistItem = playlistItemDataProvider.GetPlaylistItemData(playlistItemPath, playlistData);

                playlistItems.Add(playlistItem);
	        }

            return playlistItems;
        }

        private ICommand GetDeleteCommand(PlaylistData playlistData)
        {
            Action<object> delete = o =>
            {
                var playlistPath = Environment.CurrentDirectory + @"\Files\Playlists\" + playlistData.Name + ".m3u8";

                File.Delete(playlistPath);

                playlistData.Owner.Playlists.Remove(playlistData);
            };

            return new DelegateCommand(delete);
        }

        private ICommand GetPlayAllCommand(PlaylistData playlistData)
        {
            Action<object> playAll = o =>
            {
                var playlistPath = Environment.CurrentDirectory + @"\Files\Playlists\" + playlistData.Name + ".m3u8";

                Process.Start(playlistPath);
            };

            return new DelegateCommand(playAll);
        }
    }
}
