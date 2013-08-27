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
            this.input.Text = @"add(3,4) / 5";
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
            var queue = tokenizer.Tokenize(this.input.Text);
            tree.BuildTree(queue);
            this.resultStack.Children.Add(tree.Root.GetElement());
            this.input.BorderBrush = Brushes.Gray;
            //try {
            //} catch {
            //    this.input.BorderBrush = Brushes.Red;
            //}
        }
    }
}
