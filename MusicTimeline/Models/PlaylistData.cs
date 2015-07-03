using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NathanHarrenstein.ComposerTimeline.UI.Data
{
    public class PlaylistData
    {
        public ICommand PlayAllCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ObservableCollection<PlaylistItemData> PlaylistItems { get; set; }
        public string Name { get; set; }
        public ListItemData Owner { get; set; }
    }
}
