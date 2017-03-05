﻿using Microsoft.Win32;
using NathanHarrenstein.MusicTimeline.Audio;
using NathanHarrenstein.MusicTimeline.Converters;
using NathanHarrenstein.MusicTimeline.Data;
using NathanHarrenstein.MusicTimeline.Parsers;
using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics;
using System.EDTF;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class ComposerPage : Page, IDisposable
    {
        private Dictionary<ComposerImage, List<string>> addedImages = new Dictionary<ComposerImage, List<string>>();
        private Composer composer;
        private bool isDisposed;
        private Button nationalitiesEditButton;
        private Button samplesEditButton;

        public ComposerPage()
        {
            InitializeComponent();

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.Navigating += MainWindow_Navigating;
            Loaded += ComposerPage_Loaded;
        }

        ~ComposerPage()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            mp3PlayerControl.Dispose();

            isDisposed = true;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();

            e.Handled = true;
        }

        private void biographyHeader_ButtonClick(object sender, RoutedEventArgs e)
        {
            biographyScrollViewer.Visibility = Visibility.Collapsed;
            biographyEditPanel.Visibility = Visibility.Visible;

            biographyRichTextBox.Document = BiographyUtility.LoadDocument(composer.Biography.Text);
        }

        private void biographyTextBox_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }

        private void biographyToolbar_Cancelling(object sender, EventArgs e)
        {
            biographyRichTextBox.Document.Blocks.Clear();

            biographyEditPanel.Visibility = Visibility.Collapsed;
            biographyScrollViewer.Visibility = Visibility.Visible;

            biographyScrollViewer.Document = BiographyUtility.LoadDocument(composer.Biography.Text);

            biographyHeader.IsEnabled = true;
        }

        private void biographyToolbar_Saving(object sender, EventArgs e)
        {
            composer.Biography.Text = XamlWriter.Save(biographyRichTextBox.Document);

            App.ClassicalMusicContext.UpdateObject(composer);

            try
            {
                App.ClassicalMusicContext.SaveChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Changes could not be saved.");
            }

            biographyRichTextBox.Document.Blocks.Clear();

            biographyEditPanel.Visibility = Visibility.Collapsed;
            biographyScrollViewer.Visibility = Visibility.Visible;

            biographyScrollViewer.Document = BiographyUtility.LoadDocument(composer.Biography.Text);

            biographyHeader.IsEnabled = true;
        }

        private void bornHeader_ButtonClick(object sender, RoutedEventArgs e)
        {
            bornEditPanel.Visibility = Visibility.Visible;
            bornTextBlock.Visibility = Visibility.Collapsed;

            var dates = ExtendedDateTimeInterval.Parse(composer.Dates);
            var birthDate = dates.Start;

            birthDateTextBox.Text = birthDate.ToString();
            birthLocationTextBox.Text = composer.BirthLocation?.Name;
        }

        private void bornToolbar_Cancelling(object sender, EventArgs e)
        {
            bornTextBlock.Visibility = Visibility.Visible;
            bornEditPanel.Visibility = Visibility.Collapsed;

            bornHeader.IsEnabled = true;
        }

        private void bornToolbar_Saving(object sender, EventArgs e)
        {
            var dates = ExtendedDateTimeInterval.Parse(composer.Dates);
            var birthDate = ExtendedDateTime.Parse(birthDateTextBox.Text);

            if (birthDate == null)
            {
                MessageBox.Show("The entered date is invalid.");

                return;
            }

            dates.Start = birthDate;

            composer.Dates = dates.ToString();

            LocationUtility.UpdateBirthLocation(birthLocationTextBox.Text, composer, App.ClassicalMusicContext);

            App.ClassicalMusicContext.UpdateObject(composer);
            App.ClassicalMusicContext.SaveChanges();

            bornTextBlock.Text = CreateBornText();
            bornTextBlock.Visibility = Visibility.Visible;
            bornEditPanel.Visibility = Visibility.Collapsed;
            bornHeader.IsEnabled = true;
        }

        private void ComposerPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadComposer();
        }

        private void compositionsHeader_ButtonClick(object sender, RoutedEventArgs e)
        {
            compositionsPanel.Visibility = Visibility.Collapsed;
            compositionsEditPanel.Visibility = Visibility.Visible;
        }




        private string CreateBornText()
        {
            if (composer.BirthLocation == null)
            {
                return ExtendedDateTimeInterval.Parse(composer.Dates).Start.ToString();
            }
            else
            {
                return $"{ExtendedDateTimeInterval.Parse(composer.Dates).Start}; {composer.BirthLocation.Name}";
            }
        }

        private string CreateDiedText()
        {
            if (composer.DeathLocation == null)
            {
                return ExtendedDateTimeInterval.Parse(composer.Dates).End.ToString();
            }
            else
            {
                return $"{ExtendedDateTimeInterval.Parse(composer.Dates).End}; {composer.DeathLocation.Name}";
            }
        }

        private void diedHeader_ButtonClick(object sender, RoutedEventArgs e)
        {
            diedEditPanel.Visibility = Visibility.Visible;
            diedTextBlock.Visibility = Visibility.Collapsed;

            var dates = ExtendedDateTimeInterval.Parse(composer.Dates);
            var deathDate = dates.End;

            deathDateTextBox.Text = deathDate.ToString();
            deathLocationTextBox.Text = composer.DeathLocation?.Name;
        }

        private void diedToolbar_Cancelling(object sender, EventArgs e)
        {
            diedTextBlock.Visibility = Visibility.Visible;
            diedEditPanel.Visibility = Visibility.Collapsed;

            diedHeader.IsEnabled = true;
        }

        private void diedToolbar_Saving(object sender, EventArgs e)
        {
            var dates = ExtendedDateTimeInterval.Parse(composer.Dates);
            var deathDate = ExtendedDateTime.Parse(deathDateTextBox.Text);

            if (deathDate == null)
            {
                MessageBox.Show("The entered date is invalid.");

                return;
            }

            dates.End = deathDate;

            composer.Dates = dates.ToString();

            LocationUtility.UpdateDeathLocation(birthLocationTextBox.Text, composer, App.ClassicalMusicContext);

            App.ClassicalMusicContext.UpdateObject(composer);
            App.ClassicalMusicContext.SaveChanges();

            diedTextBlock.Text = CreateDiedText();

            diedTextBlock.Visibility = Visibility.Visible;
            diedEditPanel.Visibility = Visibility.Collapsed;

            diedHeader.IsEnabled = true;
        }

        //private void image_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var image = (Image)sender;
        //    var composerImage = (ComposerImage)image.DataContext;
        //    var converter = new ComposerToBitmapImageConverter();

        //    image.Source = converter.Convert(composerImage.Composer, new ComposerToBitmapImageConverterSettings(composerImage.ComposerImageId, true));
        //}

        //private void imagesEditButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var imagesEditButton = (Button)sender;

        //    imagesEditButton.IsEnabled = false;

        //    imagesToolbar.Visibility = Visibility.Visible;
        //}

        //private void imagesListBox_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    var editButton = (Button)imagesListBox.Template.FindName("imagesEditButton", imagesListBox);

        //    if (App.HasCredential && editButton.IsEnabled)
        //    {
        //        editButton.Visibility = Visibility.Visible;
        //    }
        //}

        //private void imagesListBox_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    var editButton = (Button)imagesListBox.Template.FindName("imagesEditButton", imagesListBox);
        //    editButton.Visibility = Visibility.Collapsed;
        //}

        //private void imagesToolbar_Adding(object sender, EventArgs e)
        //{
        //    var composerImage = new ComposerImage();

        //    var openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.gif, *.png) | *.jpg; *.jpeg; *.gif; *.png";

        //    bool? result = openFileDialog.ShowDialog();

        //    if (result == true)
        //    {
        //        string filename = openFileDialog.FileName;

        //        List<string> images;

        //        if (addedImages.TryGetValue(composerImage, out images))
        //        {
        //            images.Add(filename);
        //        }
        //        else
        //        {
        //            addedImages.Add(composerImage, new List<string> { filename });
        //        }

        //        App.ClassicalMusicContext.AddToComposerImages(composerImage);
        //        composer.Images.Add(composerImage);

        //        imagesListBox.ItemsSource = composer.Images;
        //        imagesListBox.SelectedItem = composerImage;
        //    }
        //}

        //private void imagesToolbar_Cancelling(object sender, EventArgs e)
        //{
        //    addedImages.Clear();

        //    imagesToolbar.Visibility = Visibility.Collapsed;

        //    var composerImagesQuery = App.ClassicalMusicContext.LoadProperty(composer, nameof(composer.Images));
        //    var composerImages = composerImagesQuery.Cast<ComposerImage>().ToList();

        //    imagesListBox.ItemsSource = composerImages;
        //    imagesListBox.SelectedIndex = 0;

        //    var imagesEditButton = (Button)imagesListBox.Template.FindName("imagesEditButton", imagesListBox);
        //    imagesEditButton.IsEnabled = true;
        //}

        //private void imagesToolbar_Removing(object sender, EventArgs e)
        //{
        //    var image = (ComposerImage)imagesListBox.SelectedItem;

        //    composer.Images.Remove(image);

        //    App.ClassicalMusicContext.DeleteObject(image);

        //    imagesListBox.ItemsSource = composer.Images;
        //}

        //private void imagesToolbar_Saving(object sender, EventArgs e)
        //{
        //    foreach (var image in addedImages)
        //    {
        //        foreach (var path in image.Value)
        //        {
        //            App.ClassicalMusicContext.SetSaveStream(image.Key, File.OpenRead(path), true, null);
        //        }
        //    }

        //    App.ClassicalMusicContext.UpdateObject(composer);
        //    App.ClassicalMusicContext.SaveChanges();

        //    imagesToolbar.Visibility = Visibility.Collapsed;

        //    var imagesEditButton = (Button)imagesListBox.Template.FindName("imagesEditButton", imagesListBox);
        //    imagesEditButton.IsEnabled = true;
        //}

        private void influenceButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var composer = (Composer)button.DataContext;

            Application.Current.Properties["SelectedComposer"] = composer.ComposerId;

            var mainWindow = (NavigationWindow)Application.Current.MainWindow;
            mainWindow.NavigationService.Refresh();
        }

        private void influencedHeader_ButtonClick(object sender, RoutedEventArgs e)
        {
            influencedEditPanel.Visibility = Visibility.Visible;
            influencedItemsControl.Visibility = Visibility.Collapsed;

            influencedListBox.ItemsSource = App.ClassicalMusicContext.Composers.OrderBy(c => c.Name);

            foreach (var composer in composer.Influenced)
            {
                influencedListBox.SelectedItems.Add(composer);
            }

            influencedListBox.Focus();
        }

        private void influencedToolbar_Cancelling(object sender, EventArgs e)
        {
            influencedListBox.ItemsSource = null;

            influencedItemsControl.Visibility = Visibility.Visible;
            influencedEditPanel.Visibility = Visibility.Collapsed;

            influencedHeader.IsEnabled = true;
        }

        private void influencedToolbar_Saving(object sender, EventArgs e)
        {
            foreach (Composer item in influencedListBox.SelectedItems)
            {
                if (!composer.Influenced.Contains(item))
                {
                    composer.Influenced.Add(item);
                }
            }

            var influenced = composer.Influenced;

            foreach (var item in influenced)
            {
                if (!influencedListBox.SelectedItems.Contains(item))
                {
                    composer.Influenced.Remove(item);
                }
            }

            App.ClassicalMusicContext.UpdateObject(composer);
            App.ClassicalMusicContext.SaveChanges();

            influencedItemsControl.ItemsSource = composer.Influenced;
            influencedItemsControl.Visibility = Visibility.Visible;

            influencedListBox.ItemsSource = null;
            influencedEditPanel.Visibility = Visibility.Collapsed;

            influencedHeader.IsEnabled = true;
        }

        private void influencesHeader_ButtonClick(object sender, RoutedEventArgs e)
        {
            influencesEditPanel.Visibility = Visibility.Visible;
            influencesItemsControl.Visibility = Visibility.Collapsed;

            influencesListBox.ItemsSource = App.ClassicalMusicContext.Composers.OrderBy(c => c.Name);

            foreach (var composer in composer.Influences)
            {
                influencesListBox.SelectedItems.Add(composer);
            }

            influencesListBox.Focus();
        }

        private void influencesToolbar_Cancelling(object sender, EventArgs e)
        {
            influencesListBox.ItemsSource = null;

            influencesItemsControl.Visibility = Visibility.Visible;
            influencesEditPanel.Visibility = Visibility.Collapsed;

            influencesHeader.IsEnabled = true;
        }

        private void influencesToolbar_Saving(object sender, EventArgs e)
        {
            foreach (Composer item in influencesListBox.SelectedItems)
            {
                if (!composer.Influences.Contains(item))
                {
                    composer.Influences.Add(item);
                }
            }

            var influences = composer.Influences;

            foreach (var item in influences)
            {
                if (!influencesListBox.SelectedItems.Contains(item))
                {
                    composer.Influences.Remove(item);
                }
            }

            App.ClassicalMusicContext.UpdateObject(composer);
            App.ClassicalMusicContext.SaveChanges();

            influencesItemsControl.ItemsSource = composer.Influences;
            influencesItemsControl.Visibility = Visibility.Visible;

            influencesListBox.ItemsSource = null;
            influencesEditPanel.Visibility = Visibility.Collapsed;

            influencesHeader.IsEnabled = true;
        }

        private void linksHeader_ButtonClick(object sender, RoutedEventArgs e)
        {
            linksEditPanel.Visibility = Visibility.Visible;
            linksItemsControl.Visibility = Visibility.Collapsed;
            var youTubeParser = new YouTubeParser();
            var spotifyParser = new SpotifyParser();

            var filteredItems = new List<Link>();

            foreach (var link in composer.Links)
            {
                if (!youTubeParser.IsValidVideoUrl(link.Url) && !spotifyParser.IsValidTrackUrl(link.Url))
                {
                    filteredItems.Add(link);
                }
            }

            linksListBox.ItemsSource = filteredItems.ToList();

            linksListBox.Focus();
        }

        private void linksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            linksListBox.Items.Refresh();
        }

        private void linksToolbar_Adding(object sender, EventArgs e)
        {
            var link = new Link();
            link.Name = "New Link";

            App.ClassicalMusicContext.AddRelatedObject(composer, "Links", link);
            composer.Links.Add(link);

            var linkList = (List<Link>)linksListBox.ItemsSource;
            linkList.Add(link);

            linksListBox.Items.Refresh();
        }

        private void linksToolbar_Cancelling(object sender, EventArgs e)
        {
            linksListBox.ItemsSource = null;

            linksItemsControl.Visibility = Visibility.Visible;
            linksEditPanel.Visibility = Visibility.Collapsed;

            linksHeader.IsEnabled = true;
        }

        private void linksToolbar_Removing(object sender, EventArgs e)
        {
            var link = (Link)linksListBox.SelectedItem;

            App.ClassicalMusicContext.DeleteLink(composer, "Links", link);

            composer.Links.Remove(link);

            App.ClassicalMusicContext.DeleteObject(link);

            var linkList = (List<Link>)linksListBox.ItemsSource;
            linkList.Remove(link);

            linksListBox.Items.Refresh();
        }

        private void linksToolbar_Saving(object sender, EventArgs e)
        {
            foreach (var link in composer.Links)
            {
                if (string.IsNullOrWhiteSpace(link.Url))
                {
                    MessageBox.Show("Could not save because a link has an empty URL.");

                    return;
                }
            }

            App.ClassicalMusicContext.UpdateObject(composer);
            App.ClassicalMusicContext.SaveChanges();

            var youTubeParser = new YouTubeParser();
            var spotifyParser = new SpotifyParser();

            var filteredItems = new List<Link>();

            foreach (var link in composer.Links)
            {
                if (!youTubeParser.IsValidVideoUrl(link.Url) && !spotifyParser.IsValidTrackUrl(link.Url))
                {
                    filteredItems.Add(link);
                }
            }

            linksItemsControl.ItemsSource = filteredItems;
            linksItemsControl.Visibility = Visibility.Visible;

            linksListBox.ItemsSource = null;
            linksEditPanel.Visibility = Visibility.Collapsed;

            linksHeader.IsEnabled = true;
        }

        private void linkTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var link = (Link)textBox.DataContext;
            var url = link.Url;

            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }

            if (!url.StartsWith("http"))
            {
                url = "http://" + url;
            }

            link.Name = WebUtility.GetTitle(url);
        }

        private void LoadComposer()
        {
            if (ProgressBar.Visibility == Visibility.Collapsed)
            {
                ProgressBar.Visibility = Visibility.Visible;
            }

            var composerId = (int)Application.Current.Properties["SelectedComposer"];

            App.ClassicalMusicContext.MergeOption = MergeOption.OverwriteChanges;

            if (!App.ClassicalMusicContext.TryGetEntity(new Uri($"http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc/Composers({composerId})"), out this.composer))
            {
                this.composer = App.ClassicalMusicContext.Execute<Composer>(new Uri($"http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc/Composers({composerId})")).Single();
            }

            App.ClassicalMusicContext.LoadProperty(composer, nameof(composer.Compositions));
            App.ClassicalMusicContext.LoadProperty(composer, nameof(composer.Biography));
            App.ClassicalMusicContext.LoadProperty(composer, nameof(composer.Images));
            App.ClassicalMusicContext.LoadProperty(composer, nameof(composer.Influences));
            App.ClassicalMusicContext.LoadProperty(composer, nameof(composer.Influenced));
            App.ClassicalMusicContext.LoadProperty(composer, nameof(composer.Samples));
            App.ClassicalMusicContext.LoadProperty(composer, nameof(composer.Links));

            ComposerNameTextBlock.Text = NameUtility.ToFirstLast(composer.Name);

            bornTextBlock.Text = CreateBornText();
            diedTextBlock.Text = CreateDiedText();

            biographyScrollViewer.Document = BiographyUtility.LoadDocument(composer.Biography.Text);

            influencesItemsControl.ItemsSource = composer.Influences;

            var influencesVisibility = composer.Influences.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            influencesItemsControl.Visibility = influencesVisibility;
            influencesHeader.Visibility = influencesVisibility;

            influencedItemsControl.ItemsSource = composer.Influenced;

            var influencedVisibility = composer.Influenced.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            influencedItemsControl.Visibility = influencedVisibility;
            influencedHeader.Visibility = influencedVisibility;

            var youTubeParser = new YouTubeParser();
            var spotifyParser = new SpotifyParser();

            var filteredItems = new List<Link>();

            foreach (var link in composer.Links)
            {
                if (youTubeParser.IsValidVideoUrl(link.Url) || spotifyParser.IsValidTrackUrl(link.Url))
                {
                    filteredItems.Add(link);
                }
            }

            mediaItemsControl.ItemsSource = filteredItems;

            var mediaVisibility = filteredItems.Count() > 0 ? Visibility.Visible : Visibility.Collapsed;

            mediaItemsControl.Visibility = mediaVisibility;
            mediaHeader.Visibility = mediaVisibility;

            filteredItems = new List<Link>();

            foreach (var link in composer.Links)
            {
                if (!youTubeParser.IsValidVideoUrl(link.Url) && !spotifyParser.IsValidTrackUrl(link.Url))
                {
                    filteredItems.Add(link);
                }
            }

            linksItemsControl.ItemsSource = filteredItems;

            var linksVisibility = filteredItems.Count() > 0 ? Visibility.Visible : Visibility.Collapsed;

            linksItemsControl.Visibility = linksVisibility;
            linksHeader.Visibility = linksVisibility;

            samplesEditButton = (Button)mp3PlayerControl.Template.FindName("samplesEditButton", mp3PlayerControl);
            samplesEditButton.Click += samplesEditButton_Click;

            nationalitiesEditButton = (Button)nationalitiesPanel.Template.FindName("nationalitiesEditButton", nationalitiesPanel);
            nationalitiesEditButton.Click += nationalitiesEditButton_Click;

            foreach (var sample in composer.Samples)
            {
                var mp3PlaylistItem = new PlaylistItem();
                mp3PlaylistItem.StreamUri = App.ClassicalMusicContext.GetReadStreamUri(sample);
                mp3PlaylistItem.Metadata.Add("Title", sample.Title);
                mp3PlaylistItem.Metadata.Add("Artist", sample.Artists);

                mp3PlayerControl.Playlist.Add(mp3PlaylistItem);

                if (mp3PlayerControl.Playlist.Count == 1)
                {
                    mp3PlayerControl.Play();
                }
            }

            imagesPanel.Composer = composer;
            samplesEditPanel.Composer = composer;
            nationalitiesPanel.Composer = composer;
            nationalitiesEditPanel.Composer = composer;
            compositionsPanel.Compositions = composer.Compositions;
            compositionsEditPanel.Composer = composer;

            ProgressBar.Visibility = Visibility.Collapsed;
        }

        private void MainWindow_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            Dispose();

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.Navigating -= MainWindow_Navigating;
        }

        private void mediaHeader_ButtonClick(object sender, RoutedEventArgs e)
        {
            mediaEditPanel.Visibility = Visibility.Visible;
            mediaItemsControl.Visibility = Visibility.Collapsed;

            var youTubeParser = new YouTubeParser();
            var spotifyParser = new SpotifyParser();
            var filteredItems = new List<Link>();

            foreach (var link in composer.Links)
            {
                if (youTubeParser.IsValidVideoUrl(link.Url) || spotifyParser.IsValidTrackUrl(link.Url))
                {
                    filteredItems.Add(link);
                }
            }

            mediaListBox.ItemsSource = filteredItems.ToList();
            mediaListBox.Focus();
        }

        private void mediaListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mediaListBox.Items.Refresh();
        }

        private void mediaToolbar_Adding(object sender, EventArgs e)
        {
            var link = new Link();
            link.Name = "New Link";

            App.ClassicalMusicContext.AddRelatedObject(composer, "Links", link);
            composer.Links.Add(link);

            var linkList = (List<Link>)mediaListBox.ItemsSource;
            linkList.Add(link);

            mediaListBox.Items.Refresh();
        }

        private void mediaToolbar_Cancelling(object sender, EventArgs e)
        {
            mediaListBox.ItemsSource = null;

            mediaItemsControl.Visibility = Visibility.Visible;
            mediaEditPanel.Visibility = Visibility.Collapsed;

            mediaHeader.IsEnabled = true;
        }

        private void mediaToolbar_Removing(object sender, EventArgs e)
        {
            var link = (Link)mediaListBox.SelectedItem;

            App.ClassicalMusicContext.DeleteLink(composer, nameof(composer.Links), link);

            composer.Links.Remove(link);

            App.ClassicalMusicContext.DeleteObject(link);

            var linkList = (List<Link>)mediaListBox.ItemsSource;
            linkList.Remove(link);

            mediaListBox.Items.Refresh();
        }

        private void mediaToolbar_Saving(object sender, EventArgs e)
        {
            foreach (var link in composer.Links)
            {
                if (string.IsNullOrWhiteSpace(link.Url))
                {
                    MessageBox.Show("Could not save because a link has an empty URL.");

                    return;
                }
            }

            App.ClassicalMusicContext.UpdateObject(composer);
            App.ClassicalMusicContext.SaveChanges();

            var youTubeParser = new YouTubeParser();
            var spotifyParser = new SpotifyParser();

            var filteredItems = new List<Link>();

            foreach (var link in composer.Links)
            {
                if (youTubeParser.IsValidVideoUrl(link.Url) || spotifyParser.IsValidTrackUrl(link.Url))
                {
                    filteredItems.Add(link);
                }
            }

            mediaItemsControl.ItemsSource = filteredItems;
            mediaItemsControl.Visibility = Visibility.Visible;

            mediaListBox.ItemsSource = null;
            mediaEditPanel.Visibility = Visibility.Collapsed;

            mediaHeader.IsEnabled = true;
        }

        private void mp3PlayerControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (App.HasCredential && samplesEditButton.IsEnabled)
            {
                samplesEditButton.Visibility = Visibility.Visible;
            }
        }

        private void mp3PlayerControl_MouseLeave(object sender, MouseEventArgs e)
        {
            samplesEditButton.Visibility = Visibility.Hidden;
        }

        private void nationalitiesEditButton_Click(object sender, RoutedEventArgs e)
        {
            var nationalitiesEditButton = (Button)sender;
            nationalitiesEditButton.IsEnabled = false;

            nationalitiesPanel.Visibility = Visibility.Collapsed;
            nationalitiesEditPanel.Visibility = Visibility.Visible;
        }

        private void nationalitiesPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            if (App.HasCredential && nationalitiesEditButton.IsEnabled)
            {
                nationalitiesEditButton.Visibility = Visibility.Visible;
            }
        }

        private void nationalitiesPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            nationalitiesEditButton.Visibility = Visibility.Hidden;
        }

        private void RemoveComposition(Composition selectedComposition)
        {
            var compositions = composer.Compositions.ToArray();

            foreach (var composition in compositions)
            {
                if (composition == selectedComposition)
                {
                    composer.Compositions.Remove(composition);

                    return;
                }
            }

            return;
        }

        private void samplesEditButton_Click(object sender, RoutedEventArgs e)
        {
            var samplesEditButton = (Button)sender;

            samplesEditButton.IsEnabled = false;

            mp3PlayerControl.Stop();
            mp3PlayerControl.Visibility = Visibility.Collapsed;
            samplesEditPanel.Visibility = Visibility.Visible;
        }

        private void samplesEditPanel_Cancelling(object sender, EventArgs e)
        {
            mp3PlayerControl.Visibility = Visibility.Visible;
            samplesEditButton.IsEnabled = true;
        }

        private void samplesEditPanel_Saving(object sender, EventArgs e)
        {
            mp3PlayerControl.Playlist.Clear();

            App.ClassicalMusicContext.LoadProperty(composer, "Samples");

            foreach (var sample in composer.Samples)
            {
                var mp3PlaylistItem = new PlaylistItem();
                mp3PlaylistItem.StreamUri = App.ClassicalMusicContext.GetReadStreamUri(sample);
                mp3PlaylistItem.Metadata.Add("Title", sample.Title);
                mp3PlaylistItem.Metadata.Add("Artist", sample.Artists);

                mp3PlayerControl.Playlist.Add(mp3PlaylistItem);

                if (mp3PlayerControl.Playlist.Count == 1)
                {
                    mp3PlayerControl.Play();
                }
            }

            mp3PlayerControl.Visibility = Visibility.Visible;
            samplesEditButton.IsEnabled = true;
        }

        private void selectedImage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var image = (Image)sender;
            var composerImage = (ComposerImage)image.DataContext;
            var converter = new ComposerToBitmapImageConverter();

            image.Source = converter.Convert(composerImage.Composer, new ComposerToBitmapImageConverterSettings(composerImage.ComposerImageId, false));
        }

        private void youTubeButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var link = (Link)button.DataContext;

            Process.Start(link.Url);
        }

        private void nationalitiesEditPanel_Cancelling(object sender, EventArgs e)
        {
            nationalitiesPanel.Visibility = Visibility.Visible;
            nationalitiesEditButton.IsEnabled = true;
        }

        private void nationalitiesEditPanel_Saving(object sender, EventArgs e)
        {
            nationalitiesPanel.UpdateData();
            nationalitiesPanel.Visibility = Visibility.Visible;
            nationalitiesEditButton.IsEnabled = true;
        }

        private void compositionsEditPanel_Cancelling(object sender, EventArgs e)
        {
            compositionsPanel.Visibility = Visibility.Visible;
            compositionsHeader.IsEnabled = true;
        }

        private void compositionsEditPanel_Saving(object sender, EventArgs e)
        {
            compositionsPanel.UpdateData();
            compositionsPanel.Visibility = Visibility.Visible;
            compositionsHeader.IsEnabled = true;
        }
    }
}