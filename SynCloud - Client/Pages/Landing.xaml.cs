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

            //Set language
            ResourceDictionary dict = new ResourceDictionary();
            switch (Controller.Instance.AppSettings.Language)
            {
                case Settings.Languages.English:
                    dict.Source = new Uri("..\\ResourceDictionary\\Translations.en.xaml",
                                  UriKind.Relative);
                    break;
                case Settings.Languages.Français:
                    dict.Source = new Uri("..\\ResourceDictionary\\Translations.fr.xaml",
                                       UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
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
