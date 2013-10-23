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

        private List<FrameworkElement> elements = new List<FrameworkElement>();

        private void Rebuild() {
            _grid.RowDefinitions.Clear();
            _grid.Children.Clear();

            int elementCount = elements.Count();
            for (int i = 0; i < elementCount * 2; i += 2) {
                var e = elements.ElementAt(i / 2);
                addControl(i, e);
            }
            var toAdd = new Grid();
            addControl(elementCount * 2, toAdd);
        }

        private void addControl(int i, FrameworkElement child) {
            _grid.RowDefinitions.Add(new RowDefinition() {
                Height = new GridLength(5)
            });

            GridLength length;
            double height = child.ActualHeight;
            if (height == 0 || double.IsNaN(height)) {
                length = GridLength.Auto;
            } else {
                length = new GridLength(height);
            }
            _grid.RowDefinitions.Add(new RowDefinition() {
                Height = length
            });


            System.Windows.Controls.GridSplitter gridSplitter =
            new System.Windows.Controls.GridSplitter() {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                ResizeDirection = GridResizeDirection.Rows,
                Background = Brushes.Black,
                Tag = child
            };

            _grid.Children.Add(gridSplitter);
            Grid.SetRow(gridSplitter, i);
            Grid.SetColumn(gridSplitter, 0);
            Grid.SetColumnSpan(gridSplitter, 2);

            _grid.Children.Add(child);
            Grid.SetRow(child, i + 1);
            Grid.SetColumn(child, 1);
        }

        public void Add(FrameworkElement child) {
            elements.Add(child);
            this.Rebuild();
        }
    }
}
