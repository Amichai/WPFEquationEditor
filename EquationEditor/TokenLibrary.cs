using EquationEditor.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationEditor {
    class TokenLibrary {

        private static Dictionary<string, IToken> tokens = new Dictionary<string, IToken>() {
            { "^", new InfixOperator("^", 4, Associativity.right ) }, 
            { "**", new InfixOperator("**", 4, Associativity.right ) }, 
            { "*", new InfixOperator("*", 3, Associativity.left ) }, 
            { "/", new InfixOperator("/", 3, Associativity.left ) }, 
            { @"\over", new InfixOperator(@"\over", 3, Associativity.left ) }, 
            { "-", new InfixOperator("-", 2, Associativity.left ) }, 
            { "+", new InfixOperator("+", 2, Associativity.left ) }, 
            { "=", new InfixOperator("=", 1, Associativity.left ) },
            { ",", new BreakingChar(",")},
            { ";", new BreakingChar(";")},
            { "(", new BreakingChar("(")},
            { ")", new BreakingChar(")")},
            { "add", new Function("add")},
            { "sum", new Function("sum")},
            { "Line", new Function("Line")},
            { "Rectangle", new Function("Rectangle")},
            { "Ellipse", new Function("Ellipse")},
            { "Negate", new Function("Negate")},

        };

        public static IToken Create(string t) {
            //Check if it is a number
            if (tokens.ContainsKey(t)) {
                return tokens[t].Clone();
            } else {
                double result;
                if (double.TryParse(t, out result)) {
                    return new Number(t, result);
                } else {
                    return new Keyword(t);
                }
            }
        }
    }
}
