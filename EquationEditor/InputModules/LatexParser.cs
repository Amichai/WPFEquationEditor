using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Web;

namespace EquationEditor.InputModules {
    public class LatexParser : IInputModule {
        public FrameworkElement Process(string input) {
            ///Find keywords
            ///subscripts and superscripts
            //string path = @"http://latex.codecogs.com/gif.latex?\forall&space;x&space;\in&space;X,&space;\quad&space;\exists&space;y\leq\epsilon&space;t";
            var url = stringToUrl(input);
            return imageFromUrl(url);
            
        }

        public string stringToUrl(string s) {
            return @"http://latex.codecogs.com/gif.latex?\LARGE&space;" + s;
        }

        private Image imageFromUrl(string url) {
            Image myImage = new Image();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(url);
            bi.EndInit();
            myImage.Stretch = Stretch.None; 
            myImage.Source = bi;
            myImage.Margin = new Thickness(10,10,10,10);
            return myImage;
        }


        public string ForHtml(string input) {
            string url = stringToUrl(input);
            url = System.Uri.EscapeUriString(url);
            string toReturn = @"<img src=" + url + "></img>";
            return toReturn;
        }
    }
}
