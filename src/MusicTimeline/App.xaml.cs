using NathanHarrenstein.MusicTimeline.Data;
using NathanHarrenstein.MusicTimeline.Logging;
using NathanHarrenstein.MusicTimeline.Security;
using NathanHarrenstein.MusicTimeline.Views;
using System;
using System.Data.Services.Client;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows;

namespace NathanHarrenstein.MusicTimeline
{
    public partial class App : Application
    {
        public static ClassicalMusicEntities ClassicalMusicContext { get; set; }
        public Credential Credential { get; private set; }
        public static Logger Logger { get; private set; }

        static App()
        {
            Logger = new Logger();

            ClassicalMusicContext = new ClassicalMusicEntities(new Uri("http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc/"));
            ClassicalMusicContext.Format.UseJson();
            ClassicalMusicContext.MergeOption = MergeOption.OverwriteChanges;
            ClassicalMusicContext.SendingRequest2 += ClassicalMusicContext_SendingRequest2;
        }

        private static void ClassicalMusicContext_SendingRequest2(object sender, SendingRequest2EventArgs e)
        {
            var currentApp = (App)App.Current;

            var requestMessage = e.RequestMessage as HttpWebRequestMessage;

            if (requestMessage == null)
            {
                return;
            }

            var request = requestMessage.HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            var authorizationString = $"{currentApp.Credential?.UserName}:{currentApp.Credential?.Password}";
            var authorizationBytes = Encoding.Default.GetBytes(authorizationString);
            var authorizationBase64String = Convert.ToBase64String(authorizationBytes);

            requestMessage.SetHeader("Authorization", $"Basic {authorizationBase64String}");
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