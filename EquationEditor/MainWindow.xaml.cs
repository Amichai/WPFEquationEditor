using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
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
using System.Windows.Threading;

namespace EquationEditor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private int workSheetIndex = 0;

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            addSheet((++workSheetIndex).ToString());
        }

        private void addSheet(string title) {
            var stack = new WorkbenchStack();
            var s = new Xceed.Wpf.AvalonDock.Layout.LayoutAnchorable() { Title = title };

            s.Content = stack;

            this.root.Children.Insert(0, s);
            this.root.Children.First().IsSelected = true;            
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e) {
            addSheet((++workSheetIndex).ToString());

        }
    }
}
