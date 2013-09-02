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

        public List<IToken> Tokenize(string text) {
            List<string> tokenStrings = new List<string>() { text };
            foreach (string c in breakingChars) {
                tokenStrings = breakOnChar(tokenStrings, c);
            }
            var tokens = tokenStrings.Select(i => TokenLibrary.Create(i)).ToList();

            return ConvertToPostfixed(tokens);
        }

        private IToken lastToken;

        private List<IToken> ConvertToPostfixed(List<IToken> tokens) {
            List<IToken> postFixed = new List<IToken>();
            Stack<IToken> operatorStack = new Stack<IToken>();
            foreach (var token in tokens) { 
                switch (token.Type) {
                    case TokenType.number:
                         postFixed.Add(token);
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
                                postFixed.Add(operatorStack.Pop());
                            }
                            int stackPos;
                            for(stackPos = operatorStack.Count() - 1; stackPos >= 0; stackPos--){
                                if(operatorStack.ElementAt(stackPos).Value == "("){
                                    break;
                                }
                            }
                            operatorStack.ElementAt(stackPos + 1).NumberOfChildren++;
                        } else if (token.Value == "(") {
                            operatorStack.Push(token);
                            break;
                        } else if (token.Value == ")") {
                            if (operatorStack.Count() == 0) {
                                throw new Exception("Mismatched parenthesis");
                            }
                            while (operatorStack.Peek().Value != "(") {
                                postFixed.Add(operatorStack.Pop());
                            }
                            operatorStack.Pop();
                        }
                        break;
                    case TokenType.infixOperator:
                        if (token.Value == "-" && lastToken != null && (breakingChars.Contains(lastToken.Value))) {
                            //operatorStack.Push() ///push a single valued negation function
                            postFixed.Add(new Function("Negate") { NumberOfChildren = 1 });
                            break;
                        }

                        var asInfix = token as InfixOperator;
                        while (operatorStack.Count() > 0 ){
                            if (operatorStack.Peek() is InfixOperator) {
                                var toPopAndAdd = operatorStack.Peek() as InfixOperator;
                                if ((asInfix.Associativity == Associativity.left && asInfix.Precedence == toPopAndAdd.Precedence)
                                    || asInfix.Precedence < toPopAndAdd.Precedence) {
                                    postFixed.Add(operatorStack.Pop());
                                } else {
                                    break;
                                }
                            } else if (operatorStack.Peek() is IFunction) {
                                postFixed.Add(operatorStack.Pop());
                            } else {
                                break;
                            }
                        }
                        operatorStack.Push(token);
                        break;
                    case TokenType.keyword:
                        postFixed.Add(token);
                        break;
                    default:
                        throw new Exception();
                }
                this.lastToken = token;
            }
            while (operatorStack.Count() > 0) {
                postFixed.Add(operatorStack.Pop());
            }
            if (postFixed.Where(i => i.Value == " ").Count() > 0) {
                throw new Exception();
            }
            for (int i = 0; i < postFixed.Count() - 1; i++) {
                var thisToken = postFixed.ElementAt(i);
                var nextToken = postFixed.ElementAt(i + 1);
                if (i < postFixed.Count() - 1 && thisToken.Value == "Negate" && nextToken.Type == TokenType.number) {
                    var newVal = (nextToken as Number).numericalVal * -1;
                    nextToken.Value = newVal.ToString();
                    postFixed.RemoveAt(i);
                    ///Remove the thisToken
                    ///update the indices
                }
            }
            return postFixed;
        }
    }
}
