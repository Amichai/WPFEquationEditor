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
            this.input.Text = @"y = 3 + 4 * 2 / ( 1 - x ) ^ 2 ^ 3";
            update();
        }

        ParseTree tree = new ParseTree();

        private void Update_Click_1(object sender, RoutedEventArgs e) {
            update();
        }

        private void update() {
            var queue = tree.TokenQueue(this.input.Text);
            tree.BuildTree(queue);
            this.resultStack.Children.Add(tree.Root.GetElement());
        }
    }
}
