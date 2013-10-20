using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EquationEditor.InputModules {
    public class Charting : IInputModule {
        ScriptEngine engine;
        ScriptScope scope;
        public Charting() {
            engine = Python.CreateEngine();
            scope = engine.CreateScope();


        }



        public FrameworkElement Process(string input) {
            engine.Execute(input, scope);
            // First approach: get the DLR to build us a delegate from the function
            Func<double, double> function;
            if (!scope.TryGetVariable<Func<double, double>>("f", out function)) {
                throw new Exception();
            }
            // An alternative approach, using C# 4's dynamic type
            // dynamic function;
            // if (!scope.TryGetVariable("f", out function)) ...
            // Then use dynamic typing from C#.
            double maxInputX = 10;
            double minInputX = -10;
            double step = (maxInputX - minInputX) / 100;
            for (int i = 0; i < 101; i++) {
                //values[i] = function(minInputX + step * i);
            }
            throw new NotImplementedException();
        }


        public string ForHtml(string input) {
            throw new NotImplementedException();
        }
    }
}
