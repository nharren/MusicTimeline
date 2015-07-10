﻿using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.Controls;
using NathanHarrenstein.MusicTimeline.Providers;
using NathanHarrenstein.MusicTimeline.Utilities;
using System.EDTF;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class ComposerPage : Page
    {
        public ComposerPage()
        {
            InitializeComponent();

            var composer = Application.Current.Properties["SelectedComposer"] as Composer;

            if (composer != null)
            {
                LoadComposer(composer);
            }
        }

        private void LoadComposer(Composer composer)
        {
            BiographyTextBlock.Text = composer.Biography;
            BornTextBlock.Text = GetBorn(composer);
            ComposerImagesListBox.ItemsSource = composer.ComposerImages;
            ComposerNameTextBlock.Text = NameUtility.ToFirstLast(composer.Name);
            DiedTextBlock.Text = GetDied(composer);
            ComposerFlagsItemsControl.ItemsSource = composer.Nationalities;
            InfluencedItemsControl.ItemsSource = composer.Influenced;
            InfluencedItemsControl.Visibility = InfluencedTextBlock.Visibility = composer.Influenced.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            InfluencesItemsControl.ItemsSource = composer.Influences;
            InfluencesItemsControl.Visibility = InfluencesTextBlock.Visibility = composer.Influences.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            LinksItemControl.ItemsSource = composer.ComposerLinks;
            LinksItemControl.Visibility = LinksTextBlock.Visibility = composer.ComposerLinks.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            TreeView.Children = composer.CompositionCollections
                .Select<CompositionCollection, Controls.TreeViewItem>(cc => CompositionCollectionTreeViewItemProvider.GetCompositionCollectionTreeViewItem(cc, null))
                .Concat(composer.Compositions
                    .Select(c => CompositionTreeViewItemProvider.GetCompositionTreeViewItem(c, null)))
                .OrderBy(tvi => tvi.Header);
            ComposerImagesListBox.SelectedIndex = 0;
        }

        private static string GetBorn(Composer composer)
        {
            if (composer.BirthLocation != null)
            {
                return $"{ExtendedDateTimeInterval.Parse(composer.Dates).Start}; {composer.BirthLocation.Name}";
            }

            return ExtendedDateTimeInterval.Parse(composer.Dates).Start.ToString();
        }

        private static string GetDied(Composer composer)
        {
            if (composer.DeathLocation != null)
            {
                return $"{ExtendedDateTimeInterval.Parse(composer.Dates).End}; {composer.DeathLocation.Name}";
            }

            return ExtendedDateTimeInterval.Parse(composer.Dates).End.ToString();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();

            e.Handled = true;
        }

        private void ComposerButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var composer = (Composer)button.DataContext;

            LoadComposer(composer);
        }
    }
}