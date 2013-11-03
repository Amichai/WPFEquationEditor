using IronPython;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonTester {
    class Program {

        private static ScriptScope scope;
        private static ScriptEngine engine;

        static void Main(string[] args) {

            var options = new Dictionary<string, object>();
            options["Frames"] = true;
            options["FullFrames"] = true;
            options["DivisionOptions"] = PythonDivisionOptions.New;
            engine = Python.CreateEngine(options);
            scope = engine.CreateScope();

//sys.path.append(r'c:\Windows\system32\python27.zip')
            string importScript = @"
import sys


sys.path.append(r'c:\Program Files (x86)\IronPython 2.7')
sys.path.append(r'c:\Program Files (x86)\IronPython 2.7\DLLs')
sys.path.append(r'c:\Program Files (x86)\IronPython 2.7\Lib')
sys.path.append(r'c:\Program Files (x86)\IronPython 2.7\Lib\site-packages')
sys.path.append(r'c:\Python27\DLLs')
sys.path.append(r'c:\Python27\Lib')
sys.path.append(r'c:\Python27\Lib\site-packages')

import clr
clr.AddReference('mtrand.dll')
from scipy import *
";
//from sympy import *
//sys.platform = 'win32'
//sys.path.append(r'C:\Python27\lib\plat-win')
//sys.path.append(r'C:\Python27\lib\lib-tk')
//init_printing(use_unicode=False, wrap_line=False, no_global=True)
//x = Symbol('x')

            engine.Execute(importScript, scope);
            scope.ImportModule("math");
            scope.ImportModule("numpy");
            scope.ImportModule("scipy");
            //scope.ImportModule("sympy");


            var result = engine.Execute("math.isinf(3.4)", scope);
            Debug.Print(result.ToString() as string);
            result = engine.Execute("array([1, 2, 3, 0, 4])", scope);
            Debug.Print(result.ToString() as string);
        }
    }
}
