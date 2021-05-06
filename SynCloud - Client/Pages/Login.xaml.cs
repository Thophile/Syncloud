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
    /// Logique d'interaction pour Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
            this.InitializeLanguage();
        }

        private async void LogButton_Click(object sender, RoutedEventArgs e)
        {
            if(await Controller.Instance.ApiClient.Login(UsernameField.Text, PasswordField.Password))
            {
                    Window w = MainWindow.GetWindow(this);
                    if (w != null) w.Content = new Home();
            }
        }

        private void SignButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GetWindow(this).Content = new Signin();
        }
    }
}
