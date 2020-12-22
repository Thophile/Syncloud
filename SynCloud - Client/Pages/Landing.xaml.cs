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

namespace SynCloud.Pages
{
    /// <summary>
    /// Logique d'interaction pour Landing.xaml
    /// </summary>
    public partial class Landing : Page
    {
        public Landing()
        {
            InitializeComponent();
        }

        public void Home()
        {
            MainWindow.GetWindow(this).Content = new Home();
        }

        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameField.Text;
            var password = PasswordField.Password;
            //if authentication succeed
            Home();
        }
    }
}
