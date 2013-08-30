using EquationEditor.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationEditor {
    class Tokenizer {
        private static List<string> breakingChars = new List<string> { " ", ",", ")", "(", "*", "/", "^", "+", "-", "=" };

        private List<string> splitString(string input, string c) {
            List<string> result = new List<string>();
            int index = input.IndexOf(c);
            while (index != -1) {
                if (index > 0) {
                    result.Add(input.Substring(0, index));
                }
                if (c != " ") {
                    result.Add(c.ToString());
                }
                input = string.Concat(input.Skip(index + c.Count()));
                index = input.IndexOf(c);
            }
            if (input != "") {
                result.Add(input);
            }
            return result;
        }

        private List<string> breakOnChar(List<string> input, string c) {
            List<string> outputStrings = new List<string>();
            foreach (var s in input) {
                var split = splitString(s, c);
                for (int i = 0; i < split.Count(); i++) {
                    var toAdd = split[i];
                    outputStrings.Add(toAdd);
                }
            }
            return outputStrings;
        }

        public Queue<IToken> Tokenize(string text) {
            List<string> tokenStrings = new List<string>() { text };
            foreach (string c in breakingChars) {
                tokenStrings = breakOnChar(tokenStrings, c);
            }
            var tokens = tokenStrings.Select(i => TokenLibrary.Create(i)).ToList();

            return ConvertToPostfixed(tokens);
        }

        private static Queue<IToken> ConvertToPostfixed(List<IToken> tokens) {
            Queue<IToken> postFixed = new Queue<IToken>();
            Stack<IToken> operatorStack = new Stack<IToken>();
            foreach (var token in tokens) { 
                switch (token.Type) {
                    case TokenType.number:
                         postFixed.Enqueue(token);
                        break;
                    case TokenType.function:
                        operatorStack.Push(token);
                        break;
                    case TokenType.seperator:
                        if (token.Value == ",") {
                            if (operatorStack.Count() == 0) {
                                throw new Exception("Comma and no arg");
                            }
                            while (operatorStack.Peek().Value != "(") {
                                postFixed.Enqueue(operatorStack.Pop());
                            }
                        } else if (token.Value == "(") {
                            operatorStack.Push(token);
                            break;
                        } else if (token.Value == ")") {
                            if (operatorStack.Count() == 0) {
                                throw new Exception("Mismatched parenthesis");
                            }
                            while (operatorStack.Peek().Value != "(") {
                                postFixed.Enqueue(operatorStack.Pop());
                            }
                            operatorStack.Pop();
                        }
                        break;
                    case TokenType.infixOperator:
                        var asInfix = token as InfixOperator;
                        while (operatorStack.Count() > 0 ){
                            if (operatorStack.Peek() is InfixOperator) {
                                var toPopAndAdd = operatorStack.Peek() as InfixOperator;
                                if ((asInfix.Associativity == Associativity.left && asInfix.Precedence == toPopAndAdd.Precedence)
                                    || asInfix.Precedence < toPopAndAdd.Precedence) {
                                    postFixed.Enqueue(operatorStack.Pop());
                                } else {
                                    break;
                                }
                            } else if (operatorStack.Peek() is IFunction) {
                                postFixed.Enqueue(operatorStack.Pop());
                            } else {
                                break;
                            }
                        }
                        operatorStack.Push(token);
                        break;
                    case TokenType.keyword:
                        postFixed.Enqueue(token);
                        break;
                    default:
                        throw new Exception();
                }
            }
            while (operatorStack.Count() > 0) {
                postFixed.Enqueue(operatorStack.Pop());
            }
            if (postFixed.Where(i => i.Value == " ").Count() > 0) {
                throw new Exception();
            }

            return postFixed;
        }
    }
}
