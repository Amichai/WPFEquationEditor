using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Workbench.Lib {
    /// <summary>
    /// Interaction logic for Chart.xaml
    /// </summary>
    public partial class Chart : UserControl {
        PlotModel plot = new PlotModel();
        public Chart(bool dateTimeAxis = true, Axis xAxis = null, Axis yAxis = null) {
            InitializeComponent();
            //this.Height = 300;
            Random rand = new Random();
            if (dateTimeAxis) {
                plot.Axes.Add(new DateTimeAxis());
            }
            if (xAxis != null) {
                xAxis.Position = AxisPosition.Bottom;
                if (plot.Axes.Count() > 0) {
                    plot.Axes[0] = xAxis;
                } else {
                    plot.Axes.Add(xAxis);
                }
            }
            if (yAxis != null) {
                yAxis.Position = AxisPosition.Left;
                if (plot.Axes.Count() > 1) {
                    plot.Axes[1] = yAxis;
                } else {
                    plot.Axes.Add(yAxis);
                }
            }
            this.Root.Model = plot;
        }

        public void AddSeries(CandleStickSeries s) {
            s.Color = colors[plot.Series.Count() % colors.Count()].ToOxyColor();
            plot.Series.Add(s);
            this.Root.Model.IsLegendVisible = false;
            this.Root.Model = plot;
        }

        public void AddSeries(BarSeries b) {
            plot.Series.Add(b);
            this.Root.Model = plot;
        }

        public void AddSeries(RectangleBarSeries b) {
            plot.Series.Add(b);
            this.Root.Model = plot;
        }

        public void AddSeries(ScatterSeries s) {
            s.MarkerFill = colors[plot.Series.Count() % colors.Count()].ToOxyColor();

            plot.Series.Add(s);
            this.Root.Model = plot;
        }

        public void AddSeries(LineSeries s) {
            s.Color = colors[plot.Series.Count() % colors.Count()].ToOxyColor();
            plot.Series.Add(s);
            this.Root.Model = plot;
        }


        List<Color> colors = new List<Color> { Colors.Black, Colors.Red, Colors.Blue, Colors.Green, Colors.Purple, Colors.Black };

        private void UserControl_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Escape:
                    ///Close this window
                    throw new NotImplementedException();
            }
        }
    }
}
