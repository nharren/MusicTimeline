using Microsoft.Win32;
using MusicTimelineWebApi.Models;
using NathanHarrenstein.MusicTimeline.Audio;
using NathanHarrenstein.MusicTimeline.Converters;
using NathanHarrenstein.MusicTimeline.Parsers;
using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics;
using System.EDTF;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private ComposerDetail composer;
        private bool isDisposed;

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

        private async void ComposerPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadComposer();        
        }

        private string CreateBornText()
        {
            if (composer.BirthLocation == null)
            {
                return ExtendedDateTimeInterval.Parse(composer.Dates).Start.ToString();
            }
            else
            {
                return $"{ExtendedDateTimeInterval.Parse(composer.Dates).Start}; {composer.BirthLocation}";
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
                return $"{ExtendedDateTimeInterval.Parse(composer.Dates).End}; {composer.DeathLocation}";
            }
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

            Application.Current.Properties["SelectedComposer"] = composer.Id;

            var mainWindow = (NavigationWindow)Application.Current.MainWindow;
            mainWindow.NavigationService.Refresh();
        }

        private async Task LoadComposer()
        {
            if (ProgressBar.Visibility == Visibility.Collapsed)
            {
                ProgressBar.Visibility = Visibility.Visible;
            }

            var composerId = (int)Application.Current.Properties["SelectedComposer"];
            composer = await WebApiInterface.GetComposerAsync(composerId);

            ComposerNameTextBlock.Text = NameUtility.ToFirstLast(composer.Name);

            bornTextBlock.Text = CreateBornText();
            diedTextBlock.Text = CreateDiedText();

            biographyScrollViewer.Document = BiographyUtility.LoadDocument(composer.Biography);
            biographyScrollViewer.Document.Foreground = App.Current.TryFindResource("ForegroundBrush") as SolidColorBrush;

            influencesItemsControl.ItemsSource = composer.Influences;

            var influencesVisibility = composer.Influences.Count() > 0 ? Visibility.Visible : Visibility.Collapsed;

            influencesItemsControl.Visibility = influencesVisibility;
            influencesHeader.Visibility = influencesVisibility;

            influencedItemsControl.ItemsSource = composer.Influenced;

            var influencedVisibility = composer.Influenced.Count() > 0 ? Visibility.Visible : Visibility.Collapsed;

            influencedItemsControl.Visibility = influencedVisibility;
            influencedHeader.Visibility = influencedVisibility;

            var youTubeParser = new YouTubeParser();
            var spotifyParser = new SpotifyParser();

            var filteredItems = new List<string>();

            foreach (var link in composer.Links)
            {
                if (youTubeParser.IsValidVideoUrl(link) || spotifyParser.IsValidTrackUrl(link))
                {
                    filteredItems.Add(link);
                }
            }

            mediaItemsControl.ItemsSource = filteredItems;

            var mediaVisibility = filteredItems.Count() > 0 ? Visibility.Visible : Visibility.Collapsed;

            mediaItemsControl.Visibility = mediaVisibility;
            mediaHeader.Visibility = mediaVisibility;

            filteredItems = new List<string>();

            foreach (var link in composer.Links)
            {
                if (!youTubeParser.IsValidVideoUrl(link) && !spotifyParser.IsValidTrackUrl(link))
                {
                    filteredItems.Add(link);
                }
            }

            linksItemsControl.ItemsSource = filteredItems;

            var linksVisibility = filteredItems.Count() > 0 ? Visibility.Visible : Visibility.Collapsed;

            linksItemsControl.Visibility = linksVisibility;
            linksHeader.Visibility = linksVisibility;            

            foreach (var sample in composer.Samples)
            {
                var mp3PlaylistItem = new PlaylistItem();
                mp3PlaylistItem.StreamUri = new Uri(WebApiInterface.ApiRoot + sample.Url);
                mp3PlaylistItem.Metadata.Add("Title", sample.Title);
                mp3PlaylistItem.Metadata.Add("Artist", sample.Artist);

                mp3PlayerControl.Playlist.Add(mp3PlaylistItem);

                if (mp3PlayerControl.Playlist.Count == 1)
                {
                    mp3PlayerControl.Play();
                }
            }

            imagesPanel.Composer = composer;
            nationalitiesPanel.Composer = composer;
            compositionsPanel.Compositions = composer.Compositions;

            ProgressBar.Visibility = Visibility.Collapsed;
        }

        private void MainWindow_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            Dispose();

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.Navigating -= MainWindow_Navigating;
        }       
        
        //private void selectedImage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    var image = (Image)sender;
        //    var composerImage = (ComposerImage)image.DataContext;
        //    var converter = new ComposerToBitmapImageConverter();

        //    image.Source = converter.Convert(composerImage.Composer, new ComposerToBitmapImageConverterSettings(composerImage.ComposerImageId, false));
        //}

        private void youTubeButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var link = (string)button.DataContext;

            Process.Start(link);
        }
    }
}