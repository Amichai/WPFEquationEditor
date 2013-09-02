using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace EquationEditor.InputModules {
    class XamlParser : IInputModule {
        public FrameworkElement Process(string input) {
            StringReader sr = new StringReader(input);
            XmlReader reader = XmlReader.Create(sr);
            return XamlReader.Load(reader) as FrameworkElement;
        }
    }
}
