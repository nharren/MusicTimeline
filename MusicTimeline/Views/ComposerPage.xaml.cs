using NathanHarrenstein.MusicTimeline.Initializers;
using NathanHarrenstein.MusicTimeline.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class ComposerPage : Page
    {
        public ComposerPage()
        {
            InitializeComponent();

            ComposerPageInitializer.Initialize(this);
        }

        public string Biography { get; set; }

        public string Born { get; set; }

        public IEnumerable<BitmapImage> ComposerImages { get; set; }

        public string Died { get; set; }

        public IEnumerable<Flag> Flags { get; set; }

        public Visibility HasInfluenced { get; set; }

        public Visibility HasInfluences { get; set; }

        public string HeaderText { get; set; }

        public IEnumerable<Influence> Influenced { get; set; }

        public IEnumerable<Influence> Influences { get; set; }

        public Visibility LinksVisibility { get; set; }

        public IEnumerable<Link> Links { get; set; }

        public string Nationality { get; set; }

        public IEnumerable<Controls.TreeViewItem> TreeViewItems { get; set; }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();

            e.Handled = true;
        }
    }
}