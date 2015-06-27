using System.Windows;
using System.Windows.Input;

namespace NathanHarrenstein.ComposerTimeline
{
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty ManageDataCommandProperty = DependencyProperty.Register("ManageDataCommand", typeof(ICommand), typeof(MainWindow));
        public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();

            MainWindowInitializer.Initialize(this);
        }

        public ICommand ManageDataCommand
        {
            get
            {
                return (ICommand)GetValue(ManageDataCommandProperty);
            }

            set
            {
                SetValue(ManageDataCommandProperty, value);
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                return (ICommand)GetValue(CloseCommandProperty);
            }

            set
            {
                SetValue(CloseCommandProperty, value);
            }
        }
    }
}