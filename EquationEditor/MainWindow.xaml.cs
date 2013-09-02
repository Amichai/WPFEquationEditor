using EquationEditor.InputModules;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            Modules = new List<IInputModule>();

            ///Modules: python, drawing, charting, equation editing
            ///File reading input output functionality, api integration

            Modules.Add(new InputModules.IronPython());
            //Modules.Add(new InputModules.BlackBox());
            //Modules.Add(new InputModules.EquationEditor());
            Modules.Add(new DrawingModule());
            Modules.Add(new XamlParser());
            //Modules.Add(new LatexParser());


        }

        public static void SetContextMenu(FrameworkElement control){
            ContextMenu contextMenu = new ContextMenu();
            var copy = new MenuItem() { Header = "Copy" };
            copy.Click += (s, e) => {
                var element = ((s as MenuItem).Parent as ContextMenu).Tag as FrameworkElement;
                double width = element.ActualWidth;
                double height = element.ActualHeight;
                RenderTargetBitmap bmpCopied = new RenderTargetBitmap((int)Math.Round(width), (int)Math.Round(height), 96, 96, PixelFormats.Default);
                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen()) {
                    VisualBrush vb = new VisualBrush(element);
                    dc.DrawRectangle(vb, null, new Rect(new Point(), new Size(width, height)));
                }
                bmpCopied.Render(dv);
                Clipboard.SetImage(bmpCopied);
            };
            //var hide = new MenuItem() { Header = "Hide" };
            //hide.Click += (s, e) => {
            //    var element = ((s as MenuItem).Parent as ContextMenu).Tag as FrameworkElement;
            //    element.Visibility = Visibility.Collapsed;
            //};
            var delete = new MenuItem() { Header = "Delete" };
            delete.Click += (s, e) => {
                var element = ((s as MenuItem).Parent as ContextMenu).Tag as FrameworkElement;
                element.Visibility = Visibility.Collapsed;
            };

            contextMenu.Items.Add(copy);
            //contextMenu.Items.Add(hide);
            contextMenu.Items.Add(delete);
            contextMenu.Tag = control;
            control.ContextMenu = contextMenu;
        }

        ///TODO: append modules to the bottom of an existing pad

        private int workSheetIndex = 0;

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            addSheet((++workSheetIndex).ToString());
        }

        public static List<IInputModule> Modules;

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
