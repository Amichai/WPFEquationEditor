using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
using System.Windows.Threading;

namespace Workbench.Controls {
    /// <summary>
    /// Interaction logic for PageLine.xaml
    /// </summary>
    public partial class PageLine : UserControl, INotifyPropertyChanged {
        public PageLine() {
            InitializeComponent();
            this.LineNumber = lineCounter++;
        }

        public void SetFocus() {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate() {
                this.input.Focus();         // Set Logical Focus
                Keyboard.Focus(this.input); // Set Keyboard Focus
            }));
        }

        private static int lineCounter = 0;

        private int _LineNumber;
        public int LineNumber {
            get { return _LineNumber; }
            set {
                if (_LineNumber != value) {
                    _LineNumber = value;
                    OnPropertyChanged("LineNumber");
                }
            }
        }

        public string Result {
            get {
                return this.textResult.Text;
            }
        }

        public string GetText() {
            return this.input.Text;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) {
            var eh = PropertyChanged;
            if (eh != null) {
                eh(this, new PropertyChangedEventArgs(name));
            }
        }

        private void UserControl_PreviewMouseDown_1(object sender, MouseButtonEventArgs e) {
            this.SetFocus();
        }

        private void stringResult(string result) {
            if (result == "") {
                return;
            }
            this.textResult.Text += result + "\n";
            this.delResult.Visibility = Visibility.Visible;
            this.textResult.Visibility = Visibility.Visible;
        }

        public Subject<FrameworkElement> NewUIResult = new Subject<FrameworkElement>();

        internal void SetResult(object result) {
            if (result is string) {
                var asString = result as string;
                stringResult(asString);
            } else if (result is FrameworkElement) {
                //(result as FrameworkElement).Height = double.NaN;
                this.NewUIResult.OnNext(result as FrameworkElement);
            } else if (result is ILScene) {
                ILPanel p = new ILPanel();
                p.Scene.Add(result as ILScene);
                System.Windows.Forms.Integration.WindowsFormsHost e = new System.Windows.Forms.Integration.WindowsFormsHost();
                e.Child = p;
                //e.Height = 500;
                Grid g = new Grid();
                g.Children.Add(e);
                this.NewUIResult.OnNext(g as FrameworkElement);
            } else if (result is ILPlotCube) {
                ILPanel p = new ILPanel();
                p.Scene.Add(result as ILPlotCube);
                System.Windows.Forms.Integration.WindowsFormsHost e = new System.Windows.Forms.Integration.WindowsFormsHost();
                e.Child = p;
                //e.Height = 500;
                Grid g = new Grid();
                g.Children.Add(e);
                this.NewUIResult.OnNext(g as FrameworkElement);
            } else if (result is IEnumerable) {
                foreach (var r in (result as IEnumerable)) {
                    SetResult(r);
                }
            } else {
                stringResult(result.ToString());
            }
        }

        public void ClearResult() {
            this.delResult.Visibility = Visibility.Collapsed;
            this.textResult.Visibility = Visibility.Collapsed;
        }

        private void delResult_Click_1(object sender, RoutedEventArgs e) {
            ClearResult();
        }
    }
}
