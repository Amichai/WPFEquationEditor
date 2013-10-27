using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Workbench.Lib {
    public static class Charting {
        public static UIElement Parametric(Func<double, double> xOft, Func<double, double> yOft, double t0, double tf, double dt = 0.01) {
            var ts = new TimeSeries("");
            for (double t = t0; t < tf; t += dt) {
                ts.Add(xOft(t), yOft(t));
            }
            return ts.Chart(false);
        }

        public static UIElement Plot(this Func<double, double> f, double x0, double xf, double dx = 0) {
            if (dx == 0) {
                dx = (xf - x0) / 100.0;
            }
            TimeSeries ts = new TimeSeries("");
            for (double i = x0; i < xf; i += dx) {
                ts.Add(i, f(i));
            }
            var chart = new Chart(false);
            //chart.Height = 300;
            chart.AddSeries(ts.GetLineSeries());
            return chart;
        }

        public static UIElement Plot(double x0, double xf, params Func<double, double>[] funcs) {
            var dx = (xf - x0) / 100.0;
            var chart = new Chart(false);
            foreach (var f in funcs) {
                TimeSeries ts = new TimeSeries("");
                for (double i = x0; i < xf; i += dx) {
                    ts.Add(i, f(i));
                }
                chart.AddSeries(ts.GetLineSeries());
            }
            return chart;
        }

        public static ILScene Plot(Func<double, double, double> zOfxy, double x0, double xf, double y0, double yf) {
            // create some test data (RBF) 
            ILArray<float> Y = 1;
            double dx = (xf - x0) / 100;
            double dy = (yf - y0) / 100;


            ILArray<float> result = ILMath.array<float>(new ILSize(xf, yf));
            for (double x = x0; x < xf; x += dx) {
                for (double y = y0; y < yf; y+= dy) {
                    result[x, y] = (float)zOfxy(x, y);
                }
            }

            //ILArray<float> X = ILMath.meshgrid(
            //  ILMath.linspace<float>(-2f, 2f, 100),
            //  ILMath.linspace<float>(-2f, 2f, 100), Y);
            //Y = X * X;
            //Y = X * X + Y * Y;
            //ILArray<float> A = 1 - ILMath.exp(-Y * 2f);

            // create new scene, add plot cube
            var scene = new ILScene {
                      new ILPlotCube(twoDMode: false) {
	                    // rotate around X axis only 
	                    Rotation = Matrix4.Rotation(new Vector3(1,0,0),1.1f),
	                    // set perspective projection 
	                    Projection = Projection.Perspective,  
	                    // add plot cube contents
	                    Childs = {
	                      // add surface plot, default configuration
	                      //new ILSurface(A, 1 - A) { Alpha = 0.9f },
                          new ILSurface(result) { Alpha = 0.9f },
                          //new ILSurface(A) { Alpha = 0.9f },
	                      // add contour plot in 3D, configure individual levels
                          //new ILContourPlot(A, new List<ContourLevel> {
                          //  new ContourLevel() { Value = 0.8f, LineWidth = 4, ShowLabel = false },   
                          //  new ContourLevel() { Value = 0.5f, LineWidth = 4, ShowLabel = false },
                          //  new ContourLevel() { Value = 0.2f, LineWidth = 4, ShowLabel = false, LineColor = 1  },  
                          //  new ContourLevel() { Value = 0.02f, LineWidth = 4, ShowLabel = false},  
                          //}, create3D: true),
                          //// add contour plot in 2D, default levels, no labels
                          //new ILContourPlot(1 - A, showLabels: false), 
                          //// add legend, label first contour plots levels only
                          //new ILLegend("0.8","0.5","0.2","0.02") {
                          //    // move legend to upper right corner
                          //    Location = new System.Drawing.PointF(0.97f,0.05f),
                          //    Anchor = new System.Drawing.PointF(1,0)
                          //}
	                    }
                      }
                    };
            return scene;
        }

        public static UIElement Plot(double x0, double xf, double dx, params Func<double, double>[] funcs) {
            var chart = new Chart(false);
            foreach (var f in funcs) {
                TimeSeries ts = new TimeSeries("");
                for (double i = x0; i < xf; i += dx) {
                    ts.Add(i, f(i));
                }
                chart.AddSeries(ts.GetLineSeries());
            }

            return chart;
        }


    }
}