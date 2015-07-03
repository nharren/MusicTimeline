using NathanHarrenstein.MusicDb;
using System;
using System.Windows;

namespace NathanHarrenstein.MusicTimeline
{
    public partial class App : Application
    {
        public static DataProvider DataProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            DataProvider = new DataProvider();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            DataProvider.Dispose();

            base.OnExit(e);
        }
    }
}