using System;
using System.Windows;
using System.Windows.Controls;

namespace SynCloud.Pages
{
    /// <summary>
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
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
    }
}
