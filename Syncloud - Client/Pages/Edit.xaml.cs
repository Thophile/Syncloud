using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Syncloud.Model;

namespace Syncloud.Pages
{
    /// <summary>
    /// Logique d'interaction pour Edit.xaml
    /// </summary>
    public partial class Edit : Page
    {
        private SyncFolder _syncFolder;
        public Edit(SyncFolder syncFolder)
        {
            InitializeComponent();
            this.InitializeLanguage();

            _syncFolder = syncFolder;
            PathField.Text = _syncFolder.Path;
            NameField.Text = _syncFolder.Name;
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            _syncFolder.Name = NameField.Text;
            _syncFolder.Path = PathField.Text;
            Controller.Instance.UpdateSyncFolders();
            MainWindow.GetWindow(this).Content = new Home();
        }

        private void PathBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = PathField.Text;
            dialog.ShowDialog();
            PathField.Text = dialog.SelectedPath;
        }
    }
}
