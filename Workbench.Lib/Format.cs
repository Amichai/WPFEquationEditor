using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Workbench.Lib {
    public class Format {
        public static UIElement Equation(string input){
            EquationEditor.InputModules.EquationEditor module = new EquationEditor.InputModules.EquationEditor();
            var result = module.Process(input);
            result.Height = double.NaN;
            return result;
        }
    }
}
