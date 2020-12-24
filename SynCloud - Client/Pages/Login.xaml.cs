﻿using System;
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
    /// Logique d'interaction pour Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
            this.InitializeLanguage();
        }

        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GetWindow(this).Content = new Home();
        }

        private void SignButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GetWindow(this).Content = new Signin();
        }
    }
}
