using codeding.WPF.XamlQuery;
using System;
using System.Collections.Generic;
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
using Workbench.Controls;

namespace Workbench {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            CSharp = new CSharpEngine();
            AppendNewLine();
            setFocus();
        }

        void setFocus() {
            var count = this.allLines.Children.Count;
            var line = this.allLines.Children[count - 1] as PageLine;
            line.SetFocus();
        }

        public static CSharpEngine CSharp;

        public void AppendNewLine() {
            selectedIndex++;
            var newLine = new PageLine();
            Observable.FromEventPattern(newLine.del, "Click").Select(i => i.Sender).Subscribe(sl => {
                this.allLines.Children.Remove((sl as Button).Tag as UIElement);
            });
            newLine.NewUIResult.Subscribe(i => {
                var smartGrid = new GridSplitter.SmartGrid();
                //i.Height = double.NaN;
                smartGrid.Add(i);
                this.allLines.Children.Add(smartGrid);
                selectedIndex++;
            });

            Observable.FromEventPattern(newLine.input, "PreviewKeyDown").Subscribe(i => {
                var sender = ((i.Sender as TextBox).Tag as PageLine);
                var e = (i.EventArgs as KeyEventArgs);
                if (e.Key == Key.Enter && (Keyboard.IsKeyDown(Key.RightShift) || Keyboard.IsKeyDown(Key.LeftShift))) {
                    var result = CSharp.AppendCSharp(newLine.input.Text, newLine.LineNumber);
                    newLine.SetResult(result);
                    AppendNewLine();
                    e.Handled = true;
                } else if (e.Key == Key.Enter && (Keyboard.IsKeyDown(Key.RightCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl))) {
                    var result = sender.Result;
                    if (result == "") {
                        result = sender.GetText();
                    }
                    CSharp.CSharpAssign(result, sender.LineNumber);
                    AppendNewLine();
                    e.Handled = true;
                }
            });
            this.allLines.Children.Add(newLine);
            setFocus();
        }

        private int selectedIndex = 0;

        ///TODO: thisi fails, because all lines contains result controls which aren't PageLines anymore
        private void setTextFromIndex(int index) {
            var count = this.allLines.Children.Count;
            var pl = (this.allLines.Children[index] as PageLine);
            if (pl == null) {
                return;
            }
            var text = pl.input.Text;
            var active = this.allLines.Children[count - 1];
            (active as PageLine).input.Text = text;
        }

        private void Window_PreviewKeyDown_1(object sender, KeyEventArgs e) {
            if (e.Key == Key.Up) {
                if (selectedIndex < 1) {
                    return;
                }
                selectedIndex--;
                if (selectedIndex > this.allLines.Children.Count - 1) {
                    return;
                }
                setTextFromIndex(selectedIndex);
            } else if (e.Key == Key.Down) {
                if (selectedIndex > this.allLines.Children.Count - 2) {
                    return;
                }
                selectedIndex++;
                setTextFromIndex(selectedIndex);
            }
        }
    }
}
