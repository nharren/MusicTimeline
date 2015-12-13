using HTMLConverter;
using Microsoft.Win32;
using NathanHarrenstein.MusicTimeline.Converters;
using NathanHarrenstein.MusicTimeline.Scrapers;
using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using NathanHarrenstein.MusicTimeline.ClassicalMusicDb;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class ComposerEditPage : Page, IDisposable
    {
        private static ClassicalMusicContext _classicalMusicContext;
        private bool _isDisposed;
        private CancellationTokenSource _loadingCancellationTokenSource;

        public ComposerEditPage()
        {
            InitializeComponent();
        }

        ~ComposerEditPage()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        internal string SelectLocationName(object obj)
        {
            return ((Location)obj).Name;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _loadingCancellationTokenSource.Cancel();
                }

                _isDisposed = true;
            }
        }

        private void BirthLocationAutoCompleteBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var composer = (Composer)DataContext;

            var birthLocationQuery = _classicalMusicContext.Locations.Local.FirstOrDefault(l => l.Name == BirthLocationAutoCompleteBox.Text);

            if (birthLocationQuery == null)
            {
                if (composer.Details.BirthLocation != null && composer.Details.BirthLocation.BirthLocationComposerDetailsCollection.Count + composer.Details.BirthLocation.DeathLocationComposerDetailsCollection.Count + composer.Details.BirthLocation.Recordings.Count == 1)
                {
                    _classicalMusicContext.Locations.Remove(composer.Details.BirthLocation);
                }

                if (string.IsNullOrEmpty(BirthLocationAutoCompleteBox.Text))
                {
                    composer.Details.BirthLocation = null;
                }
                else
                {
                    var location = new Location();
                    location.Name = BirthLocationAutoCompleteBox.Text;

                    _classicalMusicContext.Locations.Add(location);
                    composer.Details.BirthLocation = location;
                }
            }
            else
            {
                if (composer.Details.BirthLocation != null && birthLocationQuery.Name != composer.Details.BirthLocation.Name && composer.Details.BirthLocation.BirthLocationComposerDetailsCollection.Count + composer.Details.BirthLocation.DeathLocationComposerDetailsCollection.Count + composer.Details.BirthLocation.Recordings.Count == 1)
                {
                    _classicalMusicContext.Locations.Remove(composer.Details.BirthLocation);
                }

                composer.Details.BirthLocation = birthLocationQuery;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _loadingCancellationTokenSource.Cancel();

            NavigationService.GoBack();
        }

        private void CompositionDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var composer = (Composer)DataContext;
            var composition = (Composition)e.EditingElement.DataContext;

            composition.Composers.Add(composer);
        }

        private void DeathLocationAutoCompleteBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var composer = (Composer)DataContext;

            var birthLocationQuery = _classicalMusicContext.Locations.Local.FirstOrDefault(l => l.Name == BirthLocationAutoCompleteBox.Text);

            if (birthLocationQuery == null)
            {
                if (composer.Details.BirthLocation != null && composer.Details.BirthLocation.BirthLocationComposerDetailsCollection.Count + composer.Details.BirthLocation.DeathLocationComposerDetailsCollection.Count + composer.Details.BirthLocation.Recordings.Count == 1)
                {
                    _classicalMusicContext.Locations.Remove(composer.Details.BirthLocation);
                }

                if (string.IsNullOrEmpty(BirthLocationAutoCompleteBox.Text))
                {
                    composer.Details.BirthLocation = null;
                }
                else
                {
                    var location = new Location();
                    location.Name = BirthLocationAutoCompleteBox.Text;

                    _classicalMusicContext.Locations.Add(location);
                    composer.Details.BirthLocation = location;
                }
            }
            else
            {
                if (composer.Details.BirthLocation != null && birthLocationQuery.Name != composer.Details.BirthLocation.Name && composer.Details.BirthLocation.BirthLocationComposerDetailsCollection.Count + composer.Details.BirthLocation.DeathLocationComposerDetailsCollection.Count + composer.Details.BirthLocation.Recordings.Count == 1)
                {
                    _classicalMusicContext.Locations.Remove(composer.Details.BirthLocation);
                }

                composer.Details.BirthLocation = birthLocationQuery;
            }
        }

        private void EraDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var comboBox = (ComboBox)e.EditingElement;

            var oldEra = (Era)comboBox.DataContext;
            var newEra = _classicalMusicContext.Eras.Where(n => n.Name == comboBox.Text).FirstOrDefault();

            var composer = (Composer)EraDataGrid.DataContext;

            composer.Eras.Remove(oldEra);

            if (newEra != null)
            {
                composer.Eras.Add(newEra);
            }

            EraDataGrid.ItemsSource = null;
            EraDataGrid.ItemsSource = composer.Eras;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            ShowsNavigationUI = true;
        }

        private void InfluenceDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var comboBox = (ComboBox)e.EditingElement;

            var oldInfluence = (Composer)comboBox.DataContext;
            var newInfluence = _classicalMusicContext.Composers.Where(n => n.Name == comboBox.Text).FirstOrDefault();

            var composer = (Composer)InfluenceDataGrid.DataContext;

            composer.Influences.Remove(oldInfluence);

            if (newInfluence != null)
            {
                composer.Influences.Add(newInfluence);
            }

            InfluenceDataGrid.ItemsSource = null;
            InfluenceDataGrid.ItemsSource = composer.Influences;
        }

        private async void LinkDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var textBox = (TextBox)e.EditingElement;
            var composer = (Composer)DataContext;

            var url = textBox.Text;
            var link = (Link)textBox.DataContext;
            link.Url = url;

            if (!url.StartsWith("http"))
            {
                url = "http://" + url;
            }

            if (!FileUtility.WebsiteExists(url))
            {
                e.Cancel = true;

                return;
            }

            link.Composers.Add(composer);
            link.Name = UrlToTitleConverter.UrlToTitle(url);

            if (url.Contains("wikipedia"))
            {
                composer.Details.Biography = BiographyUtility.CleanXaml(HtmlToXamlConverter.ConvertHtmlToXaml(WikipediaScraper.ScrapeArticle(url), true));

                BiographyRichTextBox.Document = StringToFlowDocument(composer.Details.Biography);
            }

            if (url.Contains("charles.smith"))
            {
                ClassicalMusicNavigatorScraper.ScrapeComposer(url, composer, _classicalMusicContext);
            }

            if (url.Contains("klassika"))
            {
                var progressBar = new ProgressBar();
                progressBar.Width = 500;
                progressBar.Height = 20;
                progressBar.Maximum = 1;
                progressBar.Minimum = 0;
                progressBar.Margin = new Thickness(15);

                var progressDialog = new Window();
                progressDialog.Content = progressBar;
                progressDialog.ResizeMode = ResizeMode.NoResize;
                progressDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                progressDialog.Owner = App.Current.MainWindow;
                progressDialog.ShowInTaskbar = false;
                progressDialog.SizeToContent = SizeToContent.WidthAndHeight;
                progressDialog.Title = "Downloading Compositions";

                var progress = new Progress<double>();
                progress.ProgressChanged += (o, p) =>
                {
                    progressBar.Value = p;

                    if (p == 1d)
                    {
                        progressDialog.Close();
                    }
                };

                var cancellationTokenSource = new CancellationTokenSource();

                await KlassikaScraper.ScrapeComposerDetailPageAsync(url, composer, _classicalMusicContext, progress, cancellationTokenSource.Token);

                // An exception can occur if the function completes before the dialog window has had time to open.

                try
                {
                    progressDialog.ShowDialog();
                }
                catch (Exception)
                {

                }               
            }
        }

        private void LoadAudioButton_Click(object sender, RoutedEventArgs e)
        {
            var sample = ((Button)sender).DataContext as Sample;

            if (sample == null)
            {
                throw new InvalidOperationException("There is no Sample associated with this LoadButton.");
            }

            var openFileDialog = new OpenFileDialog();

            openFileDialog.DefaultExt = ".mp3";
            openFileDialog.Filter = "MP3 Files (*.mp3)|*.mp3";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string filename = openFileDialog.FileName;

                sample.Bytes = File.ReadAllBytes(filename);
            }
        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var composerImage = button.DataContext as ComposerImage;

            if (composerImage == null)
            {
                return;
            }

            if (composerImage.Composer == null)
            {
                composerImage.Composer = (Composer)DataContext;
            }

            var openFileDialog = new OpenFileDialog();

            openFileDialog.DefaultExt = ".jpg";
            openFileDialog.Filter = "JPG Files (*.jpg)|*.jpg";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string filename = openFileDialog.FileName;

                composerImage.Bytes = File.ReadAllBytes(filename);
            }

            DependencyObject dataGridCellQuery = button;

            while (!(dataGridCellQuery is DataGridCell))
            {
                dataGridCellQuery = VisualTreeHelper.GetParent(dataGridCellQuery);
            }

            var cell = (DataGridCell)dataGridCellQuery;
            cell.IsEditing = false;
        }

        private void NationalityDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var comboBox = (ComboBox)e.EditingElement;

            var oldNationality = (Nationality)comboBox.DataContext;
            var newNationality = _classicalMusicContext.Nationalities.Where(n => n.Name == comboBox.Text).FirstOrDefault();

            var composer = (Composer)NationalityDataGrid.DataContext;

            composer.Nationalities.Remove(oldNationality);

            if (newNationality != null)
            {
                composer.Nationalities.Add(newNationality);
            }

            NationalityDataGrid.ItemsSource = null;
            NationalityDataGrid.ItemsSource = composer.Nationalities;
        }

        private FlowDocument StringToFlowDocument(string input)
        {
            var parserContext = new ParserContext();
            parserContext.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            parserContext.XmlSpace = "preserve";

            var inputBytes = Encoding.UTF8.GetBytes(input);
            var memoryStream = new MemoryStream(inputBytes);

            return (FlowDocument)XamlReader.Load(memoryStream, parserContext);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _classicalMusicContext = new ClassicalMusicContext(new Uri("http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc"));
            _loadingCancellationTokenSource = new CancellationTokenSource();

            var composerId = (int)Application.Current.Properties["SelectedComposer"];
            var composer = await _classicalMusicContext.Composers.FirstAsync(c => c.ComposerId == composerId, _loadingCancellationTokenSource.Token);

            DataContext = composer;

            ComposerDataGrid.ItemsSource = new Composer[] { composer };            

            if (!string.IsNullOrEmpty(composer.Details.Biography))
            {
                BiographyRichTextBox.Document = StringToFlowDocument(composer.Details.Biography);
            }

            await _classicalMusicContext.Locations.LoadAsync(_loadingCancellationTokenSource.Token);

            var locationViewSource = (CollectionViewSource)FindResource("locationViewSource");
            locationViewSource.Source = _classicalMusicContext.Locations.Local;

            await _classicalMusicContext.Eras.LoadAsync(_loadingCancellationTokenSource.Token);

            var eraViewSource = (CollectionViewSource)FindResource("eraViewSource");
            eraViewSource.Source = _classicalMusicContext.Eras.Local;

            await _classicalMusicContext.Nationalities.LoadAsync(_loadingCancellationTokenSource.Token);

            var nationalityViewSource = (CollectionViewSource)FindResource("nationalityViewSource");
            nationalityViewSource.Source = _classicalMusicContext.Nationalities.Local;

            await _classicalMusicContext.Composers.LoadAsync(_loadingCancellationTokenSource.Token);

            var composerViewSource = (CollectionViewSource)FindResource("composerViewSource");
            composerViewSource.Source = _classicalMusicContext.Composers.Local;

            await _classicalMusicContext.Keys.LoadAsync(_loadingCancellationTokenSource.Token);

            var keyViewSource = (CollectionViewSource)FindResource("keyViewSource");
            keyViewSource.Source = _classicalMusicContext.Keys.Local;
        }

        private void RichTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var richTextBox = (RichTextBox)sender;

            ((Composer)DataContext).Details.Biography = XamlWriter.Save(richTextBox.Document);
        }

        private void SaveAudioButton_Click(object sender, RoutedEventArgs e)
        {
            var sample = ((Button)sender).DataContext as Sample;

            if (sample == null)
            {
                throw new InvalidOperationException("There is no Sample associated with this LoadButton.");
            }

            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.DefaultExt = ".mp3";
            saveFileDialog.Filter = "MP3 Files (*.mp3)|*.mp3";
            saveFileDialog.FileName = sample.Title;

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                saveFileDialog.FileName = saveFileDialog.FileName.Replace(c, '_');
            }

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string filename = saveFileDialog.FileName;

                File.WriteAllBytes(filename, sample.Bytes);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var composer = (Composer)DataContext;

            foreach (var catalog in composer.Catalogs)
            {
                catalog.Composer = composer;
            }

            foreach (var image in composer.ComposerImages)
            {
                image.Composer = composer;
            }

            _classicalMusicContext.Catalogs.RemoveRange(_classicalMusicContext.Catalogs.Local.Where(c => c.Composer == null).ToArray());
            _classicalMusicContext.ComposerImages.RemoveRange(_classicalMusicContext.ComposerImages.Local.Where(c => c.Composer == null).ToArray());

            _classicalMusicContext.SaveChanges();

            NavigationService.GoBack();
        }

        private void SaveImageButton_Click(object sender, RoutedEventArgs e)
        {
            var composerImage = ((Button)sender).DataContext as ComposerImage;

            if (composerImage == null)
            {
                return;
            }

            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.DefaultExt = ".jpg";
            saveFileDialog.Filter = "JPG Files (*.jpg)|*.jpg";
            saveFileDialog.FileName = composerImage.Composer.Name;

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                saveFileDialog.FileName = saveFileDialog.FileName.Replace(c, '_');
            }

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string filename = saveFileDialog.FileName;

                File.WriteAllBytes(filename, composerImage.Bytes);
            }
        }
    }
}