using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Syncloud.Pages
{
    /// <summary>
    /// Logique d'interaction pour Signin.xaml
    /// </summary>
    public partial class Signin : Page
    {
        public Signin()
        {
            InitializeComponent();
            this.InitializeLanguage();
        }

        private async void SignButton_Click(object sender, RoutedEventArgs e)
        {
            if(PasswordField.Password == ConfirmPasswordField.Password && await Controller.Instance.ApiClient.Register(UsernameField.Text, MailField.Text, PasswordField.Password))
            {
                MainWindow.GetWindow(this).Content = new Login();
            }
        }

        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GetWindow(this).Content = new Login();
        }
    }
}
