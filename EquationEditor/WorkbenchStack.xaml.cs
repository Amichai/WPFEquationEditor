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
            //this.input.Text = "Line(-10,20, 200, 40, 3)";
            this.input.Text = @"tex: \vec{r}(t) = \hat{i} b t + \hat{j} \left( c t - \frac{g t^2}{2} \right) + \hat{k} 0";
            ///TODO: Polygon
            //this.input.Text = "import scipy"
            

            this.input.BorderThickness = new Thickness(1, 1, 1, 1);
            this.input.BorderBrush = Brushes.Gray;
            

            this.inputStrings = new Stack<string>();
            update();
        }

        public static double SelectedFontSize = 12;

        private void Update_Click_1(object sender, RoutedEventArgs e) {
            update();
        }

        private List<IInputModule> GetModulesToTest(string input, out string adjustedInput){            
            adjustedInput = input;
            string keyword = "tex:";
            if (string.Concat(input.Take(keyword.Count())) == keyword) {
                adjustedInput = string.Concat(input.Skip(keyword.Count()));
                return new List<IInputModule>() { new LatexParser() };
            } else {
                return MainWindow.Modules;
            }
            throw new NotImplementedException();
        }

        private void update() {

            string adjustedInput;
            var modulesToTest = GetModulesToTest(this.input.Text, out adjustedInput);            

            FrameworkElement result = null;
            foreach (var m in modulesToTest) {
                try {
                    result = m.Process(adjustedInput);
                    MainWindow.SetContextMenu(result);
                    ///TODO: we should get rid of this.
                    if (result == null) {
                        result = Util.AsTextBlock(adjustedInput, SelectedFontSize);
                    }
                    break;
                } catch (Exception ex){
                    Debug.Print(ex.Message);
                    result = Util.AsTextBlock(adjustedInput, SelectedFontSize);
                    (result as TextBlock).Background = Brushes.Pink;
                }
            }

            ///Getting the index of the text block not the text itself:
            int insertIdx = this.resultStack.Children.IndexOf(this.input);
            //result.Tag = this.input.Text;
            this.resultStack.Children.Insert(insertIdx, result);
            int lineNumber = inputStrings.Count();
            var inputTextBox = Util.AsTextBlock("  (" + lineNumber.ToString() + ") " + this.input.Text, SelectedFontSize, HorizontalAlignment.Left, TextAlignment.Left);
            MainWindow.SetContextMenu(inputTextBox);
            this.resultStack.Children.Insert(insertIdx, inputTextBox);
            this.inputStrings.Push(this.input.Text);
            this.input.Text = "";
        }

        Stack<string> inputStrings;

        ///TODO: visualize the modules in use and offer immediate toggle functionality

        private void Window_KeyDown_1(object sender, KeyEventArgs e) {
            //switch (e.Key) {
            //    case Key.Enter:
            //        update();
            //        break;
            //}
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e) {
            ///TODO: this would be a lot nicer with reactive extensions
            switch (e.Key) {
                case Key.Up:
                    if (inputStrings.Count() == 0) {
                        return;
                    }
                    if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl)) {
                        return;
                    }
                    //if (this.input.CaretIndex != this.input.Text.Length) {
                    //    return;
                    //}
                    var text = inputStrings.Pop();
                    this.input.Text = text;
                    this.input.CaretIndex = this.input.Text.Length;
                    int lastIdx = this.resultStack.Children.Count - 1;
                    this.resultStack.Children.RemoveAt(lastIdx - 1);
                    this.resultStack.Children.RemoveAt(lastIdx - 2);
                    break;
                case Key.Down:
                    if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl)) {
                        return;
                    }
                    if (this.input.CaretIndex != this.input.Text.Length) {
                        return;
                    }
                    update();
                    break;
            }
        }
    }
}
