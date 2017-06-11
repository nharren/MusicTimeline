using MusicTimelineWebApi.Models;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class ImagesPanel : Control
    {
        public static readonly DependencyProperty ComposerProperty = DependencyProperty.Register("Composer", typeof(ComposerDetail), typeof(ImagesPanel), new PropertyMetadata(null, new PropertyChangedCallback(Composer_Changed)));

        private Button editButton;
        private ListBox listBox;
        private Image selectedImage;

        static ImagesPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImagesPanel), new FrameworkPropertyMetadata(typeof(ImagesPanel)));
        }

        public ImagesPanel()
        {

        }

        public ComposerDetail Composer
        {
            get
            {
                return (ComposerDetail)GetValue(ComposerProperty);
            }

            set
            {
                SetValue(ComposerProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            listBox = GetTemplateChild("listBox") as ListBox;
            selectedImage = GetTemplateChild("selectedImage") as Image;
            editButton = GetTemplateChild("editButton") as Button;

            listBox.SelectionChanged += listBox_SelectionChanged;

            UpdateData();
        }

        private static void Composer_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imagesPanel = (ImagesPanel)d;
            imagesPanel.UpdateData();
        }

        private IEnumerable GetThumbnails()
        {
            if (Composer == null)
            {
                yield break;
            }

            foreach (var composerImage in Composer.Images)
            {
                var uri = new Uri(WebApiInterface.ApiRoot + composerImage);
                var bitmapImage = new BitmapImage(uri);
                var image = new Image();
                image.DataContext = composerImage;
                image.Source = bitmapImage;
                image.Height = 30;

                yield return image;
            }
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var image = listBox.SelectedItem as Image;

            if (image != null)
            {
                selectedImage.Source = image.Source;
            }
        }

        private void UpdateData()
        {
            if (listBox != null && Composer != null)
            {
                listBox.ItemsSource = GetThumbnails();
            }
        }
    }
}