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
            var newLine = new PageLine();
            Observable.FromEventPattern(newLine.del, "Click").Select(i => i.Sender).Subscribe(sl => {
                this.allLines.Children.Remove((sl as Button).Tag as UIElement);
            });

            Observable.FromEventPattern(newLine.input, "PreviewKeyDown").Subscribe(i => {
                //var sender = (i.Sender as PageLine);
                var e = (i.EventArgs as KeyEventArgs);
                if (e.Key == Key.Enter && (Keyboard.IsKeyDown(Key.RightShift) || Keyboard.IsKeyDown(Key.LeftShift))) {
                    var result = CSharp.AppendCSharp(newLine.input.Text, newLine.LineNumber);
                    newLine.SetResult(result);
                    AppendNewLine();
                    e.Handled = true;
                } else if (e.Key == Key.Enter && (Keyboard.IsKeyDown(Key.RightCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl))) {
                    //var result = sender.Result;
                    //if(result == ""){
                    //    result = sender.GetText();
                    //}
                    //CSharp.CSharpAssign(result, sender.LineNumber);

                    AppendNewLine();
                }
            });
            this.allLines.Children.Add(newLine);
            setFocus();
        }
    }
}
