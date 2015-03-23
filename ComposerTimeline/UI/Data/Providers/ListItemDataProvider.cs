using NathanHarrenstein.ComposerDatabase;
using NathanHarrenstein.ComposerTimeline.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using System.Windows;
using System.Collections.ObjectModel;
using NathanHarrenstein.ComposerTimeline.UI.Data.Comparers;
using System.Diagnostics;

namespace NathanHarrenstein.ComposerTimeline.UI.Data.Providers
{
    public class ListItemDataProvider
    {
        public ListItemData GetListItemData(ListItemData owner)
        {
            var listItemData = new ListItemData();

            listItemData.Owner = owner;
            listItemData.ExpandCommand = GetExpandCommand(listItemData);
            listItemData.CollapseCommand = GetCollapseCommand(listItemData);
            listItemData.Playlists = GetPlaylists(listItemData);
            listItemData.HavePlaylists = GetHavePlaylists(listItemData);
            // listItemData.PlayCommand = Set by the provider of a class which inherits ListItemData.
            // listItemData.AddToPlaylistCommand = Set by the provider of a class which inherits ListItemData.
            // listItemData.StarVisibility = Set by the provider of a class which inherits ListItemData.
            // listItemData.IsExpanded = Set by ExpandCommand and CollapseCommand.
            // listItemData.ListItems = Set by the provider of a class which inherits ListItemData.

            return listItemData;
        }

        public bool GetHavePlaylists(ListItemData listItemData)
        {
            return listItemData.Playlists.Count > 0;
        }

        //private DelegateCommand GetAddToPlaylistCommand()
        //{
        //    Action<object> addToPlaylist = o =>
        //    {
        //        var destinationDirectory = Path.Combine(Environment.CurrentDirectory, "Files", "Playlists");

        //        if (obj == null)
        //        {
        //            if (!Directory.Exists(destinationDirectory))
        //            {
        //                Directory.CreateDirectory(destinationDirectory);
        //            }

        //            var saveFileDialog = new SaveFileDialog();
        //            saveFileDialog.Filter = "UTF-8 Audio Playlist (*.m3u8)|*.m3u8|All files (*.*)|*.*";
        //            saveFileDialog.InitialDirectory = destinationDirectory;
        //            saveFileDialog.FileName = "New Playlist.m3u8";

        //            if (saveFileDialog.ShowDialog() == true)
        //            {
        //                var playlistPath = saveFileDialog.FileName;

        //                using (var file = new StreamWriter(playlistPath, false))
        //                {
        //                    foreach (var audioPath in GetAudioPaths())
        //                    {
        //                        file.WriteLine(audioPath);
        //                    }
        //                }

        //                var playlistName = Path.GetFileNameWithoutExtension(playlistPath);
        //                var playlistDataProvider = new PlaylistDataProvider();

        //                var playlist = playlistDataProvider.GetPlaylistData(playlistName, ListItemData);

        //                ListItemData.Playlists.Add(playlist);

        //                if (!ListItemData.HavePlaylists)
        //                {
        //                    ListItemData.HavePlaylists = true;

        //                    ListItemData.OnPropertyChanged("HavePlaylists");
        //                }
        //            }
        //            else
        //            {
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            var playlistName = (string)obj;
        //            var playlistPath = Path.Combine(destinationDirectory, playlistName + ".m3u8");

        //            if (Directory.Exists(destinationDirectory) && File.Exists(playlistPath))
        //            {
        //                using (var file = new StreamWriter(playlistPath, true))
        //                {
        //                    foreach (var audioPath in GetAudioPaths())
        //                    {
        //                        file.WriteLine(audioPath);
        //                    }
        //                }
        //            }
        //        }
        //    };
        //    return new DelegateCommand(AddToPlaylist);
        //}

        private ObservableCollection<PlaylistData> GetPlaylists(ListItemData listItemData)
        {
            var destinationDirectory = Path.Combine(Environment.CurrentDirectory, "Files", "Playlists");
            var directoryHasFiles = Directory.EnumerateFiles(destinationDirectory).Count() > 0;

            if (Directory.Exists(destinationDirectory) && directoryHasFiles)
            {
                var playlistPaths = Directory.EnumerateFiles(destinationDirectory);

                var playlists = playlistPaths.Select(pp => 
                {
                    var playlistDataProvider = new PlaylistDataProvider();
                    var playlistName = Path.GetFileNameWithoutExtension(pp);

                    return playlistDataProvider.GetPlaylistData(playlistName, listItemData);
                });

                return new ObservableCollection<PlaylistData>(playlists);
            }

            return new ObservableCollection<PlaylistData>();
        }

        //protected override IEnumerable<string> GetAudioPaths()
        //{
        //    return compositionGroup.Compositions.SelectMany(composition =>
        //    {
        //        if (composition.Movements.Any(movement => !string.IsNullOrWhiteSpace(movement["Audio"])))
        //        {
        //            return composition.Movements.Where(movement => !string.IsNullOrWhiteSpace(movement["Audio"]))
        //                                        .OrderBy(movement => movement["Number"], new NaturalComparer())
        //                                        .Select(movement => movement["Audio"]);
        //        }
        //        else if (!string.IsNullOrWhiteSpace(composition["Audio"]))
        //        {
        //            return new string[] { composition["Audio"] };
        //        }
        //        else
        //        {
        //            return base.GetAudioPaths();
        //        }
        //    })
        //    .Where(audio => !string.IsNullOrWhiteSpace(audio));
        //}

        //protected override void LoadChildren()
        //{
        //    foreach (var item in CompositionGroup.Compositions)
        //    {
        //        compositionViewModels.Add(new CompositionViewModel(item, this));
        //    }
        //}

        //private void Play(object obj)
        //{
        //    var fileName = Path.GetTempFileName() + Guid.NewGuid().ToString() + ".m3u8";

        //    File.WriteAllLines(fileName, GetAudioPaths());

        //    Process.Start(fileName);
        //}

        //private Visibility GetStarVisibility(ListItemData)
        //{
        //    ListItemData

        //    return String.Equals(properties["Popular"].Value, "Yes") ? Visibility.Visible : Visibility.Collapsed;
        //}
    }
}
