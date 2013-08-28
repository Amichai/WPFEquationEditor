using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EquationEditor.InputModules {
    class IronPython : IInputModule {
        ScriptEngine engine = Python.CreateEngine();

        public FrameworkElement Process(string input) {
            var result = engine.Execute(input);

            if (result != null) {
                return Util.AsTextBlock(result.ToString());
            }
            return null;
        }
    }
}
