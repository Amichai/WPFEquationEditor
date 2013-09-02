using Microsoft.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EquationEditor.InputModules {
    class DrawingModule : IInputModule {
        public DrawingModule() {
        }

        private double maxHeight;
        private double maxWidth;

        private void updateMaxWidthAndHeight(double width, double height) {
            if (width > maxWidth) {
                this.maxWidth = width;
            }
            if (height > maxHeight) {
                this.maxHeight = height;
            }
        }

        private Shape getShape(string command) {
            Tokenizer t = new Tokenizer();
            var a = t.Tokenize(command);
            ParseTree p = new ParseTree();
            p.BuildTree(a);
            if(p.Root.Token.Value == "Rectangle") {
                    return rectangle(p.Root);
            } else if (p.Root.Token.Value == "Ellipse") {
                    return ellipse(p.Root);
            } else if (p.Root.Token.Value == "Line") {
                    return line(p.Root);
            }
            throw new Exception();

        }

        private Shape line(Node root) {
            ///TODO: this doesn't work yet
            int lastIndex = root.Children.Count() - 1;
            double x1 = double.Parse(root.Children[lastIndex].Token.Value);
            double y1 = double.Parse(root.Children[lastIndex - 1].Token.Value);
            double x2 = double.Parse(root.Children[lastIndex - 2].Token.Value);
            double y2 = double.Parse(root.Children[lastIndex - 3].Token.Value);
            var width = Math.Max(x2,x1);
            var height = Math.Max(y2, y1);
            Line l = new Line() { X1 = x1, Y1 = height - y1, X2 = x2, Y2 = height - y2 };
            Canvas.SetLeft(l, x1);
            Canvas.SetBottom(l, y1);
            l.Stroke = Brushes.Black;
            l.Visibility = Visibility.Visible;
            l.StrokeThickness = 2;
            updateMaxWidthAndHeight(width, height);

            if (root.Children.Count() > 4) {
                var t = double.Parse(root.Children[lastIndex - 4].Token.Value);
                l.StrokeThickness = t;
            }
            if (root.Children.Count() > 5) {
                var colorName = root.Children[lastIndex - 5].Token.Value;
                var color = (Color)ColorConverter.ConvertFromString(colorName);
                l.Stroke = new SolidColorBrush(color);
            }
            return l;
        }

        private Shape ellipse(Node root) {
            int lastIndex = root.Children.Count() - 1;
            double x = double.Parse(root.Children[lastIndex].Token.Value);
            double y = double.Parse(root.Children[lastIndex - 1].Token.Value);
            double w = double.Parse(root.Children[lastIndex - 2].Token.Value);
            double h = double.Parse(root.Children[lastIndex - 3].Token.Value);

            Ellipse r = new Ellipse() { Width = w, Height = h };
            r.Fill = Brushes.Black;
            //c.Children.Add(r);
            Canvas.SetLeft(r, x);
            Canvas.SetBottom(r, y);
            var width = x + w;
            var height = y + h;
            updateMaxWidthAndHeight(width, height);


            if (root.Children.Count() > 4) {
                var colorName = root.Children[lastIndex - 4].Token.Value;
                var color = (Color)ColorConverter.ConvertFromString(colorName);
                r.Fill = new SolidColorBrush(color);
            }
            return r;
        }

        private Shape rectangle(Node root) {
            int lastIndex = root.Children.Count() - 1;
            double x = double.Parse(root.Children[lastIndex].Token.Value);
            double y = double.Parse(root.Children[lastIndex - 1].Token.Value);
            double w = double.Parse(root.Children[lastIndex - 2].Token.Value);
            double h = double.Parse(root.Children[lastIndex - 3].Token.Value);

            Rectangle r = new Rectangle() { Width = w, Height = h };
            r.Fill = Brushes.Black;
            Canvas.SetLeft(r, x);
            Canvas.SetBottom(r, y);
            var width = x + w;
            var height = y + h;
            updateMaxWidthAndHeight(width, height);


            if (root.Children.Count() > 4) {
                var colorName = root.Children[lastIndex - 4].Token.Value;
                var color = (Color)ColorConverter.ConvertFromString(colorName);
                r.Fill = new SolidColorBrush(color);
            }
            return r;
        }
        public FrameworkElement Process(string input) {
            Canvas c = intialize();

            var drawCommands = input.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var command in drawCommands) {
                var s = getShape(command);
                c.Children.Add(s);
            }
            c.Width = maxWidth;
            c.Height = maxHeight;
            return c;
        }

        private Canvas intialize() {
            Canvas c = new Canvas() { HorizontalAlignment = HorizontalAlignment.Left };
            ///TODO: draw an x and y axis?
            this.maxWidth = 0;
            this.maxHeight = 0;
            return c;
        }
    }
}
