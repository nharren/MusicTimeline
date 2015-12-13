using NathanHarrenstein.MusicTimeline.ClassicalMusicDb;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class ComposersEditPage : Page, IDisposable
    {
        private static ClassicalMusicContext _classicalMusicContext;
        private bool _isDisposed = false;
        private CancellationTokenSource _loadingCancellationTokenSource;

        public ComposersEditPage()
        {
            InitializeComponent();
        }

        ~ComposersEditPage()
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

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _classicalMusicContext = new ClassicalMusicContext(new Uri("http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc"));
            _loadingCancellationTokenSource = new CancellationTokenSource();

            await _classicalMusicContext.Composers.LoadAsync(_loadingCancellationTokenSource.Token);

            var composerViewSource = (CollectionViewSource)FindResource("composerViewSource");
            composerViewSource.Source = _classicalMusicContext.Composers;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var composersWithoutDetails = _classicalMusicContext.Composers.Where(c => c.Details == null);

            foreach (var composer in composersWithoutDetails)
            {
                composer.Details = new ComposerDetails();
            }

            _classicalMusicContext.SaveChanges();

            NavigationService.GoBack();
        }
    }
}