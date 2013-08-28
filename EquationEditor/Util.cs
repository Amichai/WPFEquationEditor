using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EquationEditor {
    class Util {
        public static FrameworkElement AsTextBlock(string text) {
            return new TextBlock() { Text = text, TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        }
    }
}
