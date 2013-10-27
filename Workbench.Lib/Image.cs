using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Workbench.Lib {
    public class Image {
        public static System.Windows.Controls.Image Load(string url) {
            System.Windows.Controls.Image finalImage = new System.Windows.Controls.Image();
            finalImage.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(url);
            logo.EndInit();
            finalImage.Source = logo;
            return finalImage;
        }

    }
}
