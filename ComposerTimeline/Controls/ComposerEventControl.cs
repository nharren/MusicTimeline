using Database;
using NathanHarrenstein.Timeline;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.ComposerTimeline.Controls
{
    public class ComposerEventControl : EventControl
    {
        public static readonly DependencyProperty FlagsProperty = DependencyProperty.Register("Flags", typeof(List<Flag>), typeof(ComposerEventControl));

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(BitmapImage), typeof(ComposerEventControl));

        public static readonly DependencyProperty PlayPopularCommandProperty = DependencyProperty.Register("PlayPopularCommand", typeof(ICommand), typeof(ComposerEventControl));

        static ComposerEventControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComposerEventControl), new FrameworkPropertyMetadata(typeof(ComposerEventControl)));
        }

        public string Born { get; set; }

        public Composer Composer { get; set; }

        public string Died { get; set; }

        public List<Flag> Flags
        {
            get
            {
                return (List<Flag>)GetValue(FlagsProperty);
            }
            set
            {
                SetValue(FlagsProperty, value);
            }
        }
        public BitmapImage Image
        {
            get
            {
                return (BitmapImage)GetValue(ImageProperty);
            }
            set
            {
                SetValue(ImageProperty, value);
            }
        }
        public ICommand PlayPopularCommand
        {
            get
            {
                return (ICommand)GetValue(PlayPopularCommandProperty);
            }
            set
            {
                SetValue(PlayPopularCommandProperty, value);
            }
        }
    }
}