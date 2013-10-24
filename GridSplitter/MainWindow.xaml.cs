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

namespace GridSplitter {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            _smartGrid.Add(new TextBox() { Text = "AAA", Height= 30 });
            //_smartGrid.Add(new TextBox() { Text = "" });

            _smartGrid.Add(new TextBlock() { Text = "BBB" });
            _smartGrid.Add(new TextBlock() { Text = "CCC" });
            _smartGrid.Add(new TextBlock() { Text = "DDD" });
            _smartGrid.Add(new TextBlock() { Text = "EEE" });
        
            _smartGrid.Add(new TextBlock() { Text = "FFF" });

        }

        private void AddButton_Click(object sender, RoutedEventArgs e) {
            _smartGrid.Add(new TextBlock() { Text = _texBox.Text });
        }
    }
}
