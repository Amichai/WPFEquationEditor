using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EquationEditor.InputModules {
    class BlackBox : IInputModule{
        public FrameworkElement Process(string input) {
            if (input == "black box") {
                return new Rectangle() { Width = 220, Height = 40, Fill = Brushes.Blue, Margin = new Thickness(10,10,10,10) };
            }
            return null;
        }
    }
}
