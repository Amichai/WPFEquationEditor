﻿using IronPython.Hosting;
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
            //this.input.Text = @"4 * 22 = (34 / (3 + 33 + 2^3^3))";
            //this.input.Text = @"4 * 22 = (34 / (3 + 33 + 2^3^3))  / 33^3^3^3^3^3^3^3^3^3";
            this.input.Text = "3 ** 4";

            this.input.BorderThickness = new Thickness(1, 1, 1, 1);
            this.input.BorderBrush = Brushes.Gray;
            this.modules = new List<IInputModule>();
            this.modules.Add(new InputModules.IronPython());
            this.modules.Add(new InputModules.BlackBox());
            this.modules.Add(new InputModules.EquationEditor());

            this.inputStrings = new Stack<string>();
            update();
        }

        private void Update_Click_1(object sender, RoutedEventArgs e) {
            update();
        }

        List<IInputModule> modules;
        ///TODO: preserve original input above the output

        private void update() {
            int insertIdx = this.resultStack.Children.IndexOf(this.input);
            FrameworkElement result = null;
            foreach (var m in modules) {
                try {
                    result = m.Process(this.input.Text);
                    if (result == null) {
                        result = Util.AsTextBlock(this.input.Text);
                    }
                    break;
                } catch {
                    result = Util.AsTextBlock(this.input.Text);
                    (result as TextBlock).Background = Brushes.Pink;
                }
            }

            result.Tag = this.input.Text;
            this.resultStack.Children.Insert(insertIdx, result);
            int lineNumber = inputStrings.Count();
            var inputTextBox = Util.AsTextBlock("  (" + lineNumber.ToString() + ") "+  this.input.Text, HorizontalAlignment.Left);
            this.resultStack.Children.Insert(insertIdx, inputTextBox);
            this.inputStrings.Push(this.input.Text);
            this.input.Text = "";
        }

        Stack<string> inputStrings;

        ///TODO: visualize the modules in use and offer immediate toggle functionality

        private void Window_KeyDown_1(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Enter:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) {
                        var text = inputStrings.Pop();
                        this.input.Text = text;
                        this.input.CaretIndex = this.input.Text.Length;
                        int lastIdx = this.resultStack.Children.Count - 1;
                        this.resultStack.Children.RemoveAt(lastIdx - 1);
                        this.resultStack.Children.RemoveAt(lastIdx - 2);
                    } else {
                        update();
                    }
                    break;
            }
        }
    }
}
