using NathanHarrenstein.ComposerTimeline.UI.Initializers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace NathanHarrenstein.ComposerTimeline
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

        public IEnumerable<Uri> ComposerImages { get; set; }

        public string Died { get; set; }

        public IEnumerable<Flag> Flags { get; set; }

        public Visibility HasInfluenced { get; set; }

        public Visibility HasInfluences { get; set; }

        public string HeaderText { get; set; }

        public IEnumerable<InfluenceData> Influenced { get; set; }

        public IEnumerable<InfluenceData> Influences { get; set; }

        public Visibility LinksVisibility { get; set; }

        public IEnumerable<LinkData> Links { get; set; }

        public string Nationality { get; set; }

        public IEnumerable<NathanHarrenstein.Controls.TreeViewItem> TreeViewItems { get; set; }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();

            e.Handled = true;
        }
    }
}