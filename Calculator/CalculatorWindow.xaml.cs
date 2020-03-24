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
using System.Text.RegularExpressions;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for CalculatorWindow.xaml
    /// </summary>
    public partial class CalculatorWindow : Window
    {
        private System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();

        String[] unaryOperation = { "sqrtButton", "squareButton", "inverseButton", "opositeButton" };
        Button pressedButton;
        private double previousNumber;
        private double currentNumber;
        private Operator operatorSymbol;
        bool operationWaitingFirst;
        bool operationWaiting;
        bool operatorPressed;
        bool equalPressed;
        bool pointActivated;

        public CalculatorWindow()
        {
            operationWaiting = false;
            operationWaitingFirst = true;
            operatorPressed = false;
            equalPressed = false;
            pointActivated = false;
            previousNumber = 0;
            currentNumber = 0;
            operatorSymbol = new Operator();
            pressedButton = new Button();
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
            notifyIcon.ShowBalloonTip(100);
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
        private void DigitButtonClick(object sender, RoutedEventArgs e)
        {
            pressedButton.Background = Brushes.Gray;
            if (equalPressed == true)
            {
                CButtonClick(sender, e);
                equalPressed = false;
            }
            if(operationWaiting == true && operationWaitingFirst == true)
            {
                displayTextBox.Clear();
                displayTextBox.Text = "0";
                operationWaitingFirst = false;

            }
            if (displayTextBox.Text != "0")
            {
                displayTextBox.Text += (sender as Button).Content.ToString();
            }
            else
            {
                displayTextBox.Text = (sender as Button).Content.ToString();
            }
            operatorPressed = false;
        }

        private void OperatorButtonClick(object sender, RoutedEventArgs e)
        {
            pointActivated = false;
            pressedButton.Background = Brushes.Gray;
            currentNumber = double.Parse(displayTextBox.Text);
            (sender as Button).Background = Brushes.White;
            pressedButton = sender as Button;

            if ((sender as Button).Name == "equalButton")
            {
                (sender as Button).Background = Brushes.Gray;
                if (operationWaiting == true && operatorPressed == false)
                {
                    try
                    {
                        previousNumber = operatorSymbol.PerformBinaryOperation(previousNumber, currentNumber);
                    }
                    catch (ArithmeticException exception)
                    {
                        displayTextBox.Text = exception.Message;
                    }
                    operatorSymbol.OperatorProperty = Operator.OperatorSymbol.None;
                    operationWaiting = false;
                    equalPressed = true;
                    displayTextBox.Text = previousNumber.ToString();
                    return;
                }
            }

            if (unaryOperation.Contains((sender as Button).Name))
            {
                if (operationWaiting == true && operatorPressed == false)
                {
                    try
                    {
                        previousNumber = operatorSymbol.PerformBinaryOperation(previousNumber, currentNumber);
                        displayTextBox.Text = previousNumber.ToString();
                    }
                    catch (ArithmeticException exception)
                    {
                        displayTextBox.FontSize = 5;
                        displayTextBox.Text = exception.Message;
                    }
                    return;
                }
                previousNumber = currentNumber;
                operatorSymbol.DetectOperator((sender as Button).Name);
                try
                {
                    previousNumber = operatorSymbol.PerformUnaryOperation(previousNumber);
                    displayTextBox.Text = previousNumber.ToString();
                }
                catch (ArithmeticException exception)
                {
                    displayTextBox.Text = exception.Message;
                }
            }

            if (operationWaiting == true && operatorPressed == false)
            {
                try
                {
                    previousNumber = operatorSymbol.PerformBinaryOperation(previousNumber, currentNumber);
                    displayTextBox.Text = previousNumber.ToString();
                }
                catch (ArithmeticException exception)
                {
                    displayTextBox.FontSize = 5;
                    displayTextBox.Text = exception.Message;
                }
                operatorSymbol.DetectOperator((sender as Button).Name);
                operationWaitingFirst = true;
            }
            else if (operatorPressed == false)
            {
                previousNumber = currentNumber;
                operatorSymbol.DetectOperator((sender as Button).Name);
                currentNumber = 0;
                operationWaiting = true;
                operationWaitingFirst = true;
            }
            else
            {
                operatorSymbol.DetectOperator((sender as Button).Name);
                currentNumber = 0;
                operationWaiting = true;
                operationWaitingFirst = true;
            }

            operatorPressed = true;
        }
        private void CEButtonClick(object sender, RoutedEventArgs e)
        {
            pressedButton.Background = Brushes.Gray;
            displayTextBox.Clear();
            displayTextBox.Text = "0";
        }
        private void CButtonClick(object sender, RoutedEventArgs e)
        {
            operationWaitingFirst = true;
            pressedButton.Background = Brushes.Gray;
            previousNumber = 0;
            operationWaiting = false;
            operatorPressed = false;
            operatorSymbol.OperatorProperty = Operator.OperatorSymbol.None;
            displayTextBox.Clear();
            displayTextBox.Text = "0";
        }
        private void CutClick(object sender, RoutedEventArgs e)
        {
            pressedButton.Background = Brushes.Gray;
            displayTextBox.Cut();
            displayTextBox.Text = "0";
            pointActivated = false;
        }
        private void CopyClick(object sender, RoutedEventArgs e)
        {
            displayTextBox.Copy();
        }
        private void PasteClick(object sender, RoutedEventArgs e)
        {
            displayTextBox.Clear();
            Regex regex = new Regex("[0-9]+");
            if (regex.IsMatch(Clipboard.GetText()) == true)
            {
                displayTextBox.Paste();
                if (displayTextBox.Text.Contains(",") == true)
                {
                    pointActivated = true;
                }
            }
            else
            {
                displayTextBox.Text = "0";
            }
        }
        private void pointButtonClick(object sender, RoutedEventArgs e)
        {
            pressedButton.Background = Brushes.Gray;
            if (pointActivated == false)
            {
                displayTextBox.Text += ",";
                pointActivated = true;
            }
        }
        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            double val;
            if (double.TryParse(displayTextBox.Text, out val) == false)
            {
                displayTextBox.Text = "0";
            }
            if (displayTextBox.Text.Length > 1)
            {
                displayTextBox.Text = displayTextBox.Text.Substring(0, displayTextBox.Text.Length - 1);
            }
            else
            {
                displayTextBox.Text = "0";
            }
        }
    }
}

