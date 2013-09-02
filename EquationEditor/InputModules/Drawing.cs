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
            if (a.First().Value == "Rectangle" && a.ElementAt(1).Type == TokenType.number
                 && a.ElementAt(2).Type == TokenType.number && 
                 a.ElementAt(3).Type == TokenType.number && 
                 a.ElementAt(4).Type == TokenType.number) {
                    return rectangle(a);
            } else if (a.First().Value == "Ellipse" && a.ElementAt(1).Type == TokenType.number
                 && a.ElementAt(2).Type == TokenType.number &&
                 a.ElementAt(3).Type == TokenType.number &&
                 a.ElementAt(4).Type == TokenType.number) {
                    return ellipse(a);
            } else if (a.First().Value == "Line" && a.ElementAt(1).Type == TokenType.number
                 && a.ElementAt(2).Type == TokenType.number &&
                 a.ElementAt(3).Type == TokenType.number &&
                 a.ElementAt(4).Type == TokenType.number) {
                    return line(a);
            }
            throw new Exception();

        }

        private Shape line(Queue<IToken> a) {
            ///TODO: this doesn't work yet
            double x1 = double.Parse(a.ElementAt(1).Value);
            double y1 = double.Parse(a.ElementAt(2).Value);
            double x2 = double.Parse(a.ElementAt(3).Value);
            double y2 = double.Parse(a.ElementAt(4).Value);
            var width = Math.Max(x2,x1);
            var height = Math.Max(y2, y1);
            Line l = new Line() { X1 = x1, Y1 = height - y1, X2 = x2, Y2 = height - y2 };
            Canvas.SetLeft(l, x1);
            Canvas.SetBottom(l, y1);
            l.Stroke = Brushes.Black;
            l.Visibility = Visibility.Visible;
            l.StrokeThickness = 2;
            updateMaxWidthAndHeight(width, height);

            if (a.Count() > 5) {
                var t = double.Parse(a.ElementAt(5).Value);
                l.StrokeThickness = t;
            }
            if (a.Count() > 6) {
                var colorName = a.ElementAt(6).Value;
                var color = (Color)ColorConverter.ConvertFromString(colorName);
                l.Stroke = new SolidColorBrush(color);
            }
            return l;
        }

        private Shape ellipse(Queue<IToken> a) {
            double x = double.Parse(a.ElementAt(1).Value);
            double y = double.Parse(a.ElementAt(2).Value);
            double w = double.Parse(a.ElementAt(3).Value);
            double h = double.Parse(a.ElementAt(4).Value);

            Ellipse r = new Ellipse() { Width = w, Height = h };
            r.Fill = Brushes.Black;
            //c.Children.Add(r);
            Canvas.SetLeft(r, x);
            Canvas.SetBottom(r, y);
            var width = x + w;
            var height = y + h;
            updateMaxWidthAndHeight(width, height);


            if (a.Count() > 5) {
                var colorName = a.ElementAt(5).Value;
                var color = (Color)ColorConverter.ConvertFromString(colorName);
                r.Fill = new SolidColorBrush(color);
            }
            return r;
        }

        private Shape rectangle(Queue<IToken> a) {
            double x = double.Parse(a.ElementAt(1).Value);
            double y = double.Parse(a.ElementAt(2).Value);
            double w = double.Parse(a.ElementAt(3).Value);
            double h = double.Parse(a.ElementAt(4).Value);

            Rectangle r = new Rectangle() { Width = w, Height = h };
            r.Fill = Brushes.Black;
            Canvas.SetLeft(r, x);
            Canvas.SetBottom(r, y);
            var width = x + w;
            var height = y + h;
            updateMaxWidthAndHeight(width, height);


            if (a.Count() > 5) {
                var colorName = a.ElementAt(5).Value;
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
