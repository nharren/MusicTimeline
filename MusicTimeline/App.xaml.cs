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
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;

            DataProvider = new DataProvider();

            base.OnStartup(e);
        }

        private void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            DataProvider.Dispose();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            DataProvider.Dispose();

            base.OnExit(e);
        }

        internal static void EnsureDbConnection()
        {
            if (DataProvider.Database.Connection.State != System.Data.ConnectionState.Open)
            {
                DataProvider.Database.Connection.Open();
            }
        }
    }
}