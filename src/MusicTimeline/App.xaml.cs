using HTMLConverter;
using NathanHarrenstein.MusicTimeline.Logging;
using NathanHarrenstein.MusicTimeline.Scrapers;
using NathanHarrenstein.MusicTimeline.Utilities;
using NathanHarrenstein.MusicTimeline.Views;
using System;
using System.ComponentModel;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows;

namespace NathanHarrenstein.MusicTimeline
{
    public partial class App : Application
    {       
        public static Logger Logger { get; private set; }

        static App()
        {
            Logger = new Logger();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            MainWindow = new MainWindow();
            MainWindow.Show();           
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Log(e.ExceptionObject as Exception);
        }
    }
}