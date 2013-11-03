using IronPython;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workbench.Lib {
    public class PythonEngine {
        private static ScriptScope scope;
        public static PythonEngine Instance = new PythonEngine();
        private static ScriptEngine engine;
        public object Execute(string input) {
            return engine.Execute(input, scope);
        }

        private PythonEngine() {
            var options = new Dictionary<string, object>();
            options["Frames"] = true;
            options["FullFrames"] = true;
            options["DivisionOptions"] = PythonDivisionOptions.New;
            engine = Python.CreateEngine(options);
            scope = engine.CreateScope();

            string importScript = @"
import sys

sys.path.append(r'c:\Program Files (x86)\IronPython 2.7')
sys.path.append(r'c:\Program Files (x86)\IronPython 2.7\DLLs')
sys.path.append(r'c:\Program Files (x86)\IronPython 2.7\Lib')
sys.path.append(r'c:\Program Files (x86)\IronPython 2.7\Lib\site-packages')


sys.path.append(r'c:\Python27\DLLs')
sys.path.append(r'c:\Python27\Lib')
sys.path.append(r'c:\Python27\Lib\site-packages')

";
//sys.path.append(r'c:\Windows\system32\python27.zip')
//sys.path.append(r'C:\Python27\lib\plat-win')
//sys.path.append(r'C:\Python27\lib\lib-tk')
//from sympy import *
//init_printing(use_unicode=False, wrap_line=False, no_global=True)
//x = Symbol('x')

            engine.Execute(importScript, scope);
            engine.ImportModule("sympy");
            //            //scope.ImportModule("math");
            //scope.ImportModule("scipy");
        }
    }
}
