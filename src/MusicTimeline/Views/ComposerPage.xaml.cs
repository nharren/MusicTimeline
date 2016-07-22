using Microsoft.Win32;
using NathanHarrenstein.MusicTimeline.Audio;
using NathanHarrenstein.MusicTimeline.Controls;
using NathanHarrenstein.MusicTimeline.Data;
using NathanHarrenstein.MusicTimeline.Extensions;
using NathanHarrenstein.MusicTimeline.Parsers;
using NathanHarrenstein.MusicTimeline.Utilities;
using NathanHarrenstein.MusicTimeline.ViewModels;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics;
using System.EDTF;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Collections;
using NathanHarrenstein.MusicTimeline.Converters;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class ComposerPage : Page, IDisposable
    {
        private Composer composer;
        private bool isDisposed;

#if TRACE
        private Stopwatch performanceStopwatch = new Stopwatch();
#endif

        public ComposerPage()
        {

#if TRACE
            performanceStopwatch.Start();
#endif

            InitializeComponent();

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
            if (!isDisposed)
            {
                if (disposing)
                {

                }

                mp3PlayerControl.Dispose();

                isDisposed = true;
            }
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

            biographyRichTextBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#33000000"));
            biographyRichTextBox.Padding = new Thickness(10.0);
            biographyRichTextBox.Document = BiographyUtility.LoadDocument(composer.ComposerBiography.Biography);
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

            biographyScrollViewer.Document = BiographyUtility.LoadDocument(composer.ComposerBiography.Biography);

            biographyHeader.CanEdit = true;
        }

        private void biographyToolbar_Saving(object sender, EventArgs e)
        {
            if (composer.ComposerBiography == null)
            {
                composer.ComposerBiography = new ComposerBiography();
            }

            composer.ComposerBiography.Biography = XamlWriter.Save(biographyRichTextBox.Document);

            App.ClassicalMusicContext.UpdateObject(composer);
            App.ClassicalMusicContext.SaveChanges();

            biographyRichTextBox.Document.Blocks.Clear();

            biographyEditPanel.Visibility = Visibility.Collapsed;
            biographyScrollViewer.Visibility = Visibility.Visible;

            biographyScrollViewer.Document = BiographyUtility.LoadDocument(composer.ComposerBiography.Biography);

            biographyHeader.CanEdit = true;
        }

        private void bornHeader_ButtonClick(object sender, RoutedEventArgs e)
        {
            bornEditPanel.Visibility = Visibility.Visible;
            bornTextBlock.Visibility = Visibility.Collapsed;

            bornTextBox.Text = bornTextBlock.Text;
        }

        private void bornToolbar_Cancelling(object sender, EventArgs e)
        {
            bornTextBlock.Visibility = Visibility.Visible;
            bornEditPanel.Visibility = Visibility.Collapsed;

            bornHeader.CanEdit = true;
        }

        private void bornToolbar_Saving(object sender, EventArgs e)
        {
            var splitBornText = bornTextBox.Text.Split(new string[] { "; " }, StringSplitOptions.None);
            var birthDateString = splitBornText[0];
            var birthDate = ExtendedDateTimeInterval.Parse(splitBornText[0]);

            if (birthDate == null)
            {
                MessageBox.Show("The entered date is invalid.");

                return;
            }

            composer.Dates = birthDateString;

            if (splitBornText.Length < 2)
            {
                return;
            }

            LocationUtility.UpdateBirthLocation(splitBornText[1], composer, App.ClassicalMusicContext);
            App.ClassicalMusicContext.UpdateObject(composer);
            App.ClassicalMusicContext.SaveChanges();

            bornTextBlock.Text = CreateBornText();
            bornTextBlock.Visibility = Visibility.Visible;
            bornEditPanel.Visibility = Visibility.Collapsed;
            bornHeader.CanEdit = true;
        }


        private void ComposerPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadComposer();
        }

        private void compositionsHeader_ButtonClick(object sender, RoutedEventArgs e)
        {
            compositionsPanel.Visibility = Visibility.Collapsed;
            compositionsEditPanel.Visibility = Visibility.Visible;

            compositionsListBox.ItemsSource = new SortedSet<Composition>(composer.Compositions, Comparer<Composition>.Create((x, y) => string.Compare(x.Name, y.Name)));
        }

        private void compositionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            compositionsListBox.Items.Refresh();
        }

        private void compositionsToolbar_Adding(object sender, EventArgs e)
        {
            var composition = new Composition();
            composition.Name = "New Composition";

            composer.Compositions.Add(composition);
            App.ClassicalMusicContext.AddToCompositions(composition);

            var compositionsList = (ICollection<Composition>)compositionsListBox.ItemsSource;

            compositionsList.Add(composition);
            compositionsListBox.Items.Refresh();

            compositionsListBox.SelectedItem = composition;

            compositionsListBox.ScrollIntoView(composition);
        }

        private void compositionsToolbar_Cancelling(object sender, EventArgs e)
        {
            compositionsListBox.ItemsSource = null;

            compositionsPanel.Visibility = Visibility.Visible;
            compositionsEditPanel.Visibility = Visibility.Collapsed;

            compositionsHeader.CanEdit = true;
        }

        private void compositionsToolbar_Removing(object sender, EventArgs e)
        {
            var selectedComposition = (Composition)compositionsListBox.SelectedItem;

            RemoveComposition(selectedComposition);
            App.ClassicalMusicContext.DeleteObject(selectedComposition);

            var compositionsList = (ICollection<Composition>)compositionsListBox.ItemsSource;

            compositionsList.Remove(selectedComposition);
            compositionsListBox.Items.Refresh();
        }

        private void compositionsToolbar_Saving(object sender, EventArgs e)
        {
            foreach (var composition in composer.Compositions)
            {
                if (string.IsNullOrWhiteSpace(composition.Name))
                {
                    MessageBox.Show("Could not save because a composer has an empty Name.");

                    return;
                }
            }

            App.ClassicalMusicContext.UpdateObject(composer);
            App.ClassicalMusicContext.SaveChanges();

            compositionsPanel.Compositions = composer.Compositions;

            compositionsListBox.ItemsSource = null;

            compositionsPanel.Visibility = Visibility.Visible;
            compositionsEditPanel.Visibility = Visibility.Collapsed;

            compositionsHeader.CanEdit = true;
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

            diedTextBox.Text = diedTextBlock.Text;
        }

        private void diedToolbar_Cancelling(object sender, EventArgs e)
        {
            diedTextBlock.Visibility = Visibility.Visible;
            diedEditPanel.Visibility = Visibility.Collapsed;

            diedHeader.CanEdit = true;
        }

        private void diedToolbar_Saving(object sender, EventArgs e)
        {
            var splitDiedText = diedTextBox.Text.Split(new string[] { "; " }, StringSplitOptions.None);
            var deathDateString = splitDiedText[0];
            var deathDate = ExtendedDateTimeInterval.Parse(splitDiedText[0]);

            if (deathDate == null)
            {
                MessageBox.Show("The entered date is invalid.");

                return;
            }

            composer.Dates = deathDateString;

            if (splitDiedText.Length < 2)
            {
                return;
            }

            LocationUtility.UpdateDeathLocation(splitDiedText[1], composer, App.ClassicalMusicContext);

            App.ClassicalMusicContext.UpdateObject(composer);
            App.ClassicalMusicContext.SaveChanges();

            diedTextBlock.Text = CreateDiedText();

            diedTextBlock.Visibility = Visibility.Visible;
            diedEditPanel.Visibility = Visibility.Collapsed;

            diedHeader.CanEdit = true;
        }

        private void imagesEditButton_Click(object sender, RoutedEventArgs e)
        {
            var imagesEditButton = (Button)sender;

            imagesEditButton.IsEnabled = false;

            imagesToolbar.Visibility = Visibility.Visible;
        }

        private void imagesListBox_MouseEnter(object sender, MouseEventArgs e)
        {
            var imagesEditButton = (Button)imagesListBox.Template.FindName("imagesEditButton", imagesListBox);

            if (imagesEditButton.IsEnabled)
            {
                imagesEditButton.Visibility = Visibility.Visible;
            }
        }

        private void imagesListBox_MouseLeave(object sender, MouseEventArgs e)
        {
            var imagesEditButton = (Button)imagesListBox.Template.FindName("imagesEditButton", imagesListBox);

            imagesEditButton.Visibility = Visibility.Collapsed;
        }

        private void imagesToolbar_Adding(object sender, EventArgs e)
        {
            var composerImage = new ComposerImage();

            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.gif, *.png) | *.jpg; *.jpeg; *.gif; *.png";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string filename = openFileDialog.FileName;

                List<string> images;

                if (addedImages.TryGetValue(composerImage, out images))
                {
                    images.Add(filename);
                }
                else
                {
                    addedImages.Add(composerImage, new List<string> { filename });
                }

                composer.ComposerImages.Add(composerImage);

                App.ClassicalMusicContext.AddToComposerImages(composerImage);

                imagesListBox.ItemsSource = composer.ComposerImages;
                imagesListBox.SelectedItem = composerImage;
            }
        }

        private Dictionary<ComposerImage, List<string>> addedImages = new Dictionary<ComposerImage, List<string>>();

        private void imagesToolbar_Cancelling(object sender, EventArgs e)
        {
            addedImages.Clear();

            imagesToolbar.Visibility = Visibility.Collapsed;

            var composerImagesQuery = App.ClassicalMusicContext.LoadProperty(composer, "ComposerImages");
            var composerImages = composerImagesQuery.Cast<ComposerImage>().ToList();

            imagesListBox.ItemsSource = composerImages;
            imagesListBox.SelectedIndex = 0;

            var imagesEditButton = (Button)imagesListBox.Template.FindName("imagesEditButton", imagesListBox);
            imagesEditButton.IsEnabled = true;
        }

        private void imagesToolbar_Removing(object sender, EventArgs e)
        {
            var image = (ComposerImage)imagesListBox.SelectedItem;

            composer.ComposerImages.Remove(image);

            App.ClassicalMusicContext.DeleteObject(image);

            imagesListBox.ItemsSource = composer.ComposerImages;
        }

        private void imagesToolbar_Saving(object sender, EventArgs e)
        {
            foreach (var image in addedImages)
            {
                foreach (var path in image.Value)
                {
                    App.ClassicalMusicContext.SetSaveStream(image.Key, File.OpenRead(path), true, null);
                }
            }

            App.ClassicalMusicContext.UpdateObject(composer);
            App.ClassicalMusicContext.SaveChanges();

            imagesToolbar.Visibility = Visibility.Collapsed;

            var imagesEditButton = (Button)imagesListBox.Template.FindName("imagesEditButton", imagesListBox);
            imagesEditButton.IsEnabled = true;
        }

        private void influenceButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            Application.Current.Properties["SelectedComposer"] = ((Composer)button.DataContext).ComposerId;

            LoadComposer();
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

            influencedHeader.CanEdit = true;
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

            influencedHeader.CanEdit = true;
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

            influencesHeader.CanEdit = true;
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

            influencesHeader.CanEdit = true;
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

            linksHeader.CanEdit = true;
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

            linksHeader.CanEdit = true;
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

            //var composerQuery = (DataServiceQuery<Composer>);
            //App.ClassicalMusicContext.CreateQuery<Composer>("ComposerData").AddQueryOption("id", composerId);
#if TRACE
            App.ClassicalMusicContext.SendingRequest2 += ClassicalMusicContext_SendingRequest2;
            App.ClassicalMusicContext.ReceivingResponse += ClassicalMusicContext_ReceivingResponse;
#endif

            if (!App.ClassicalMusicContext.TryGetEntity(new Uri($"http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc/Composers({composerId})"), out this.composer))
            {
                this.composer = App.ClassicalMusicContext.Execute<Composer>(new Uri($"http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc/Composers({composerId})")).Single();
            }

            App.ClassicalMusicContext.LoadProperty(composer, "Compositions");
            App.ClassicalMusicContext.LoadProperty(composer, "ComposerBiography");
            App.ClassicalMusicContext.LoadProperty(composer, "ComposerImages");
            App.ClassicalMusicContext.LoadProperty(composer, "Influences");
            App.ClassicalMusicContext.LoadProperty(composer, "Influenced");
            App.ClassicalMusicContext.LoadProperty(composer, "Samples");
            App.ClassicalMusicContext.LoadProperty(composer, "Links");

#if TRACE
            Console.WriteLine($"Data Loaded: {performanceStopwatch.Elapsed.TotalSeconds}");
#endif

            ComposerNameTextBlock.Text = NameUtility.ToFirstLast(composer.Name);

            bornTextBlock.Text = CreateBornText();
            diedTextBlock.Text = CreateDiedText();

            biographyScrollViewer.Document = BiographyUtility.LoadDocument(composer.ComposerBiography?.Biography);

            ComposerFlagsItemsControl.ItemsSource = composer.Nationalities;

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

            if (composer.ComposerImages.Count == 0)
            {
                var composerImage = new ComposerImage();
                composerImage.Composer = composer;

                imagesListBox.ItemsSource = new ComposerImage[] { composerImage };
            }
            else
            {
                foreach (var image in composer.ComposerImages)
                {
                    image.Composer = composer;
                }


                imagesListBox.ItemsSource = composer.ComposerImages;
            }

            imagesListBox.SelectedIndex = 0;

            foreach (var sample in composer.Samples)
            {
                var mp3PlaylistItem = new PlaylistItem();
                mp3PlaylistItem.GetStream = () => new NetworkMp3FileReader(App.ClassicalMusicContext.GetReadStream(sample).Stream);
                mp3PlaylistItem.Metadata.Add("Title", sample.Title);
                mp3PlaylistItem.Metadata.Add("Artist", sample.Artists);

                mp3PlayerControl.Playlist.Add(mp3PlaylistItem);

                if (mp3PlayerControl.Playlist.Count == 1)
                {
                    mp3PlayerControl.Play();
                }
            }

            compositionsPanel.Compositions = composer.Compositions;

            ProgressBar.Visibility = Visibility.Collapsed;
        }

        private void ClassicalMusicContext_ReadingEntity(object sender, ReadingWritingEntityEventArgs e)
        {
            Console.WriteLine($"Entities Read: {performanceStopwatch.Elapsed.TotalSeconds}");

            App.ClassicalMusicContext.ReadingEntity -= ClassicalMusicContext_ReadingEntity;
        }

        private void ClassicalMusicContext_SendingRequest2(object sender, SendingRequest2EventArgs e)
        {
            Console.WriteLine($"Request Sent: {performanceStopwatch.Elapsed.TotalSeconds}");

            App.ClassicalMusicContext.SendingRequest2 -= ClassicalMusicContext_SendingRequest2;
        }

        private void ClassicalMusicContext_ReceivingResponse(object sender, ReceivingResponseEventArgs e)
        {
            Console.WriteLine($"Received Response: {performanceStopwatch.Elapsed.TotalSeconds}");

            App.ClassicalMusicContext.ReceivingResponse -= ClassicalMusicContext_ReceivingResponse;
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

            mediaHeader.CanEdit = true;
        }

        private void mediaToolbar_Removing(object sender, EventArgs e)
        {
            var link = (Link)mediaListBox.SelectedItem;

            App.ClassicalMusicContext.DeleteLink(composer, "Links", link);

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

            mediaHeader.CanEdit = true;
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

            foreach (var compositionCollection in composer.CompositionCollections)
            {
                foreach (var composition in compositionCollection.Compositions)
                {
                    if (composition == selectedComposition)
                    {
                        compositionCollection.Compositions.Remove(composition);

                        return;
                    }
                }
            }

            return;
        }

        private void youTubeButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var link = (Link)button.DataContext;

            Process.Start(link.Url);
        }

        private void image_Loaded(object sender, RoutedEventArgs e)
        {
            var image = (Image)sender;
            var composerImage = (ComposerImage)image.DataContext;
            var converter = new ComposerToBitmapImageConverter();

            image.Source = converter.Convert(composerImage.Composer, new Converters.ComposerToBitmapImageConverterSettings(composerImage.ComposerImageId, true));
        }

        private void selectedImage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var image = (Image)sender;
            var composerImage = (ComposerImage)image.DataContext;
            var converter = new ComposerToBitmapImageConverter();

            image.Source = converter.Convert(composerImage.Composer, new Converters.ComposerToBitmapImageConverterSettings(composerImage.ComposerImageId, false));
        }
    }
}