using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EquationEditor {
    public class Node {
        public IToken Token { get; set; }
        public List<Node> Children { get; set; }
        public Node(IToken t) {
            this.Token = t;
            this.Children = new List<Node>();
        }

        public string AsText() {
            if (this.TextRepresentation != null) {
                return TextRepresentation;
            } else {
                return this.Token.Value;
            }
        }

        public string TextRepresentation { get; set; }

        public FrameworkElement GetElement() {
            if (this.Token.Value == "/") {
                StackPanel sp = new StackPanel() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                sp.Orientation = Orientation.Vertical;
                sp.Children.Add(this.Children[1].GetElement());
                sp.Children.Add(new Separator());
                sp.Children.Add(this.Children[0].GetElement());

                sp.PreviewMouseDown += (s, e) => {
                    if (sp.Children.Count == 1) {
                        return;
                    }
                    sp.Children.Clear();
                    var textBox = new TextBox() {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Text = this.AsText(),
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    sp.Children.Add(textBox);
                    textBox.PreviewKeyDown += (s2, e2) => {
                        if (e2.Key == System.Windows.Input.Key.Enter) {
                            sp.Children.Clear();
                            sp.Children.Add(this.Children[1].GetElement());
                            sp.Children.Add(new Separator());
                            sp.Children.Add(this.Children[0].GetElement());
                        }
                    };
                };

                return sp;
            } else if(this.Token.Value == "^"){
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                sp.HorizontalAlignment = HorizontalAlignment.Center;
                sp.Children.Add(this.Children[1].GetElement());
                var margin = new Thickness(0, 0, 0, 5);
                sp.Children.Add(new TextBlock() { Text = this.Token.Value,
                    Margin = margin,
                    TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
                var toAdd = this.Children[0].GetElement();
                toAdd.Margin = margin;
                sp.Children.Add(toAdd);
                return sp;
            }else if (this.Token.Type == TokenType.infixOperator) {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                sp.HorizontalAlignment = HorizontalAlignment.Center;
                sp.Children.Add(this.Children[1].GetElement());
                sp.Children.Add(new TextBlock() { Text = " " + this.Token.Value + " ", TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
                sp.Children.Add(this.Children[0].GetElement());
                return sp;
            } else if(this.Token.Type == TokenType.function){
                StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
                sp.Children.Add(getTextBlock(Token.Value));
                sp.Children.Add(getTextBlock("("));
                for (int i = 0; i < this.Children.Count(); i++) {
                    sp.Children.Add(this.Children[i].GetElement());
                    if (i < this.Children.Count() - 1) {
                        sp.Children.Add(getTextBlock(","));
                    }
                }
                sp.Children.Add(getTextBlock(")"));
                return sp;
            } else if (this.Token.Type == TokenType.number) {
                return new TextBlock() {
                    Text = Token.Value,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
            } else {
                return new TextBlock() {
                    Text = Token.Value,
                    FontStyle = FontStyles.Italic,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
            }
        }

        private FrameworkElement getTextBlock(string text) {
            return new TextBlock() { Text = text, TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        }
    }
}
