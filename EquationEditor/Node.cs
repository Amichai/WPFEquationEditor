using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EquationEditor {
    public class Node {
        public IToken Val { get; set; }
        public List<Node> Children { get; set; }
        public Node(IToken t) {
            this.Val = t;
            this.Children = new List<Node>();
        }

        public FrameworkElement GetElement() {
            if (this.Val.Value == "/") {
                StackPanel sp = new StackPanel() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                sp.Orientation = Orientation.Vertical;
                sp.Children.Add(this.Children[1].GetElement());
                sp.Children.Add(new Separator());
                sp.Children.Add(this.Children[0].GetElement());
                return sp;
            } else if(this.Val.Value == "^"){
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                sp.HorizontalAlignment = HorizontalAlignment.Center;
                sp.Children.Add(this.Children[1].GetElement());
                var margin = new Thickness(0, 0, 0, 5);
                sp.Children.Add(new TextBlock() { Text = this.Val.Value,
                    Margin = margin,
                    TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
                var toAdd = this.Children[0].GetElement();
                toAdd.Margin = margin;
                sp.Children.Add(toAdd);
                return sp;
            }else if (this.Children.Count() == 2) {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                sp.HorizontalAlignment = HorizontalAlignment.Center;
                sp.Children.Add(this.Children[1].GetElement());
                sp.Children.Add(new TextBlock() { Text = " " + this.Val.Value + " ", TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center });
                sp.Children.Add(this.Children[0].GetElement());
                return sp;
            } else {
                return new TextBlock() { Text = Val.Value, TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            }
        }
    }
}
