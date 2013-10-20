using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
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
                return this.result.Text;
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

        internal void SetResult(string result) {
            if (result == "") {
                return;
            }
            this.result.Text = result;
            this.delResult.Visibility = Visibility.Visible;
            this.result.Visibility = Visibility.Visible;
        }

        public void ClearResult() {
            this.delResult.Visibility = Visibility.Collapsed;
            this.result.Visibility = Visibility.Collapsed;
        }

        private void delResult_Click_1(object sender, RoutedEventArgs e) {
            ClearResult();
        }
    }
}
