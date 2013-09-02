using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EquationEditor.Tokens {
    class InfixOperator : IToken, IFunction {
        public InfixOperator(string val, int precedence, Associativity associativity) {
            this.Type = TokenType.infixOperator;
            this.NumberOfChildren = 2;
            this.Value = val;
            this.Associativity = associativity;
            this.Precedence = precedence;
        }
        public int Precedence { get; set; }
        public Associativity Associativity { get; set; }
        public int NumberOfChildren { get; set; }
        public TokenType Type { get; set; }
        public string Value { get; set; }

        public IToken Clone() {
            return new InfixOperator(Value, this.Precedence, this.Associativity);
        }
    }
}
