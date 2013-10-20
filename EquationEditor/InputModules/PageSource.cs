using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationEditor.InputModules {
    public class PageSource : IInputModule {
        public System.Windows.FrameworkElement Process(string input) {
            throw new NotImplementedException();
        }

        public string ForHtml(string input) {
            var data = input.GetContent();
            return data;
        }
    }
}
