using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationEditor.Tokens {
    class Number : IToken {
        public Number(string asString, double numericalVal) {
            this.Value = asString;
            this.numericalVal = numericalVal;
            this.Type = TokenType.number;
        }

        public double numericalVal { get; private set; }
        public int NumberOfChildren { get; set; }

        public TokenType Type { get; set; }

        public string Value { get; set; }
    }
}
