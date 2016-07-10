using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class LoginDialog : Window
    {
        private string _password;
        private string _username;

        public LoginDialog()
        {
            InitializeComponent();
        }

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
            }
        }

        public string UserName
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _password = PasswordBox.Password;
        }

        private void UsernameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _username = UsernameBox.Text;
        }
    }
}