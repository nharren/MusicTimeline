using NathanHarrenstein.MusicTimeline.ClassicalMusicDb;
using NathanHarrenstein.MusicTimeline.Converters;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class CompositionEditPage : Page, IDisposable
    {
        private ClassicalMusicContext _classicalMusicContext;
        private bool _isDisposed;
        private CancellationTokenSource _loadingCancellationTokenSource;

        public CompositionEditPage()
        {
            InitializeComponent();
        }

        ~CompositionEditPage()
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
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _loadingCancellationTokenSource.Cancel();
                }

                _isDisposed = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _loadingCancellationTokenSource.Cancel();

            NavigationService.GoBack();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            ShowsNavigationUI = true;
        }

        private void LinkDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var textBox = (TextBox)e.EditingElement;
            var composition = (Composition)DataContext;

            var url = textBox.Text;
            var link = (Link)textBox.DataContext;
            link.Compositions.Add(composition);
            link.Name = UrlToTitleConverter.UrlToTitle(url);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _classicalMusicContext = new ClassicalMusicContext(new Uri("http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc"));
            _loadingCancellationTokenSource = new CancellationTokenSource();

            var composerId = (int)Application.Current.Properties["SelectedComposer"];
            var composer = await _classicalMusicContext.Composers.FirstAsync(c => c.ComposerId == composerId, _loadingCancellationTokenSource.Token);

            var compositionId = (int)Application.Current.Properties["SelectedComposition"];
            var composition = composer.Compositions.FirstOrDefault(c => c.CompositionId == compositionId);

            DataContext = composition;

            var compositionArray = new Composition[] { composition };

            CompositionDataGrid.ItemsSource = compositionArray;
            CompositionDataGrid2.ItemsSource = compositionArray;
            CommentDataGrid.ItemsSource = compositionArray;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _classicalMusicContext.SaveChanges();

            NavigationService.GoBack();
        }
    }
}