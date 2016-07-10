using Microsoft.Win32;
using NathanHarrenstein.MusicTimeline.Audio;
using NathanHarrenstein.MusicTimeline.ClassicalMusicDb;
using NathanHarrenstein.MusicTimeline.Controls;
using NathanHarrenstein.MusicTimeline.Extensions;
using NathanHarrenstein.MusicTimeline.Parsers;
using NathanHarrenstein.MusicTimeline.Utilities;
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

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class ComposerPage : Page, IDisposable
    {
        private ClassicalMusicContext classicalMusicContext;
        private Composer composer;
        private bool isDisposed;
        private CancellationTokenSource loadingCancellationTokenSource;

        public ComposerPage()
        {
            InitializeComponent();

            classicalMusicContext = new ClassicalMusicContext(new Uri("http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc"));
            classicalMusicContext.MergeOption = MergeOption.OverwriteChanges;
            classicalMusicContext.SendingRequest2 += classicalMusicContext_SendingRequest2;

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
                    loadingCancellationTokenSource.Cancel();
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
            biographyRichTextBox.Document = BiographyUtility.LoadDocument(composer.Biography);
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

            biographyScrollViewer.Document = BiographyUtility.LoadDocument(composer.Biography);

            biographyHeader.CanEdit = true;
        }

        private void biographyToolbar_Saving(object sender, EventArgs e)
        {
            composer.Biography = XamlWriter.Save(biographyRichTextBox.Document);

            classicalMusicContext.UpdateObject(composer);
            classicalMusicContext.SaveChanges();

            biographyRichTextBox.Document.Blocks.Clear();

            biographyEditPanel.Visibility = Visibility.Collapsed;
            biographyScrollViewer.Visibility = Visibility.Visible;

            biographyScrollViewer.Document = BiographyUtility.LoadDocument(composer.Biography);

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

            LocationUtility.UpdateBirthLocation(splitBornText[1], composer, classicalMusicContext);
            classicalMusicContext.UpdateObject(composer);
            classicalMusicContext.SaveChanges();

            bornTextBlock.Text = CreateBornText();
            bornTextBlock.Visibility = Visibility.Visible;
            bornEditPanel.Visibility = Visibility.Collapsed;
            bornHeader.CanEdit = true;
        }

        private void classicalMusicContext_SendingRequest2(object sender, SendingRequest2EventArgs e)
        {
            var app = (App)App.Current;

            if (app.Credential == null)
            {
                return;
            }

            var authorizationString = $"{app.Credential.UserName}:{app.Credential.Password}";
            var authorizationBytes = Encoding.Default.GetBytes(authorizationString);
            var authorizationBase64String = Convert.ToBase64String(authorizationBytes);

            e.RequestMessage.SetHeader("Authorization", $"Basic {authorizationBase64String}");
        }

        private async void ComposerPage_Loaded(object sender, RoutedEventArgs e)
        {
            imagesToolbar.Adding += imagesToolbar_Adding;
            imagesToolbar.Removing += imagesToolbar_Removing;
            imagesToolbar.Saving += imagesToolbar_Saving;
            imagesToolbar.Cancelling += imagesToolbar_Cancelling;
            bornToolbar.Saving += bornToolbar_Saving;
            bornToolbar.Cancelling += bornToolbar_Cancelling;
            diedToolbar.Saving += diedToolbar_Saving;
            diedToolbar.Cancelling += diedToolbar_Cancelling;
            influencesToolbar.Saving += influencesToolbar_Saving;
            influencesToolbar.Cancelling += influencesToolbar_Cancelling;
            influencedToolbar.Saving += influencedToolbar_Saving;
            influencedToolbar.Cancelling += influencedToolbar_Cancelling;
            mediaToolbar.Adding += mediaToolbar_Adding;
            mediaToolbar.Removing += mediaToolbar_Removing;
            mediaToolbar.Saving += mediaToolbar_Saving;
            mediaToolbar.Cancelling += mediaToolbar_Cancelling;
            linksToolbar.Adding += linksToolbar_Adding;
            linksToolbar.Removing += linksToolbar_Removing;
            linksToolbar.Saving += linksToolbar_Saving;
            linksToolbar.Cancelling += linksToolbar_Cancelling;
            biographyToolbar.Saving += biographyToolbar_Saving;
            biographyToolbar.Cancelling += biographyToolbar_Cancelling;
            biographyRichTextBox.RequestBringIntoView += biographyTextBox_RequestBringIntoView;
            compositionsToolbar.Adding += compositionsToolbar_Adding;
            compositionsToolbar.Removing += compositionsToolbar_Removing;
            compositionsToolbar.Saving += compositionsToolbar_Saving;
            compositionsToolbar.Cancelling += compositionsToolbar_Cancelling;

            await LoadComposerAsync();
        }

        private async void compositionsHeader_ButtonClick(object sender, RoutedEventArgs e)
        {
            compositionsPanel.Visibility = Visibility.Collapsed;
            compositionsEditPanel.Visibility = Visibility.Visible;

            var compositionsUri = new Uri($"http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc/Compositions?$filter=Composers/any(d:d/Name eq '{composer.Name}')&$expand=Genre,Key,Movements,CatalogNumbers,CatalogNumbers/Catalog");

            var compositions = await classicalMusicContext.ExecuteAsync<Composition>(compositionsUri, null);
            compositionsListBox.ItemsSource = new SortedSet<Composition>(compositions, Comparer<Composition>.Create((x, y) => string.Compare(x.Name, y.Name)));
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
            classicalMusicContext.AddToCompositions(composition);

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
            classicalMusicContext.DeleteObject(selectedComposition);

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

            classicalMusicContext.UpdateObject(composer);
            classicalMusicContext.SaveChanges();

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

            LocationUtility.UpdateDeathLocation(splitDiedText[1], composer, classicalMusicContext);

            classicalMusicContext.UpdateObject(composer);
            classicalMusicContext.SaveChanges();

            diedTextBlock.Text = CreateDiedText();

            diedTextBlock.Visibility = Visibility.Visible;
            diedEditPanel.Visibility = Visibility.Collapsed;

            diedHeader.CanEdit = true;
        }

        private ComposerImage GetDefaultComposerImage()
        {
            var defaultComposerImageUri = new Uri("pack://application:,,,/Resources/Composers/Unknown.jpg", UriKind.Absolute);
            var streamResourceInfo = Application.GetResourceStream(defaultComposerImageUri);

            var composerImage = new ComposerImage();
            composerImage.Bytes = StreamUtility.ReadToEnd(streamResourceInfo.Stream);

            return composerImage;
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

                composerImage.Bytes = File.ReadAllBytes(filename);

                composer.ComposerImages.Add(composerImage);

                classicalMusicContext.AddToComposerImages(composerImage);

                imagesListBox.ItemsSource = composer.ComposerImages;
                imagesListBox.SelectedItem = composerImage;
            }
        }

        private void imagesToolbar_Cancelling(object sender, EventArgs e)
        {
            imagesToolbar.Visibility = Visibility.Collapsed;

            var composerImagesQuery = classicalMusicContext.LoadProperty(composer, "ComposerImages");
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

            classicalMusicContext.DeleteObject(image);

            imagesListBox.ItemsSource = composer.ComposerImages;
        }

        private void imagesToolbar_Saving(object sender, EventArgs e)
        {
            classicalMusicContext.UpdateObject(composer);
            classicalMusicContext.SaveChanges();

            imagesToolbar.Visibility = Visibility.Collapsed;

            var imagesEditButton = (Button)imagesListBox.Template.FindName("imagesEditButton", imagesListBox);
            imagesEditButton.IsEnabled = true;
        }

        private async void influenceButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            Application.Current.Properties["SelectedComposer"] = ((Composer)button.DataContext).ComposerId;

            await LoadComposerAsync();
        }

        private void influencedHeader_ButtonClick(object sender, RoutedEventArgs e)
        {
            influencedEditPanel.Visibility = Visibility.Visible;
            influencedItemsControl.Visibility = Visibility.Collapsed;

            influencedListBox.ItemsSource = classicalMusicContext.Composers.OrderBy(c => c.Name);

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

            classicalMusicContext.UpdateObject(composer);
            classicalMusicContext.SaveChanges();

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

            influencesListBox.ItemsSource = classicalMusicContext.Composers.OrderBy(c => c.Name);

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

            classicalMusicContext.UpdateObject(composer);
            classicalMusicContext.SaveChanges();

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

            classicalMusicContext.AddRelatedObject(composer, "Links", link);
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

            classicalMusicContext.DeleteLink(composer, "Links", link);

            composer.Links.Remove(link);

            classicalMusicContext.DeleteObject(link);

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

            classicalMusicContext.UpdateObject(composer);
            classicalMusicContext.SaveChanges();

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

        private async Task LoadComposerAsync()
        {
            loadingCancellationTokenSource = new CancellationTokenSource();

            if (ProgressBar.Visibility == Visibility.Collapsed)
            {
                ProgressBar.Visibility = Visibility.Visible;
            }

            var composerId = (int)Application.Current.Properties["SelectedComposer"];

            var composerUri = new Uri($"http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc/Composers?$filter=ComposerId eq {composerId}&$expand=BirthLocation,DeathLocation,Nationalities,Influences,Influenced,Links");
            var composerQuery = await classicalMusicContext.ExecuteAsync<Composer>(composerUri, null);

            composer = composerQuery.First();

            ComposerNameTextBlock.Text = NameUtility.ToFirstLast(composer.Name);

            bornTextBlock.Text = CreateBornText();
            diedTextBlock.Text = CreateDiedText();

            biographyScrollViewer.Document = BiographyUtility.LoadDocument(composer.Biography);

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

            var composerImagesQuery = await classicalMusicContext.LoadPropertyAsync(composer, "ComposerImages");
            var composerImages = composerImagesQuery.Cast<ComposerImage>().ToList();

            if (composerImages.Count == 0)
            {
                imagesListBox.ItemsSource = new ComposerImage[] { GetDefaultComposerImage() };
            }
            else
            {
                imagesListBox.ItemsSource = composerImages;
            }

            imagesListBox.SelectedIndex = 0;

            var compositionsUri = new Uri($"http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc/Compositions?$filter=Composers/any(d:d/Name eq '{composer.Name}')&$expand=Genre,Key,Movements,CatalogNumbers,CatalogNumbers/Catalog");

            var compositions = await classicalMusicContext.ExecuteAsync<Composition>(compositionsUri, null);
            compositionsPanel.Compositions = compositions;

            var samplesQueryOperationResponse = await classicalMusicContext.LoadPropertyAsync(composer, "Samples");
            var samples = samplesQueryOperationResponse.Cast<Sample>();

            foreach (var sample in samples)
            {
                var mp3FileReader = new Mp3FileReader(new MemoryStream(sample.Bytes));

                var mp3PlaylistItem = new Mp3PlaylistItem();
                mp3PlaylistItem.Stream = mp3FileReader;
                mp3PlaylistItem.Metadata.Add("Title", sample.Title);
                mp3PlaylistItem.Metadata.Add("Artist", sample.Artists);

                mp3PlayerControl.Playlist.Add(mp3PlaylistItem);

                if (mp3PlayerControl.Playlist.Count == 1)
                {
                    mp3PlayerControl.Play();
                }
            }

            ProgressBar.Visibility = Visibility.Collapsed;
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

            classicalMusicContext.AddRelatedObject(composer, "Links", link);
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

            classicalMusicContext.DeleteLink(composer, "Links", link);

            composer.Links.Remove(link);

            classicalMusicContext.DeleteObject(link);

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

            classicalMusicContext.UpdateObject(composer);
            classicalMusicContext.SaveChanges();

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
    }
}