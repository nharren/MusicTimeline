using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Windows;

namespace NathanHarrenstein.MusicTimeline
{
    public partial class App : Application
    {
        public static DataProvider DataProvider;

        protected override void OnExit(ExitEventArgs e)
        {
            AccessibilityUtility.AllowAccessibilityShortcutKeys(true);

            DataProvider.Dispose();

            base.OnExit(e);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            AccessibilityUtility.AllowAccessibilityShortcutKeys(false);

            DataProvider = new DataProvider();

            base.OnStartup(e);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Log(e.ExceptionObject.ToString(), "MusicTimeline.log");
        }
    }
}