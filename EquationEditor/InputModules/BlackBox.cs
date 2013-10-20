using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EquationEditor.InputModules {
    public class BlackBox : IInputModule{
        public FrameworkElement Process(string input) {
            if (input == "black box") {
                return new Rectangle() { Width = 220, Height = 40, Fill = Brushes.Blue, Margin = new Thickness(10,10,10,10) };
            } else if (input == "dake") {
                Image myImage3 = new Image();
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                string path = @"./Assets/dake.jpg";
                bi3.UriSource = new Uri(path, UriKind.Relative);
                bi3.EndInit();
                myImage3.Stretch = Stretch.Fill;
                myImage3.Source = bi3;
                myImage3.PreviewMouseDown += (s, e) => MessageBox.Show("Double leg");
                return myImage3;
            }
            throw new Exception();
        }


        public string ForHtml(string input) {
            throw new NotImplementedException();
        }
    }
}
