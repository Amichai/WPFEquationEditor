using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Workbench.Lib {
    public class Drawing {
        public static UIElement Canvas(double height = 50) {
                return Canvas(Brushes.Black, height);
        }

        public static UIElement Canvas(Brush b, double height = 50) {
            var canvas = new Canvas() { Background = Brushes.White, Height = height };
            Point lastMousePoint = new Point(double.MinValue, double.MinValue);
            Observable.FromEventPattern<MouseEventArgs>(canvas, "MouseMove").Where(i => Mouse.LeftButton == MouseButtonState.Pressed).Subscribe(i => {
                var thisMousePoint = (i.EventArgs as MouseEventArgs).GetPosition(canvas);
                if (lastMousePoint.X != double.MinValue) {
                    var l = new Line() {
                        X1 = 0,
                        Y1 = 0,
                        X2 = thisMousePoint.X - lastMousePoint.X,
                        Y2 = thisMousePoint.Y - lastMousePoint.Y,
                        Stroke = b,
                        Fill = Brushes.Black,
                        StrokeThickness = 3
                    };
                    canvas.Children.Add(l);
                    System.Windows.Controls.Canvas.SetLeft(l, lastMousePoint.X);
                    System.Windows.Controls.Canvas.SetTop(l, lastMousePoint.Y);
                }
                lastMousePoint = thisMousePoint;
            });

            Observable.FromEventPattern<MouseEventArgs>(canvas, "MouseUp").Subscribe(i => {
                lastMousePoint = new Point(double.MinValue, double.MinValue);
            });

            return canvas;
        }

    }
}
