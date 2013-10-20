using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using IronPython;
using Microsoft.Scripting;
using System.Diagnostics;


///TODO: Implement an IComposable interface that automatically 
///applies combines two modules
namespace EquationEditor.InputModules {
    public class IronPython : IInputModule {
        ScriptScope scope;
        public IronPython() {

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

import clr
clr.AddReference('mtrand.dll')

import scipy
import numpy
from numpy import *
";

            engine.Execute(importScript, scope);
//            //scope.ImportModule("math");
            //scope.ImportModule("scipy");
        }
        ///TODO: we shouldn't imply an error if we just wanted to define a variable

        ScriptEngine engine;
        public FrameworkElement Process(string input) {
            
            dynamic result;

            //ScriptSource source = engine.CreateScriptSourceFromString(input, SourceCodeKind.AutoDetect);
            //result = source.Execute(scope);

            if (!scope.TryGetVariable(input, out result)) {
                result = engine.Execute(input, scope);
            }

            if (result != null) {
                //engine.Execute("_ = " + result, scope);
                return Util.AsTextBlock(result.ToString(), WorkbenchStack.SelectedFontSize);
            }
            return null;
        }


        public string ForHtml(string input) {
            dynamic result;

            if (!scope.TryGetVariable(input, out result)) {
                result = engine.Execute(input, scope);
            }

            if (result != null) {
                try {
                    engine.Execute("_ = " + result, scope);
                } catch {
                    
                }
                return result.ToString();
            }
            return null;
        }
    }
}
