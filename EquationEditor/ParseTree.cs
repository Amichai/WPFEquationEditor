﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationEditor {
    public class ParseTree {

        public void BuildTree(Queue<IToken> TokenQueue) {
            Stack<Node> stackBuffer = new Stack<Node>();
            foreach (var t in TokenQueue) {
                if (t.Type == TokenType.infixOperator) {
                    Node n = new Node(t);
                    n.Children.Add(stackBuffer.Pop());
                    n.Children.Add(stackBuffer.Pop());
                    stackBuffer.Push(n);
                } else if (t.Type == TokenType.function) {
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