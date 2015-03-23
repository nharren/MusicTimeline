using NathanHarrenstein.ComposerDatabase;
using NathanHarrenstein.ComposerTimeline.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NathanHarrenstein.ComposerTimeline.UI.Data
{
    public class ListItemData
    {
        public bool HavePlaylists { get; set; }
        public ObservableCollection<PlaylistData> Playlists { get; set; }
    }
}
