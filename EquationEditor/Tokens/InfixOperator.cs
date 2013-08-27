using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationEditor {
    class InfixOperator : IToken {
        public InfixOperator(string val, int precedence, Associativity associativity) {
            this.Type = TokenType.infixOperator;
            this.Children = 2;
            this.Value = val;
            this.Associativity = associativity;
            this.Precedence = precedence;
        }
        public int Precedence { get; set; }
        public Associativity Associativity { get; set; }
        public int Children { get; set; }
        public TokenType Type { get; set; }
        public string Value { get; set; }
    }
}
