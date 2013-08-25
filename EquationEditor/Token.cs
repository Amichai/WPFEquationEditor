using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationEditor {
    public class Token {
        public string Value { get; set; }
        public TokenType Type { get; set; }
        public static Token Create(string val) {
            Token toReturn = new Token();
            toReturn.Children = 0;
            toReturn.Value = val;
            if (Types.ContainsKey(val)) {
                toReturn.Type = Types[val];
                if (operatorPrecedence.ContainsKey(val)) {
                    toReturn.Children = 2;
                }
            } else {
                double result;
                if (double.TryParse(val, out result)) {
                    toReturn.Type = TokenType.number;
                } else {
                    toReturn.Type = TokenType.stringLiteral;
                }

            }
            return toReturn;
        }
        public int Children { get; set; }
        public OperatorPrecedence Precedence{ 
            get {
            return operatorPrecedence[this.Value];
            }
        }


        private static Dictionary<string, TokenType> Types = new Dictionary<string, TokenType>(){
            { @"\over", TokenType.infixOperator },
            { ",", TokenType.seperator },
            { "(", TokenType.leftParen},
            { ")", TokenType.rightParen },
            { "add", TokenType.function },
            { "+", TokenType.infixOperator},
            { "-", TokenType.infixOperator},
            { "^", TokenType.infixOperator},
            { "*", TokenType.infixOperator},
            { "/", TokenType.infixOperator},
            { "=", TokenType.infixOperator},
        };

        private static Dictionary<string, OperatorPrecedence> operatorPrecedence = new  Dictionary<string,OperatorPrecedence>(){ 
        {"^", new OperatorPrecedence(4, Associativity.right)}, 
        {"*", new OperatorPrecedence(3, Associativity.left)}, 
        {"/", new OperatorPrecedence(3, Associativity.left)}, 
        {"+", new OperatorPrecedence(2, Associativity.left)}, 
        {"-", new OperatorPrecedence(2, Associativity.left)}, 
        {"=", new OperatorPrecedence(1, Associativity.left)}, 

        };
    }
    public enum TokenType { number, symbol, infixOperator, stringLiteral, seperator, leftParen, rightParen, function};
}
