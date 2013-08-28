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

namespace EquationEditor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            //this.input.Text = @"y=3+4*2/(1-x)^2^3";
            //this.input.Text = @"add(3,4) / 5";
            //this.input.Text = @"3 + 4 * ( 3 / 22 - 22) ^ 3 ^4";
            this.input.Text = @"4 * 22 = (34 / (3 + 33 + 2^3^3))";

            this.input.BorderThickness = new Thickness(1, 1, 1, 1);
            this.input.BorderBrush = Brushes.Gray;
            update();
        }

        ParseTree tree = new ParseTree();
        Tokenizer tokenizer = new Tokenizer();

        private void Update_Click_1(object sender, RoutedEventArgs e) {
            update();
        }

        private void update() {
            if (this.input.Text == "") {
                return;
            }
            var queue = tokenizer.Tokenize(this.input.Text);
            tree.BuildTree(queue);
            int insertIdx = this.resultStack.Children.IndexOf(this.input);
            var equation = tree.Root.GetElement();
            equation.Tag = this.input.Text;
            this.resultStack.Children.Insert(insertIdx, equation);
            this.input.BorderBrush = Brushes.Gray;
            this.input.Text = "";
            this.lineNumber++;
            //try {
            //} catch {
            //    this.input.BorderBrush = Brushes.Red;
            //}
        }

        int lineNumber = 0;

        private void Swap(UIElementCollection col, int idx1, int idx2) {
            var toTake1 = col[idx1];
            var toTake2 = col[idx2];

            col.Remove(toTake1);
            col.Remove(toTake2);
            col.Insert(idx2, toTake1);

        }

        private void Window_KeyDown_1(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Enter:
                    if (this.input.Text.Count() > 0) {
                        update();
                    } else {
                        var text = (this.resultStack.Children[lineNumber - 1] as FrameworkElement).Tag as string;
                        this.input.Text = text;
                        this.input.CaretIndex = this.input.Text.Length;
                        Swap(this.resultStack.Children, lineNumber, lineNumber - 1);
                        lineNumber--;
                    }
                    break;
            }
        }
    }
}
