using NathanHarrenstein.ComposerTimeline.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.ComposerTimeline.Controls
{
    public class ComposerControl : Control
    {
        static ComposerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComposerControl), new FrameworkPropertyMetadata(typeof(ComposerControl)));
        }

        public string Born { get; set; }

        public ICommand Click { get; set; }

        public ComposerEvent ComposerEvent { get; set; }

        public string Died { get; set; }

        public string Era { get; set; }

        public FlagData Flag { get; set; }

        public BitmapImage Image { get; set; }

        public string SortTitle { get; set; }

        public string Title { get; set; }

        internal static BitmapImage DefaultImage { get; set; }

        public ICommand PlayPopularCommand
        {
            get { return (ICommand)GetValue(PlayPopularCommandProperty); }
            set { SetValue(PlayPopularCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayPopularCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayPopularCommandProperty =
            DependencyProperty.Register("PlayPopularCommand", typeof(ICommand), typeof(ComposerControl));
    }
}