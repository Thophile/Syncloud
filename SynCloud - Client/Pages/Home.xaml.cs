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
using Syncloud.Model;

namespace Syncloud.Pages
{
    /// <summary>
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
            this.InitializeLanguage();
            Datagrid.ItemsSource = Controller.Instance.SyncFolders;
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GetWindow(this).Content = new Settings();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            MainWindow.GetWindow(this).Content = new Edit(button.Tag as SyncFolder);
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            Controller.Instance.SyncFolders.Remove(button.Tag as SyncFolder);
        }

        private void FetchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PushButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PullButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            SyncFolder newItem = new SyncFolder();
            Controller.Instance.SyncFolders.Add(newItem);
            MainWindow.GetWindow(this).Content = new Edit(newItem);
        }
    }
}
