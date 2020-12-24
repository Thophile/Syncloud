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
    /// Logique d'interaction pour Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        Model.Settings settings;
        public Settings()
        {
            InitializeComponent();
            this.InitializeLanguage();

            settings = Controller.Instance.AppSettings;
            LanguageField.ItemsSource = Enum.GetValues(typeof(Model.Settings.Languages));

            LanguageField.SelectedItem = settings.Language;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            //Save settings
            Model.Settings settings = Controller.Instance.AppSettings;

            settings.Language = (Model.Settings.Languages)LanguageField.SelectedItem;

            Controller.Instance.AppSettings = settings;
            Controller.Instance.UpdateSettings();

            MainWindow.GetWindow(this).Content = new Home();
        }
    }
}
