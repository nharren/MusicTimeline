using NathanHarrenstein.ComposerTimeline.Controls;
using NathanHarrenstein.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NathanHarrenstein.ComposerTimeline.UI.Data
{
    public class PlaylistItemData
    {
        public DelegateCommand MoveUpCommand { get; set; }
        public DelegateCommand MoveDownCommand { get; set; }
        public string Name { get; set; }
        public PlaylistData Playlist { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand PlayCommand { get; set; }
        public PlaylistData Owner { get; set; }
        public string Path { get; set; }
    }
}
