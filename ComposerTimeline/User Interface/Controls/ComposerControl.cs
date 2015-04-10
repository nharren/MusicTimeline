using NathanHarrenstein.ComposerTimeline.Data;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.ComposerTimeline.Controls
{
    public class ComposerControl : Control
    {
        public static readonly DependencyProperty PlayPopularCommandProperty = DependencyProperty.Register("PlayPopularCommand", typeof(ICommand), typeof(ComposerControl));

        static ComposerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComposerControl), new FrameworkPropertyMetadata(typeof(ComposerControl)));
        }

        public string Born { get; set; }

        public ICommand Click { get; set; }

        public ComposerEvent ComposerEvent { get; set; }

        public string Died { get; set; }

        public IEnumerable<string> Eras { get; set; }

        public IEnumerable<Flag> Flags { get; set; }

        public BitmapImage Image { get; set; }

        public ICommand PlayPopularCommand
        {
            get { return (ICommand)GetValue(PlayPopularCommandProperty); }
            set { SetValue(PlayPopularCommandProperty, value); }
        }

        public string SortTitle { get; set; }

        public string Title { get; set; }

        internal static BitmapImage DefaultImage { get; set; }
    }
}