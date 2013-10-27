using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workbench.Lib {
    public class TimeSeries {
        List<double> domain;
        List<double> range;
        IList<OxyPlot.IDataPoint> points;
        public string Name;
        public TimeSeries(string name) {
            this.domain = new List<double>();
            this.range = new List<double>();
            this.points = new List<IDataPoint>();
            this.Name = name;
        }

        public double MaxVal = double.MinValue;
        public double MinVal = double.MaxValue;

        public void Add(double x, double y) {
            this.domain.Add(x);
            this.range.Add(y);
            if (double.IsNaN(x) || double.IsNaN(y)) {
                throw new Exception("Non a number");
            }
            this.points.Add(new DataPoint(x, y));
            if (y < MinVal) {
                MinVal = y;
            }
            if (y > MaxVal) {
                MaxVal = y;
            }
        }

        public double TotalChange {
            get {
                return EndVal - StartVal;
            }
        }

        public double PercentChange {
            get {
                return (TotalChange / StartVal) * 100.0;
            }
        }

        public double StartVal {
            get {
                return range.Last();
            }
        }

        public double EndVal {
            get {
                return range.First();
            }
        }

        public DateTime StartDate {
            get {
                return DateTimeAxis.ToDateTime(domain.Last());
            }
        }

        public DateTime EndDate {
            get {
                return DateTimeAxis.ToDateTime(domain.First());
            }
        }

        public void LogarithmicX() {
            foreach (var p in points) {
                p.X = Math.Log(p.X);
            }
        }

        public void LogarithmicY() {
            foreach (var p in points) {
                p.Y = Math.Log(p.Y);
            }
        }

        public TimeSeries GetDiffs() {
            double lastVal = double.NaN;
            TimeSeries ts = new TimeSeries(this.Name);
            ///Notice that this for loop iterates forward in time
            for (int i = domain.Count() - 1; i >= 0; i--) {
                double y = range[i] / lastVal - 1;
                if (double.IsNaN(lastVal)) y = 0;
                ts.Add(domain[i], y);
                lastVal = range[i];
            }
            return ts;
        }

        public IEnumerable<double> RangeForwardInTime() {
            for (int i = domain.Count() - 1; i >= 0; i--) {
                yield return range[i];
            }
        }

        private double maxDrawDown = double.MinValue;
        public double MaxDrawDown {
            get {
                if (maxDrawDown != double.MinValue) {
                    return maxDrawDown;
                }
                var a = DrawDowns;
                return maxDrawDown;
            }
        }

        enum sign { plus, minus, equal };

        private List<double> drawDown2 = null;
        public List<double> DrawDown2 {
            get {
                if (drawDown2 != null) return drawDown2;
                List<double> ts = new List<double>();
                sign currentSign = sign.equal;
                sign lastSign = sign.equal;
                double runningSum = 0;
                for (int i = DailyReturns.Count() - 1; i >= 0; i--) {
                    var ret = DailyReturns[i];
                    if (ret > 0) currentSign = sign.plus;
                    if (ret < 0) currentSign = sign.minus;
                    if (ret == 0) currentSign = sign.equal;
                    if (currentSign != sign.equal && currentSign != lastSign && lastSign != sign.equal) {
                        //run just broke
                        ts.Add(runningSum);
                        runningSum = 0;
                    } else {
                        runningSum += ret;
                    }
                    lastSign = currentSign;
                }
                return ts;
            }
        }


        private TimeSeries drawDowns = null;
        public TimeSeries DrawDowns {
            get {
                if (drawDowns != null) return drawDowns;
                TimeSeries ts = new TimeSeries(Name);
                double peak = double.MinValue;
                for (int i = domain.Count() - 1; i >= 0; i--) {
                    if (range[i] > peak) {
                        peak = range[i];
                    }
                    var newDrawDown = 100.0 * (peak - range[i]) / peak;
                    ts.Add(domain[i], newDrawDown);
                    if (newDrawDown > maxDrawDown) {
                        maxDrawDown = newDrawDown;
                    }
                }
                drawDowns = ts;
                return drawDowns;
            }
        }

        TimeSeries diffs = null;
        public TimeSeries DailyReturns {
            get {
                if (diffs != null) return diffs;
                diffs = this.GetDiffs();
                return diffs;
            }
        }

        public ScatterSeries RankOrder(bool positive, bool negative) {
            var tograph = new List<double>();
            if (positive) {
                tograph.AddRange(range.Where(i => i > 0));
            }
            if (negative) {
                tograph.AddRange(range.Where(i => i < 0));
            }
            return range.GraphRankOrder(Name);
        }

        public string GetStats() {
            string s = string.Empty;
            if (domain.Count() == 0) return s;
            s += "Name: " + Name + "\n";
            s += "Percent change: " + PercentChange.ToString() + "\n";
            s += "Range: " + TotalChange.ToString() + "\n";
            s += "Start date: " + StartDate.ToShortDateString() + "\n";
            s += "End date: " + EndDate.ToShortDateString() + "\n";
            s += "Start val: " + StartVal.ToString() + "\n";
            s += "End val: " + EndVal.ToString() + "\n";
            return s;
        }

        public ScatterSeries ToScatterSeries(string title) {
            var ss = new ScatterSeries() { MarkerSize = 2 };
            ss.Title = title;
            ss.Points = points;
            return ss;
        }

        public LineSeries ToLineSeries(string title) {
            var ls = new LineSeries() { StrokeThickness = .5, CanTrackerInterpolatePoints = false };
            ls.Title = title;
            ls.Points = points;
            return ls;
        }

        private double standardDeviation = double.MinValue;
        public double StandardDeviation {
            get {
                if (standardDeviation != double.MinValue) return standardDeviation;
                double ret = 0;
                if (range.Count() > 0) {
                    //Compute the Average      
                    double avg = range.Average();
                    //Perform the Sum of (value-avg)_2_2      
                    double sum = range.Sum(d => Math.Pow(d - avg, 2));
                    //Put it all together      
                    ret = Math.Sqrt((sum) / (range.Count() - 1));
                }
                return ret;
            }
        }

        public double this[int i] {
            get { return range[i]; }
            set { range[i] = value; }
        }

        private double average = double.MinValue;
        public double Average {
            get {
                if (average == double.MinValue) {
                    return range.Average();
                } else {
                    return average;
                }
            }
        }

        public int Count() {
            return domain.Count();
        }

        public LineSeries Normalize0() {
            foreach (var a in points) {
                a.Y /= StartVal;
                a.Y -= 1;
            }
            return GetLineSeries();
        }

        public LineSeries Normalize1() {
            foreach (var a in points) {
                a.Y /= StartVal;
            }
            return GetLineSeries();
        }

        public LineSeries GetLineSeries() {
            return new LineSeries() { Title = Name, StrokeThickness = .5, Points = points, CanTrackerInterpolatePoints = false };
        }


        public Chart Chart(bool dateTimeAxis = false) {
            var chart = new Chart(dateTimeAxis);
            chart.AddSeries(this.GetLineSeries());
            return chart;
        }

        public void ShowLineGraph(string title = "") {
            var chart = new Chart();
            chart.AddSeries(this.GetLineSeries());
            chart.ShowUserControl();
        }
        //Take a data set
        //calculate standard deviation, draw downs, annual return, sharpe, sortino, Jensen's alpha
        //Test correlation between different time series

        public ScatterSeries CorrelateWithNextDay() {
            ScatterSeries ss = new ScatterSeries() { MarkerSize = .8, MarkerStroke = OxyColors.Blue, MarkerFill = OxyColors.Blue };
            bool axis1Direction = this.domain[1] - this.domain[0] > 0;
            int i = this.Count() - 1;
            List<IDataPoint> newPoints = new List<IDataPoint>();
            while (i >= 1) {
                newPoints.Add(new DataPoint(this.range[i], this.range[--i]));
            }
            ss.Points = newPoints;
            return ss;
        }

        public ScatterSeries Correlate2(TimeSeries ts2) {
            ScatterSeries ss = new ScatterSeries() { MarkerSize = .8, MarkerStroke = OxyColors.Blue, MarkerFill = OxyColors.Blue };
            bool axis1Direction = this.domain[1] - this.domain[0] > 0;
            bool axis2Direction = ts2.domain[1] - ts2.domain[0] > 0;
            int i, j;
            if (axis1Direction) {
                i = this.Count() - 1;
            } else {
                i = 0;
            }
            if (axis2Direction) {
                j = ts2.Count() - 1;
            } else {
                j = 0;
            }

            List<IDataPoint> newPoints = new List<IDataPoint>();
            while (i >= 0 && j >= 0 && i < this.Count() && j < ts2.Count()) {
                double domaini = this.domain[i];
                double domainj = ts2.domain[j];
                if (domaini != domainj) {
                    if (domaini > domainj) {
                        //Move i
                        if (axis1Direction) {
                            i--;
                        } else {
                            i++;
                        }
                    } else {
                        //Move j
                        if (axis2Direction) {
                            j--;
                        } else {
                            j++;
                        }
                    }
                    continue;
                } else {
                    newPoints.Add(new DataPoint(this.range[i], ts2.range[j]));
                    //Move both
                    if (axis1Direction) {
                        i--;
                    } else {
                        i++;
                    }
                    if (axis2Direction) {
                        j--;
                    } else {
                        j++;
                    }
                }
            }
            ss.Points = newPoints;
            return ss;
        }

        public ScatterSeries Correlate(TimeSeries ts2) {
            ScatterSeries ss = new ScatterSeries() { MarkerSize = .8, MarkerStroke = OxyColors.Blue, MarkerFill = OxyColors.Blue };
            bool axis1Direction = this.domain[1] - this.domain[0] > 0;
            bool axis2Direction = ts2.domain[1] - ts2.domain[0] > 0;
            int i = this.Count() - 1;
            int j = ts2.Count() - 1;
            List<IDataPoint> newPoints = new List<IDataPoint>();
            while (i >= 0 && j >= 0) {
                double domaini = this.domain[i];
                double domainj = ts2.domain[j];
                if (domaini != domainj) {
                    if (domaini > domainj) {
                        if (axis1Direction) {
                            i--;
                        } else {
                            j--;
                        }
                    } else {
                        if (axis2Direction) {
                            j--;
                        } else {
                            i--;
                        }
                    }
                    continue;
                } else {
                    newPoints.Add(new DataPoint(this.range[i], ts2.range[j]));
                    i--;
                    j--;
                }

            }
            ss.Points = newPoints;
            return ss;
        }

        public ScatterSeries CorrelateDailyReturns(TimeSeries ts2) {
            ScatterSeries ss = new ScatterSeries() { MarkerSize = .8, MarkerStroke = OxyColors.Blue, MarkerFill = OxyColors.Blue };
            int i = this.DailyReturns.Count() - 1;
            int j = ts2.DailyReturns.Count() - 1;
            List<IDataPoint> newPoints = new List<IDataPoint>();
            while (i >= 0 && j >= 0) {
                double domaini = this.DailyReturns.domain[i];
                double domainj = ts2.DailyReturns.domain[j];
                if (domaini != domainj) {
                    if (domaini > domainj) {
                        i--;
                    } else { j--; }
                    continue;
                } else {
                    newPoints.Add(new DataPoint(this.DailyReturns.range[i], ts2.DailyReturns.range[j]));
                    i--;
                    j--;
                }

            }
            ss.Points = newPoints;
            return ss;
        }

        public List<double> GetRange() {
            return range;
        }

        public List<double> GetDomain() {
            return domain;
        }
    }
}
