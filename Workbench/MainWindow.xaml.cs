using codeding.WPF.XamlQuery;
using Microsoft.Win32;
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
using System.Xml.Linq;
using Workbench.Controls;

namespace Workbench {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            CSharp = new CSharpEngine();
            AppendNewLine(new PageLine());
            setFocus();
        }

        void setFocus() {
            var count = this.allLines.Children.Count;
            var line = this.allLines.Children[count - 1] as PageLine;
            line.SetFocus();
        }

        public static CSharpEngine CSharp;

        public void AppendNewLine(PageLine newLine) {
            selectedIndex++;
            Observable.FromEventPattern(newLine.del, "Click").Select(i => i.Sender).Subscribe(sl => {
                var lineToDelete = (sl as Button).Tag as UIElement;
                if (lineToDelete is PageLine) {
                    ///TODO: still buggy...
                    //foreach (var l in (lineToDelete as PageLine).GetDependentLineIndices()) {
                    //    this.allLines.Children.RemoveAt(l);
                    //}
                }
                this.allLines.Children.Remove(lineToDelete);
            });
            newLine.NewUIResult.Subscribe(i => {
                var smartGrid = new GridSplitter.SmartGrid();
                smartGrid.Add(i);
                this.allLines.Children.Add(smartGrid);
                newLine.AddDependentLineIndex(selectedIndex);
                selectedIndex++;
            });

            Observable.FromEventPattern(newLine.input, "PreviewKeyDown").Subscribe(i => {
                var sender = ((i.Sender as TextBox).Tag as PageLine);
                var e = (i.EventArgs as KeyEventArgs);
                if (e.Key == Key.Enter && (Keyboard.IsKeyDown(Key.RightShift) || Keyboard.IsKeyDown(Key.LeftShift))) {
                    var result = CSharp.AppendCSharp(newLine.input.Text, newLine.LineNumber);
                    newLine.SetResult(result);
                    AppendNewLine(new PageLine());
                    e.Handled = true;
                } else if (e.Key == Key.Enter && (Keyboard.IsKeyDown(Key.RightCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl))) {
                    var result = sender.Result;
                    if (result == "") {
                        result = sender.GetText();
                    }
                    CSharp.CSharpAssign(result, sender.LineNumber);
                    AppendNewLine(new PageLine());
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

        ///TODO: serialize and save to xml
        ///TODO: undo stack, up arrow
        ///TODO: delete result button

        private void Save_Click_1(object sender, RoutedEventArgs e) {
            XElement root = new XElement("AllLines");
            foreach (var l in this.allLines.Children) {
                if ((l as PageLine) == null) {
                    continue;
                }
                var lineXml = (l as PageLine).ToXml();
                root.Add(lineXml);
            }
            var dialog = new Microsoft.Win32.SaveFileDialog();
            var success = dialog.ShowDialog().Value;
            if (success) {
                root.Save(dialog.FileName);
            }
        }

        private void resetContent() {
            this.allLines.Children.Clear();
            this.selectedIndex = 0;
            PageLine.ResetLineCounter();
        }

        private void Open_Click_1(object sender, RoutedEventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            var success = ofd.ShowDialog().Value;
            if (success) {
                var xml = XElement.Load(ofd.FileName);
                resetContent();
                foreach (var line in xml.Elements("Line")) {
                    var newLine = PageLine.FromXml(line);
                    this.AppendNewLine(newLine);
                    if (newLine.Type == LineType.Eval) {
                        var result = CSharp.AppendCSharp(newLine.input.Text, newLine.LineNumber);
                        newLine.SetResult(result);
                    }
                }
            }
        }
    }
}
