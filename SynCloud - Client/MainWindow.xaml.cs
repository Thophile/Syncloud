using SynCloud.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SynCloud
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon _icon;
        public MainWindow()
        {
            InitializeComponent();
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                MessageBox.Show("SynCloud is already running");
                Process.GetCurrentProcess().Kill();
            }
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

            this.Content = new Login();

            // Minimize to tray parameters
            _icon = new System.Windows.Forms.NotifyIcon();
            _icon.BalloonTipText = this.Resources["minimized"] as string;
            _icon.BalloonTipTitle = "SynCloud";
            _icon.Text = "SynCloud";
            _icon.Icon = new System.Drawing.Icon("SynCloud.ico");
            _icon.Click += new EventHandler(_icon_Click);

            


        }
        void OnClose(object sender, CancelEventArgs args)
        {
            _icon.Dispose();
            _icon = null;
        }

        private WindowState _windowState = WindowState.Normal;
        void OnStateChanged(object sender, EventArgs args)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                if (_icon != null)
                    _icon.ShowBalloonTip(2000);
            }
            else
                _windowState = WindowState;
        }
        void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            CheckTrayIcon();
        }

        void _icon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = _windowState;
        }
        void CheckTrayIcon()
        {
            ShowTrayIcon(!IsVisible);
        }

        void ShowTrayIcon(bool show)
        {
            if (_icon != null)
                _icon.Visible = show;
        }
    }
}
