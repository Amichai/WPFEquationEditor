using EquationEditor.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationEditor {
    public class ParseTree {

        public void BuildTree(List<IToken> TokenQueue) {
            Stack<Node> stackBuffer = new Stack<Node>();
            foreach (var t in TokenQueue) {
                if (t.Type == TokenType.infixOperator) {
                    Node n = new Node(t);
                    n.Children.Add(stackBuffer.Pop());
                    n.Children.Add(stackBuffer.Pop());
                    stackBuffer.Push(n);
                } else if (t.Type == TokenType.function) {
                    Node n = new Node(t);
                    for (int i = 0; i < t.NumberOfChildren; i++) {
                        n.Children.Add(stackBuffer.Pop());
                    }
                    stackBuffer.Push(n);
                } else {
                    stackBuffer.Push(new Node(t));
                }
            }
            if (stackBuffer.Count() > 0) {
                Root = stackBuffer.Single();
            } else {
                Root = new Node(new Keyword(""));
            }
        }

        public Node Root;
    }
}
