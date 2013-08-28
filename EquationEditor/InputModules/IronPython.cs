using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EquationEditor.InputModules {
    class IronPython : IInputModule {
        ScriptEngine engine = Python.CreateEngine();
        EquationEditor editor = new EquationEditor();
        public FrameworkElement Process(string input) {
            var result = engine.Execute(input);

            if (result != null) {
                if (input == result.ToString()) {
                    return Util.AsTextBlock(result.ToString());
                }
                var rhs = Util.AsTextBlock(result.ToString());
                try {
                    var lhs = editor.Process(input);
                    var equalSign = Util.AsTextBlock(" = ");
                    var sp = new StackPanel() {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    sp.Children.Add(lhs);
                    sp.Children.Add(equalSign);
                    sp.Children.Add(rhs);
                    return sp;
                } catch {
                    return Util.AsTextBlock(input + " = " + result.ToString());
                }

            }
            return null;
        }
    }
}
