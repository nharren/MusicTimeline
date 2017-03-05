using Microsoft.Win32;
using NathanHarrenstein.MusicTimeline.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class SamplesEditPanel : Control
    {
        public static readonly DependencyProperty ComposerProperty = DependencyProperty.Register("Composer", typeof(Composer), typeof(SamplesEditPanel), new PropertyMetadata(null, new PropertyChangedCallback(Composer_Changed)));

        private List<Sample> addedSamples;
        private bool isTemplateApplied;
        private ListBox listBox;
        private Dictionary<Sample, MemoryStream> pendingUploads;
        private List<Sample> removedSamples;
        private TextBlock sampleSizeTextBlock;
        private ObservableCollection<Sample> sampleSource;

        static SamplesEditPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SamplesEditPanel), new FrameworkPropertyMetadata(typeof(SamplesEditPanel)));
        }

        public SamplesEditPanel()
        {
            pendingUploads = new Dictionary<Sample, MemoryStream>();
            addedSamples = new List<Sample>();
            removedSamples = new List<Sample>();

            IsVisibleChanged += samplesEditPanel_IsVisibleChanged;
        }

        public event EventHandler Cancelling;
        public event EventHandler Saving;

        public Composer Composer
        {
            get
            {
                return (Composer)GetValue(ComposerProperty);
            }

            set
            {
                SetValue(ComposerProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            if (isTemplateApplied)
            {
                return;
            }

            var toolbar = (AddSaveToolbar)GetTemplateChild("toolbar");
            listBox = (ListBox)GetTemplateChild("listBox");

            if (Composer != null)
            {
                sampleSource = new ObservableCollection<Sample>(Composer.Samples);
                listBox.ItemsSource = sampleSource;

            }

            toolbar.Adding += toolbar_Adding;
            toolbar.Removing += toolbar_Removing;
            toolbar.Cancelling += toolbar_Cancelling;
            toolbar.Saving += toolbar_Saving;

            listBox.SelectionChanged += listBox_SelectionChanged;

            isTemplateApplied = true;
        }

        protected virtual void OnCancelling(EventArgs e)
        {
            if (Cancelling != null)
            {
                Cancelling(this, e);
            }
        }

        protected virtual void OnSaving(EventArgs e)
        {
            if (Saving != null)
            {
                Saving(this, e);
            }
        }

        private static void Composer_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var samplesEditPanel = (SamplesEditPanel)d;

            if (!samplesEditPanel.isTemplateApplied)
            {
                return;
            }

            samplesEditPanel.sampleSource = new ObservableCollection<Sample>(samplesEditPanel.Composer.Samples);
            samplesEditPanel.listBox.ItemsSource = samplesEditPanel.sampleSource;
        }

        private string GetSampleSizeText(Sample sample)
        {
            if (addedSamples.Contains(sample))
            {
                MemoryStream sampleStream;

                if (pendingUploads.TryGetValue(sample, out sampleStream))
                {
                    return $"{sampleStream.Length} bytes";
                }

                return "No File Attached.";
            }

            var streamUri = App.ClassicalMusicContext.GetReadStreamUri(sample);

            WebRequest request = WebRequest.Create(streamUri);

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    return $"{response.ContentLength} bytes";
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);

                return "Network Error";
            }
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedItem == null)
            {
                return;
            }

            listBox.Items.Refresh();
            UpdateLayout();
            listBox.ScrollIntoView(listBox.SelectedItem); // Ensures container is generated from virtualizing panel.

            var listBoxItem = (ListBoxItem)listBox.ItemContainerGenerator.ContainerFromItem(listBox.SelectedItem);
            var contentPresenter = (ContentPresenter)listBoxItem.Template.FindName("contentPresenter", listBoxItem);
            var contentTemplateSelector = (SelectionDataTemplateSelector)contentPresenter.ContentTemplateSelector;
            var contentTemplate = contentTemplateSelector.SelectedTemplate;

            sampleSizeTextBlock = (TextBlock)contentTemplate.FindName("sampleSizeTextBlock", contentPresenter);
            var sampleLoadButton = (Button)contentTemplate.FindName("sampleLoadButton", contentPresenter);

            sampleSizeTextBlock.Text = GetSampleSizeText((Sample)listBox.SelectedItem);

            sampleLoadButton.Click += sampleLoadButton_Click;
        }

        private void sampleLoadButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var sample = (Sample)button.DataContext;

            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var memoryStream = new MemoryStream();

                    using (var stream = File.OpenRead(openFileDialog.FileName))
                    {
                        stream.CopyTo(memoryStream);
                        memoryStream.Position = 0;
                        sampleSizeTextBlock.Text = $"{stream.Length} bytes";

                        if (pendingUploads.ContainsKey(sample))
                        {
                            pendingUploads[sample] = memoryStream;
                        }
                        else
                        {
                            pendingUploads.Add(sample, memoryStream);
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("File could not be read.");

                    return;
                }
            }
        }

        private void samplesEditPanel_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                if (Composer != null && isTemplateApplied)
                {
                    sampleSource = new ObservableCollection<Sample>(Composer.Samples);
                    listBox.ItemsSource = sampleSource;
                }
            }
            else
            {
                pendingUploads.Clear();
                addedSamples.Clear();
                removedSamples.Clear();
                sampleSource.Clear();
            }
        }

        private void toolbar_Adding(object sender, EventArgs e)
        {
            var sample = new Sample();
            sample.Title = "[Title]";
            sample.Artists = "[Artists]";
            sample.Composer = Composer;
            sample.ComposerID = Composer.ComposerId;

            addedSamples.Add(sample);
            sampleSource.Add(sample);

            listBox.SelectedItem = sample;

            listBox.ScrollIntoView(sample);
        }

        private void toolbar_Cancelling(object sender, EventArgs e)
        {
            Visibility = Visibility.Collapsed;

            OnCancelling(e);
        }

        private void toolbar_Removing(object sender, EventArgs e)
        {
            var sample = (Sample)listBox.SelectedItem;
            sampleSource.Remove(sample);

            foreach (var addedSample in addedSamples)
            {
                if (addedSample == sample)
                {
                    addedSamples.Remove(sample);

                    break;
                }
            }

            if (pendingUploads.ContainsKey(sample))
            {
                pendingUploads.Remove(sample);
            }

            removedSamples.Add(sample);
        }

        private void toolbar_Saving(object sender, EventArgs e)
        {
            foreach (var addedSample in addedSamples)
            {
                if (!ValidateSample(addedSample))
                {
                    return;
                }
            }

            foreach (var sample in addedSamples)
            {
                Composer.Samples.Add(sample);

                MemoryStream stream;

                if (pendingUploads.TryGetValue(sample, out stream))
                {
                    App.ClassicalMusicContext.SetSaveStream(sample, stream, true, "audio/mpeg", string.Empty);
                }
            }

            foreach (var sample in removedSamples)
            {
                App.ClassicalMusicContext.DeleteObject(sample);
                Composer.Samples.Remove(sample);
            }

            try
            {
                App.ClassicalMusicContext.SaveChanges();
                App.ClassicalMusicContext.LoadProperty(Composer, "Samples");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Changes were not saved: {ex.Message}");
            }

            Visibility = Visibility.Collapsed;

            OnSaving(e);
        }

        private bool ValidateSample(Sample sample)
        {
            if (string.IsNullOrWhiteSpace(sample.Title))
            {
                MessageBox.Show("Title fields cannot be null or whitespace.");

                return false;
            }

            if (string.IsNullOrWhiteSpace(sample.Artists))
            {
                MessageBox.Show("Artists fields cannot be null or whitespace.");

                return false;
            }

            return true;
        }
    }
}