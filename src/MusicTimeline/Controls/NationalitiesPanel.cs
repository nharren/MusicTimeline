using NathanHarrenstein.MusicTimeline.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class NationalitiesPanel : Control
    {
        public static readonly DependencyProperty ComposerProperty = DependencyProperty.Register("Composer", typeof(Composer), typeof(NationalitiesPanel), new PropertyMetadata(null, new PropertyChangedCallback(Composer_Changed)));
        public static readonly DependencyProperty FlagSeparationProperty = DependencyProperty.Register("FlagSeparation", typeof(double), typeof(NationalitiesPanel), new PropertyMetadata(0.0, new PropertyChangedCallback(FlagSeparation_Changed)));
        public static readonly DependencyProperty UseLargeFlagsProperty = DependencyProperty.Register("UseLargeFlags", typeof(bool), typeof(NationalitiesPanel), new PropertyMetadata(false, new PropertyChangedCallback(UseLargeFlags_Changed)));

        private ItemsControl itemsControl;

        static NationalitiesPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NationalitiesPanel), new FrameworkPropertyMetadata(typeof(NationalitiesPanel)));
        }

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

        public double FlagSeparation
        {
            get
            {
                return (double)GetValue(FlagSeparationProperty);
            }

            set
            {
                SetValue(FlagSeparationProperty, value);
            }
        }

        public bool UseLargeFlags
        {
            get
            {
                return (bool)GetValue(UseLargeFlagsProperty);
            }

            set
            {
                SetValue(UseLargeFlagsProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            itemsControl = GetTemplateChild("itemsControl") as ItemsControl;

            UpdateData();
        }

        private static void Composer_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var nationalitiesPanel = (NationalitiesPanel)d;
            nationalitiesPanel.UpdateData();
        }

        private static void FlagSeparation_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var nationalitiesPanel = (NationalitiesPanel)d;
            nationalitiesPanel.UpdateData();
        }

        private static void UseLargeFlags_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var nationalitiesPanel = (NationalitiesPanel)d;
            nationalitiesPanel.UpdateData();
        }

        public void UpdateData()
        {
            if (itemsControl != null && Composer != null)
            {
                itemsControl.ItemsSource = CreateFlags();
            }
        }

        private IEnumerable<Image> CreateFlags()
        {
            if (Composer == null)
            {
                yield break;
            }

            foreach (var nationality in Composer.Nationalities)
            {
                var nationalityName = string.IsNullOrWhiteSpace(nationality.Name) ? "Unknown" : nationality.Name;
                var size = UseLargeFlags ? "32" : "16";
                var uri = new Uri($@"pack://application:,,,/Resources/Flags/{size}/{nationalityName}.png", UriKind.Absolute);
                var bitmapImage = new BitmapImage(uri);
                var image = new Image();
                image.DataContext = nationality;
                image.Source = bitmapImage;
                image.Width = int.Parse(size);
                image.ToolTip = nationalityName;
                image.Margin = new Thickness(FlagSeparation, 0, 0, 0);

                yield return image;
            }
        }
    }
}