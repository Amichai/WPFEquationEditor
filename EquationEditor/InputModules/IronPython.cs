using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

///TODO: Implement an IComposable interface that automatically 
///applies combines two modules
namespace EquationEditor.InputModules {
    class IronPython : IInputModule {
        ScriptScope scope;
        public IronPython() {
            scope = engine.CreateScope();
        }
        ScriptEngine engine = Python.CreateEngine();
        public FrameworkElement Process(string input) {
            var result = engine.Execute(input, scope);
            if (result != null) {
                engine.Execute("ans = " + result, scope);
                return Util.AsTextBlock(result.ToString());
            }
            return null;
        }
    }
}
