using EquationEditor.InputModules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for WorkbenchStack.xaml
    /// </summary>
    public partial class WorkbenchStack : UserControl {
        public WorkbenchStack() {
            InitializeComponent();
            //this.input.Text = @"y=3+4*2/(1-x)^2^3";
            //this.input.Text = @"add(3,4) / 5";
            //this.input.Text = @"3 + 4 * ( 3 / 22 - 22) ^ 3 ^4";
            //this.input.Text = @"4 * 22 = (34 / (3 + 33 + 2^3^3))";
            //this.input.Text = @"4 * 22 = (34 / (3 + 33 + 2^3^3))  / 33^3^3^3^3^3^3^3^3^3";
            //this.input.Text = "Rectangle(10,20, 200, 40, Green);Rectangle(10,0, 10, 40, Red);";
            ///TODO: negative numbers don't work yet. Known issue:
            //this.input.Text = "Ellipse(-10,20, 200, 40, Green);Rectangle(10,0, 10, 40, Red)";
            this.input.Text = "Line(10,20, 200, 40, 3)";
            ///Polygon, line
            //this.input.Text = "import scipy";
            

            this.input.BorderThickness = new Thickness(1, 1, 1, 1);
            this.input.BorderBrush = Brushes.Gray;
            

            this.inputStrings = new Stack<string>();
            update();
        }
        private void Update_Click_1(object sender, RoutedEventArgs e) {
            update();
        }

        ///TODO: preserve original input above the output       

        private void update() {
            int insertIdx = this.resultStack.Children.IndexOf(this.input);
            FrameworkElement result = null;
            foreach (var m in MainWindow.Modules) {
                try {
                    result = m. Process(this.input.Text);
                    if (result == null) {
                        result = Util.AsTextBlock(this.input.Text);
                    }
                    break;
                } catch (Exception ex){
                    Debug.Print(ex.Message);
                    result = Util.AsTextBlock(this.input.Text);
                    (result as TextBlock).Background = Brushes.Pink;
                }
            }

            result.Tag = this.input.Text;
            this.resultStack.Children.Insert(insertIdx, result);
            int lineNumber = inputStrings.Count();
            var inputTextBox = Util.AsTextBlock("  (" + lineNumber.ToString() + ") " + this.input.Text, HorizontalAlignment.Left);
            this.resultStack.Children.Insert(insertIdx, inputTextBox);
            this.inputStrings.Push(this.input.Text);
            this.input.Text = "";
        }

        Stack<string> inputStrings;

        ///TODO: visualize the modules in use and offer immediate toggle functionality

        private void Window_KeyDown_1(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Enter:
                    update();
                    break;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Up:
                    if (inputStrings.Count() == 0) {
                        return;
                    }
                    var text = inputStrings.Pop();
                    this.input.Text = text;
                    this.input.CaretIndex = this.input.Text.Length;
                    int lastIdx = this.resultStack.Children.Count - 1;
                    this.resultStack.Children.RemoveAt(lastIdx - 1);
                    this.resultStack.Children.RemoveAt(lastIdx - 2);
                    break;
                case Key.Down:
                    update();
                    break;
            }
        }
    }
}
