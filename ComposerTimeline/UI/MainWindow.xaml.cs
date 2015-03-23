using NathanHarrenstein.ComposerTimeline.UI.Initializers;
using System.Windows;
using System.Windows.Input;

namespace NathanHarrenstein.ComposerTimeline.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty AddFilesCommandProperty = DependencyProperty.Register("AddFilesCommand", typeof(ICommand), typeof(MainWindow));
        public static readonly DependencyProperty AddFolderCommandProperty = DependencyProperty.Register("AddFolderCommand", typeof(ICommand), typeof(MainWindow));
        public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();

            MainWindowInitializer.Initialize(this); // Called after InitializeComponent so Frame is visible for navigation.
        }

        public ICommand AddFilesCommand
        {
            get { return (ICommand)GetValue(AddFilesCommandProperty); }
            set { SetValue(AddFilesCommandProperty, value); }
        }

        public ICommand AddFolderCommand
        {
            get { return (ICommand)GetValue(AddFolderCommandProperty); }
            set { SetValue(AddFolderCommandProperty, value); }
        }

        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }
    }
}