using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Number
    {
        private double number;
        public Number()
        {
            number = 0;
        }
        public double NumberProperty
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }
    }
}
