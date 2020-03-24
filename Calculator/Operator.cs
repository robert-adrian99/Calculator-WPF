using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Operator
    {
        public enum OperatorSymbol
        {
            None,
            Equal,
            Plus,
            Minus,
            Multiply,
            Divide,
            SquareRoot,
            Square,
            Inverse,
            Percentage,
            Oposite
        }
        private OperatorSymbol operatorSymbol;
        public Operator()
        {
            operatorSymbol = OperatorSymbol.None;
        }
        public OperatorSymbol OperatorProperty
        {
            get
            {
                return operatorSymbol;
            }
            set
            {
                operatorSymbol = value;
            }
        }
        public double PerformBinaryOperation(double previousNumber, double currentNumber)
        {
            switch (operatorSymbol)
            {
                case OperatorSymbol.Plus:
                    return previousNumber + currentNumber;
                case OperatorSymbol.Minus:
                    return previousNumber - currentNumber;
                case OperatorSymbol.Multiply:
                    return previousNumber * currentNumber;
                case OperatorSymbol.Divide:
                    if (currentNumber == 0)
                    {
                        throw new ArithmeticException("Cannot divide by zero‬");
                    }
                    return previousNumber / currentNumber;
                default:
                    return 0;
            }
        }
        public double PerformUnaryOperation(double number)
        {
            switch(operatorSymbol)
            {
                case OperatorSymbol.Oposite:
                    return -number;
                case OperatorSymbol.Inverse:
                    if (number == 0)
                    {
                        throw new ArithmeticException("Cannot divide by zero‬");
                    }
                    return 1 / number;
                case OperatorSymbol.Square:
                    return number * number;
                case OperatorSymbol.SquareRoot:
                    if (number < 0)
                    {
                        throw new ArithmeticException("Invalid input‬");
                    }
                    return Math.Sqrt(number);
                default:
                    return 0;
            }
        }
        public void DetectOperator(string operatorString)
        {
            switch(operatorString)
            {
                case "plusButton":
                    operatorSymbol = OperatorSymbol.Plus;
                    break;
                case "minusButton":
                    operatorSymbol = OperatorSymbol.Minus;
                    break;
                case "multiplyButton":
                    operatorSymbol = OperatorSymbol.Multiply;
                    break;
                case "divideButton":
                    operatorSymbol = OperatorSymbol.Divide;
                    break;
                case "sqrtButton":
                    operatorSymbol = OperatorSymbol.SquareRoot;
                    break;
                case "squareButton":
                    operatorSymbol = OperatorSymbol.Square;
                    break;
                case "inverseButton":
                    operatorSymbol = OperatorSymbol.Inverse;
                    break;
                case "opositeButton":
                    operatorSymbol = OperatorSymbol.Oposite;
                    break;
                case "percentageButton":
                    operatorSymbol = OperatorSymbol.Percentage;
                    break;
                default:
                    operatorSymbol = OperatorSymbol.None;
                    break;
            }
        }
    }
}
