using NathanHarrenstein.Controls;
using NathanHarrenstein.Input;
using System.Windows;

namespace NathanHarrenstein.ComposerTimeline
{
    public class TreeViewItemBase : TreeViewItem
    {
        public static readonly DependencyProperty StarVisibilityProperty = DependencyProperty.Register("StarVisibility", typeof(Visibility), typeof(TreeViewItemBase));

        static TreeViewItemBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeViewItemBase), new FrameworkPropertyMetadata(typeof(TreeViewItemBase)));
        }

        public DelegateCommand AddToPlaylistCommand { get; set; }

        public bool HavePlaylists { get; set; }

        //public ObservableCollection<PlaylistData> Playlists { get; set; }

        public DelegateCommand PlayCommand { get; set; }

        public Visibility StarVisibility
        {
            get { return (Visibility)GetValue(StarVisibilityProperty); }
            set { SetValue(StarVisibilityProperty, value); }
        }
    }
}