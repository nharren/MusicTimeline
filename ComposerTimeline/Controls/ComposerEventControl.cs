using Database;
using NathanHarrenstein.Timeline;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.ComposerTimeline.Controls
{
    public class ComposerEventControl : EventControl
    {
        public static readonly DependencyProperty PlayPopularCommandProperty = DependencyProperty.Register("PlayPopularCommand", typeof(ICommand), typeof(ComposerEventControl));

        static ComposerEventControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComposerEventControl), new FrameworkPropertyMetadata(typeof(ComposerEventControl)));
        }

        public Composer Composer { get; set; }

        public string Born { get; set; }

        public string Died { get; set; }

        public IEnumerable<Flag> Flags { get; set; }

        public BitmapImage Image { get; set; }

        public ICommand PlayPopularCommand
        {
            get { return (ICommand)GetValue(PlayPopularCommandProperty); }
            set { SetValue(PlayPopularCommandProperty, value); }
        }

        internal static BitmapImage DefaultImage { get; set; }
    }
}