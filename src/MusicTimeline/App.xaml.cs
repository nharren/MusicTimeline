using NathanHarrenstein.MusicTimeline.Logging;
using NathanHarrenstein.MusicTimeline.Security;
using NathanHarrenstein.MusicTimeline.Views;
using System;
using System.Windows;

namespace NathanHarrenstein.MusicTimeline
{
    public partial class App : Application
    {
        public Credential Credential { get; private set; }
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

            Credential = CredentialManager.ReadCredential("MusicTimeline");

            if (Credential == null)
            {
                var loginDialog = new LoginDialog();

                if (loginDialog.ShowDialog() == true)
                {
                    CredentialManager.WriteCredential("MusicTimeline", loginDialog.UserName, loginDialog.Password);
                }
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Log(e.ExceptionObject as Exception);
        }
    }
}