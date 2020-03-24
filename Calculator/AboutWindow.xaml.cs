﻿using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            programmersNameTextBlock.Text += "®Bucur Robert-Adrian";
            numberOfProcessorsTextBlock.Text += "💻Number of processors: " + Environment.ProcessorCount.ToString();
            osVersionTextBlock.Text += "💻OS version: " + Environment.OSVersion.ToString();
        }
        private void LinkRequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
        }
        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
