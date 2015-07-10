using NathanHarrenstein.MusicTimeline.Input;
using NathanHarrenstein.MusicTimeline.Views;
using System;
using System.EDTF;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NathanHarrenstein.MusicTimeline.Initializers
{
    public static class MainWindowInitializer
    {
        public static void Initialize(MainWindow mainWindow)
        {
            mainWindow.ManageDataCommand = GetManageDataCommand(mainWindow);
            mainWindow.CloseCommand = GetCloseCommand(mainWindow);
            mainWindow.GoToCommand = GetGoToCommand(mainWindow);
        }

        private static ICommand GetGoToCommand(MainWindow mainWindow)
        {
            Action<object> goTo = o =>
            {
                var year = new ExtendedDateTime(int.Parse((string)o));
                var frame = (Frame)mainWindow.FindName("Frame");
                var page = frame.Content as TimelinePage;

                if (page == null)
                {
                    return;
                }

                page.timeline.HorizontalOffset = page.timeline.Ruler.ToPixels(page.timeline.Dates.Earliest(), year);
            };

            return new DelegateCommand(goTo);
        }

        private static ICommand GetAddFilesCommand(MainWindow mainWindow)
        {
            Action<object> addFiles = o =>
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.DefaultExt = ".flac";
                openFileDialog.Filter = "FLAC Audio File (.flac)|*.flac";

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    System.Windows.Application.Current.Properties.Add("FilesOpened", openFileDialog.FileNames);
                    System.Windows.Application.Current.Properties.Add("FileIndex", 0);

                    var frame = (Frame)mainWindow.FindName("Frame");

                    frame.Navigate(new Uri(@"pack://application:,,,/Views/InputPage.xaml", UriKind.Absolute));
                }
            };

            return new DelegateCommand(addFiles);
        }

        private static ICommand GetAddFolderCommand(MainWindow mainWindow)
        {
            Action<object> addFolder = o =>
            {
                var openFileDialog = new System.Windows.Forms.FolderBrowserDialog();

                var result = openFileDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    System.Windows.Application.Current.Properties.Add("FilesOpened", Directory.GetFiles(openFileDialog.SelectedPath, "*.flac", SearchOption.AllDirectories));
                    System.Windows.Application.Current.Properties.Add("FileIndex", 0);

                    var frame = (Frame)mainWindow.FindName("Frame");

                    frame.Navigate(new Uri(@"pack://application:,,,/Views/AddFilePage.xaml", UriKind.Absolute));
                }
            };

            return new DelegateCommand(addFolder);
        }

        private static ICommand GetCloseCommand(MainWindow mainWindow)
        {
            Action<object> exit = o =>
            {
                System.Windows.Application.Current.Shutdown();
            };

            return new DelegateCommand(exit);
        }

        private static ICommand GetManageDataCommand(MainWindow mainWindow)
        {
            Action<object> manageData = o =>
            {
                mainWindow.Frame.Navigate(new Uri(@"pack://application:,,,/Views/InputPage.xaml", UriKind.Absolute));
            };

            return new DelegateCommand(manageData);
        }
    }
}