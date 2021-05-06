using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading.Tasks;
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
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var syncFolder = button.Tag as SyncFolder;
            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show(this.Resources["delete_warning"].ToString(), this.Resources["warning"].ToString(), System.Windows.Forms.MessageBoxButtons.YesNo);
            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                if(await Controller.Instance.ApiClient.DeleteFolder(syncFolder))
                {
                    Controller.Instance.SyncFolders.Remove(syncFolder);
                    Controller.Instance.UpdateSyncFolders();
                }
                else
                {
                    MessageBox.Show(this.Resources["resquest_failed"].ToString());
                }
            }
        }

        private async void FetchButton_Click(object sender, RoutedEventArgs e)
        {
            if(!await Controller.Instance.ApiClient.GetList()) MessageBox.Show(this.Resources["resquest_failed"].ToString());
        }

        private async void PushButton_Click(object sender, RoutedEventArgs e)
        {   
            var syncFolder = Datagrid.SelectedItem as SyncFolder;
            if (syncFolder.Path != null)
            {
                if(!await Controller.Instance.ApiClient.SendFolder(syncFolder)) MessageBox.Show(this.Resources["resquest_failed"].ToString()); 
            }
            else
            {
                MessageBox.Show(this.Resources["path_empty_error"].ToString());
            }
        }

        private async void PullButton_Click(object sender, RoutedEventArgs e)
        {
            var syncFolder = Datagrid.SelectedItem as SyncFolder;

            if (syncFolder != null && syncFolder.Path != null)
            {
                if (!Directory.Exists(syncFolder.Path)) Directory.CreateDirectory(syncFolder.Path);

                if(!await Controller.Instance.ApiClient.GetFolder(syncFolder)) MessageBox.Show(this.Resources["request_failed"].ToString());
            }
            else
            {
                MessageBox.Show(this.Resources["path_empty_error"].ToString());
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            SyncFolder newItem = new SyncFolder();
            Controller.Instance.SyncFolders.Add(newItem);
            MainWindow.GetWindow(this).Content = new Edit(newItem);
        }
    }
}
