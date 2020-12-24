using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Syncloud
{
    public static class Extensions
    {
        public static void InitializeLanguage(this Page p)
        {
            //Set language
            ResourceDictionary dict = new ResourceDictionary();
            switch (Controller.Instance.AppSettings.Language)
            {
                case Syncloud.Settings.Languages.English:
                    dict.Source = new Uri("..\\ResourceDictionary\\Translations.en.xaml",
                                  UriKind.Relative);
                    break;
                case Syncloud.Settings.Languages.Français:
                    dict.Source = new Uri("..\\ResourceDictionary\\Translations.fr.xaml",
                                       UriKind.Relative);
                    break;
            }
            p.Resources.MergedDictionaries.Add(dict);
        }
    }
    
}
