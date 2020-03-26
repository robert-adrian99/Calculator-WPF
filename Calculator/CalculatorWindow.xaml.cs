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
using System.Globalization;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for CalculatorWindow.xaml
    /// </summary>
    public partial class CalculatorWindow : Window
    {
        private System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
        private String[] unaryOperation = { "sqrtButton", "squareButton", "inverseButton", "opositeButton" };
        private Button pressedButton;
        private double previousNumber;
        private double currentNumber;
        private Operator operatorSymbol;
        private bool operationWaitingFirst;
        private bool operationWaiting;
        private bool operatorPressed;
        private bool equalPressed;
        private bool pointActivated;

        private Stack<double> memoryStack;

        public CalculatorWindow()
        {
            memoryStack = new Stack<double>();
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
            pointButton.Content = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
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
        private void CutClick(object sender, RoutedEventArgs e)
        {
            displayTextBox.SelectAll();
            displayTextBox.Cut();
            pressedButton.Background = Brushes.Gray;
            displayTextBox.Text = "0";
            pointActivated = false;
        }
        private void CopyClick(object sender, RoutedEventArgs e)
        {
            displayTextBox.SelectAll();
            displayTextBox.Copy();
        }
        private void PasteClick(object sender, RoutedEventArgs e)
        {
            displayTextBox.Clear();
            Regex regex = new Regex("^[0-9]+$");
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
        private void AboutClick(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.Show();
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
        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            double val;
            if (double.TryParse(displayTextBox.Text, out val) == false || displayTextBox.Text.Length <= 1 || equalPressed == true)
            {
                displayTextBox.Text = "0";
            }
            else if (displayTextBox.Text.Length > 1)
            {
                displayTextBox.Text = displayTextBox.Text.Substring(0, displayTextBox.Text.Length - 1);
            }
        }
        private void PointButtonClick(object sender, RoutedEventArgs e)
        {
            pressedButton.Background = Brushes.Gray;
            if (pointActivated == false)
            {
                displayTextBox.Text += CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                pointActivated = true;
            }
        }
        private void DigitButtonClick(object sender, RoutedEventArgs e)
        {
            pressedButton.Background = Brushes.Gray;
            if (equalPressed == true)
            {
                CButtonClick(sender, e);
                equalPressed = false;
            }
            if (operationWaiting == true && operationWaitingFirst == true)
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
                (sender as Button).Background = Brushes.Gray;
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
            currentNumber = double.Parse(displayTextBox.Text);
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
        private void KeyboardKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D0 || e.Key == Key.D1 || e.Key == Key.D2 || e.Key == Key.D3 || e.Key == Key.D4 || e.Key == Key.D5 || e.Key == Key.D6 || e.Key == Key.D7 || e.Key == Key.D8 || e.Key == Key.D9 || e.Key == Key.NumPad0 || e.Key == Key.NumPad1 || e.Key == Key.NumPad2 || e.Key == Key.NumPad3 || e.Key == Key.NumPad4 || e.Key == Key.NumPad5 || e.Key == Key.NumPad6 || e.Key == Key.NumPad7 || e.Key == Key.NumPad8 || e.Key == Key.NumPad9)
            {
                pressedButton.Background = Brushes.Gray;
                if (equalPressed == true)
                {
                    CButtonClick(sender, e);
                    equalPressed = false;
                }
                if (operationWaiting == true && operationWaitingFirst == true)
                {
                    displayTextBox.Clear();
                    displayTextBox.Text = "0";
                    operationWaitingFirst = false;
                }
                if (displayTextBox.Text != "0")
                {
                    displayTextBox.Text += e.Key.ToString()[e.Key.ToString().Length - 1].ToString();
                }
                else
                {
                    displayTextBox.Text = e.Key.ToString()[e.Key.ToString().Length - 1].ToString();
                }
                operatorPressed = false;
            }
            else if (e.Key == Key.Add || e.Key == Key.Subtract || e.Key == Key.Multiply || e.Key == Key.Divide || e.Key == Key.Return || e.Key == Key.Enter)
            {
                pointActivated = false;
                pressedButton.Background = Brushes.Gray;
                currentNumber = double.Parse(displayTextBox.Text);

                if (e.Key == Key.Return || e.Key == Key.Enter)
                {
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
                else
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
                        switch(e.Key)
                        {
                            case Key.Add:
                                plusButton.Background = Brushes.White;
                                pressedButton = plusButton;
                                operatorSymbol.DetectOperator("plusButton");
                                break;
                            case Key.Subtract:
                                minusButton.Background = Brushes.White;
                                pressedButton = minusButton;
                                operatorSymbol.DetectOperator("minusButton");
                                break;
                            case Key.Multiply:
                                multiplyButton.Background = Brushes.White;
                                pressedButton = multiplyButton;
                                operatorSymbol.DetectOperator("multiplyButton");
                                break;
                            case Key.Divide:
                                divideButton.Background = Brushes.White;
                                pressedButton = divideButton;
                                operatorSymbol.DetectOperator("divideButton");
                                break;
                        }
                        operationWaitingFirst = true;
                    }
                    else if (operatorPressed == false)
                    {
                        previousNumber = currentNumber;
                        switch (e.Key)
                        {
                            case Key.Add:
                                plusButton.Background = Brushes.White;
                                pressedButton = plusButton;
                                operatorSymbol.DetectOperator("plusButton");
                                break;
                            case Key.Subtract:
                                minusButton.Background = Brushes.White;
                                pressedButton = minusButton;
                                operatorSymbol.DetectOperator("minusButton");
                                break;
                            case Key.Multiply:
                                multiplyButton.Background = Brushes.White;
                                pressedButton = multiplyButton;
                                operatorSymbol.DetectOperator("multiplyButton");
                                break;
                            case Key.Divide:
                                divideButton.Background = Brushes.White;
                                pressedButton = divideButton;
                                operatorSymbol.DetectOperator("divideButton");
                                break;
                        }
                        currentNumber = 0;
                        operationWaiting = true;
                        operationWaitingFirst = true;
                    }
                    else
                    {
                        switch (e.Key)
                        {
                            case Key.Add:
                                plusButton.Background = Brushes.White;
                                pressedButton = plusButton;
                                operatorSymbol.DetectOperator("plusButton");
                                break;
                            case Key.Subtract:
                                minusButton.Background = Brushes.White;
                                pressedButton = minusButton;
                                operatorSymbol.DetectOperator("minusButton");
                                break;
                            case Key.Multiply:
                                multiplyButton.Background = Brushes.White;
                                pressedButton = multiplyButton;
                                operatorSymbol.DetectOperator("multiplyButton");
                                break;
                            case Key.Divide:
                                divideButton.Background = Brushes.White;
                                pressedButton = divideButton;
                                operatorSymbol.DetectOperator("divideButton");
                                break;
                        }
                        currentNumber = 0;
                        operationWaiting = true;
                        operationWaitingFirst = true;
                    }
                }
                operatorPressed = true;
            }
            else if(e.Key == Key.Escape)
            {
                CButtonClick(sender, e);
            }
        }
        private void MCButtonClick(object sender, RoutedEventArgs e)
        {
            memoryStack.Clear();
        }
        private void MRButtonClick(object sender, RoutedEventArgs e)
        {
            displayTextBox.Text = memoryStack.Peek().ToString();
        }
        private void MplusButtonClick(object sender, RoutedEventArgs e)
        {
            double number = memoryStack.Peek();
            memoryStack.Pop();
            memoryStack.Push(number + double.Parse(displayTextBox.Text));
        }
        private void MminusButtonClick(object sender, RoutedEventArgs e)
        {
            double number = memoryStack.Peek();
            memoryStack.Pop();
            memoryStack.Push(number - double.Parse(displayTextBox.Text));
        }
        private void MSButtonClick(object sender, RoutedEventArgs e)
        {
            memoryStack.Push(double.Parse(displayTextBox.Text));
            displayTextBox.Text = "0";
        }
        private void PercentageButtonClick(object sender, RoutedEventArgs e)
        {
            if (operationWaiting == true)
            {
                double percentage = double.Parse(displayTextBox.Text);
                percentage /= 100;
                currentNumber = previousNumber * percentage;
                displayTextBox.Text = currentNumber.ToString();
            }
            else
            {
                previousNumber = 0;
                operationWaiting = false;
                operationWaitingFirst = false;
                operatorPressed = false;
                pointActivated = false;
                equalPressed = false;
                pressedButton.Background = Brushes.Gray;
                operatorSymbol.OperatorProperty = Operator.OperatorSymbol.None;
                displayTextBox.Text = "0";
            }
        }
    }
}

