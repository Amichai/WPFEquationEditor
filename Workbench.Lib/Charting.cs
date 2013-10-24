using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Workbench.Lib {
    public static class Charting {
        public static UIElement Chart(this Func<double, double> f, double x0, double xf, double dx = 0) {
            if (dx == 0) {
                dx = (xf - x0) / 100.0;
            }
            TimeSeries ts = new TimeSeries("");
            for (double i = x0; i < xf; i += dx) {
                ts.Add(i, f(i));
            }
            var chart = new Chart(false);
            chart.AddSeries(ts.GetLineSeries());
            return chart;
        }

        public static UIElement Chart(double x0, double xf, params Func<double, double>[] funcs) {
            var  dx = (xf - x0) / 100.0;
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

        public static UIElement Chart(double x0, double xf, double dx, params Func<double, double>[] funcs) {
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