using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationEditor {
    public class ParseTree {
        public Queue<Token> TokenQueue(string text) {
            Queue<Token> outputQueue = new Queue<Token>();
            Stack<Token> operatorStack = new Stack<Token>();
            var tokenStrings = text.Split(' ');
            var tokens = tokenStrings.Select(i => Token.Create(i));
            foreach (var token in tokens) {
                switch (token.Type) {
                    case TokenType.number:
                        outputQueue.Enqueue(token);
                        break;
                    case TokenType.function:
                        operatorStack.Push(token);
                        break;
                    case TokenType.seperator:
                        while (operatorStack.Peek().Type != TokenType.leftParen) {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }
                        break;
                    case TokenType.infixOperator:
                        while (operatorStack.Count() > 0 && operatorStack.Peek().Type == TokenType.infixOperator) {
                            var toPopAndAdd = operatorStack.Peek();
                            if ((token.Precedence.Associativity == Associativity.left && token.Precedence.Val == toPopAndAdd.Precedence.Val)
                                || token.Precedence.Val < toPopAndAdd.Precedence.Val) {
                                outputQueue.Enqueue(operatorStack.Pop());
                            } else {
                                break;
                            }
                            
                        }
                        operatorStack.Push(token);
                        break;
                    case TokenType.leftParen:
                        operatorStack.Push(token);
                        break;
                    case TokenType.rightParen:
                        while (operatorStack.Peek().Type != TokenType.leftParen) {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }
                        operatorStack.Pop();
                        break;
                    case TokenType.stringLiteral:
                        outputQueue.Enqueue(token);
                        break;
                    default:
                        throw new Exception();
                }
            }
            while (operatorStack.Count() > 0) {
                outputQueue.Enqueue(operatorStack.Pop());
            }
           
            return outputQueue;
        }

        public void BuildTree(Queue<Token> TokenQueue) {
            Stack<Node> stackBuffer = new Stack<Node>();
            foreach (var t in TokenQueue) {
                if (t.Type == TokenType.infixOperator) {
                    Node n = new Node(t);
                    n.Children.Add(stackBuffer.Pop());
                    n.Children.Add(stackBuffer.Pop());
                    stackBuffer.Push(n);
                } else {
                    stackBuffer.Push(new Node(t));
                }
            }
            Root = stackBuffer.Single();
        }

        public Node Root;
    }
}
