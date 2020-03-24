using System;
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
    /// Interaction logic for CalculatorWindow.xaml
    /// </summary>
    public partial class CalculatorWindow : Window
    {
        System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
        public CalculatorWindow()
        {
            InitializeComponent();
        }
        private void WindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void RestoreWindowDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            notifyIcon.Visible = false;
            this.ShowInTaskbar = true;
            SystemCommands.RestoreWindow(this);
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private void MinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
            notifyIcon.Icon = new System.Drawing.Icon("Calculator.ico");
            notifyIcon.Visible = true;
            notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(RestoreWindowDoubleClick);
            this.ShowInTaskbar = false;
            notifyIcon.BalloonTipText = "The application is now down in the System Tray!";
            notifyIcon.ShowBalloonTip(500);
        }
        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void AboutClick(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.Show();
        }
    }
}
