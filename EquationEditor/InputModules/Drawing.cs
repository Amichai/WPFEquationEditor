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
        Canvas c;
        private double canvasSetWidth = 0;
        private double canvasSetHeight = 0;

        private void setWidthAndHeight(double width, double height) {
            if (width > canvasSetWidth) {
                c.Width = width;
                this.canvasSetWidth = width;
            }
            if (height > canvasSetHeight) {
                c.Height = height;
                this.canvasSetHeight = height;
            }
        }

        private void draw(string command) {
            Tokenizer t = new Tokenizer();
            var a = t.Tokenize(command);
            
            if (a.First().Value == "Rectangle" && a.ElementAt(1).Type == TokenType.number
                 && a.ElementAt(2).Type == TokenType.number && 
                 a.ElementAt(3).Type == TokenType.number && 
                 a.ElementAt(4).Type == TokenType.number) {
                     double x = double.Parse(a.ElementAt(1).Value);
                     double y = double.Parse(a.ElementAt(2).Value);
                     double w = double.Parse(a.ElementAt(3).Value);
                     double h = double.Parse(a.ElementAt(4).Value);

                Rectangle r = new Rectangle() { Width = w, Height = h };
                r.Fill = Brushes.Black;
                c.Children.Add(r);
                Canvas.SetLeft(r, x);
                Canvas.SetBottom(r, y);
                var width = x + w;
                var height = y + h;
                setWidthAndHeight(width, height);


                if (a.Count() > 5) {
                    var colorName = a.ElementAt(5).Value;
                    var color = (Color)ColorConverter.ConvertFromString(colorName);
                    r.Fill = new SolidColorBrush(color);
                }
            }
        }


        public FrameworkElement Process(string input) {
            c = new Canvas();
            canvasSetWidth = 0;
            canvasSetHeight = 0;


            var drawCommands = input.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var command in drawCommands) {
                draw(command);
            }
            c.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            
            return c;
        }
    }
}
