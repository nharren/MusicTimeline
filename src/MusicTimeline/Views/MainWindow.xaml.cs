using System;
using System.ComponentModel;
using System.Windows.Navigation;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var page = Content as IDisposable;

            if (page != null)
            {
                page.Dispose();
            }
        }
    }
}