using System;
using System.Windows.Controls;

namespace NathanHarrenstein.ComposerTimeline
{
    public partial class RecordingSection : UserControl
    {
        public RecordingSection()
        {
            InitializeComponent();

            var selectedFiles = App.Current.Properties["FilesOpened"] as string[];

            if (selectedFiles == null || selectedFiles.Length == 0)
            {
                throw new InvalidOperationException("No files were opened.");
            }

            var selectedFileIndex = (int)App.Current.Properties["FileIndex"];

            TargetText.Text = selectedFiles[selectedFileIndex];

            PerformersBox.Suggestions = new string[] { "dog", "dive", "down", "dip", "dot", "dawn", "disaster", "distaste" };
        }
    }
}