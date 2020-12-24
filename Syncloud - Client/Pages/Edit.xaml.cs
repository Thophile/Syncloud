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
    /// Logique d'interaction pour Edit.xaml
    /// </summary>
    public partial class Edit : Page
    {
        public Edit(SyncFolder syncFolder)
        {
            InitializeComponent();
            this.InitializeLanguage();
        }
    }
}
