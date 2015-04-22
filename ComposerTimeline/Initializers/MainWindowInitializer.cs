using NathanHarrenstein.Input;
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

namespace NathanHarrenstein.ComposerTimeline.Initializers
{
    public static class MainWindowInitializer
    {
        public static void Initialize(MainWindow mainWindow)
        {
            mainWindow.AddFilesCommand = GetAddFilesCommand(mainWindow);
            mainWindow.AddFolderCommand = GetAddFolderCommand(mainWindow);
            mainWindow.CloseCommand = GetCloseCommand(mainWindow);
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
                    App.Current.Properties.Add("FilesOpened", openFileDialog.FileNames);
                    App.Current.Properties.Add("FileIndex", 0);

                    var frame = (Frame)mainWindow.FindName("Frame");

                    frame.Navigate(new Uri(@"\InputPage\InputPage.xaml", UriKind.Relative));
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
                    App.Current.Properties.Add("FilesOpened", Directory.GetFiles(openFileDialog.SelectedPath, "*.flac", SearchOption.AllDirectories));
                    App.Current.Properties.Add("FileIndex", 0);

                    var frame = (Frame)mainWindow.FindName("Frame");

                    frame.Navigate(new Uri(@"\Pages\AddFilePage.xaml", UriKind.Relative));
                }
            };

            return new DelegateCommand(addFolder);
        }

        private static ICommand GetCloseCommand(MainWindow mainWindow)
        {
            Action<object> exit = o =>
            {
                App.Current.Shutdown();
            };

            return new DelegateCommand(exit);
        }
    }
}