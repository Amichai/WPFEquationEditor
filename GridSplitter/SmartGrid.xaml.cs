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
    public partial class SmartGrid : UserControl {
        public SmartGrid() {
            InitializeComponent();
        }

        public void Add(UIElement child) {
            int rowIndex = _grid.RowDefinitions.Count();

            _grid.RowDefinitions.Add(
                new RowDefinition() {
                    Height = new GridLength(rowIndex == 0 ? 0 : 5)
                });

            System.Windows.Controls.GridSplitter gridSplitter =
                new System.Windows.Controls.GridSplitter() {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    ResizeDirection = GridResizeDirection.Rows,
                    Background = Brushes.Black
                };

            _grid.Children.Add(gridSplitter);
            Grid.SetRow(gridSplitter, rowIndex);
            Grid.SetColumn(gridSplitter, 0);
            Grid.SetColumnSpan(gridSplitter, 2);

            rowIndex++;

            _grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            Button button = new Button();
            button.Content = "Delete";
            button.Tag = new TagTuple() { Child = child, GridSplitter = gridSplitter };
            button.Click += new RoutedEventHandler(DeleteButton_Click);

            _grid.Children.Add(button);
            Grid.SetRow(button, rowIndex);
            Grid.SetColumn(button, 0);

            _grid.Children.Add(child);
            Grid.SetRow(child, rowIndex);
            Grid.SetColumn(child, 1);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e) {
            Button button = sender as Button;
            int columnIndex = Grid.GetRow(button);
            TagTuple tagTuple = button.Tag as TagTuple;
            _grid.Children.Remove(tagTuple.GridSplitter);
            _grid.Children.Remove(tagTuple.Child);
            _grid.Children.Remove(button as UIElement);

            _grid.RowDefinitions.RemoveAt(_grid.RowDefinitions.Count() - 1);
            _grid.RowDefinitions.RemoveAt(_grid.RowDefinitions.Count() - 1);

            foreach (UIElement child in _grid.Children) {
                int rowIndexForChild = Grid.GetRow(child);
                if (rowIndexForChild > columnIndex) {
                    Grid.SetRow(child, rowIndexForChild - 2);
                }
            }
        }

        private class TagTuple {
            public System.Windows.Controls.GridSplitter GridSplitter { get; set; }
            public UIElement Child { get; set; }
        }
    }
}
