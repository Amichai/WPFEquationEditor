using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationEditor {
    public class OperatorPrecedence {
        public OperatorPrecedence(int precedence, Associativity associativity) {
            this.Val = precedence;
            this.Associativity = associativity;
        }
        public int Val { get; set; }
        public Associativity Associativity { get; set; }
    }
    public enum Associativity { right, left };
}
