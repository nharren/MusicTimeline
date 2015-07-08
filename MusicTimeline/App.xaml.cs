using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Windows;

namespace NathanHarrenstein.MusicTimeline
{
    public partial class App : System.Windows.Application
    {
        public static DataProvider DataProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            AccessibilityUtility.AllowAccessibilityShortcutKeys(false);

            DataProvider = new DataProvider();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            AccessibilityUtility.AllowAccessibilityShortcutKeys(true);

            DataProvider.Dispose();

            base.OnExit(e);
        }
    }
}