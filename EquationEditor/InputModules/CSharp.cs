using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EquationEditor.InputModules {
    public class CSharp : IInputModule {
        public System.Windows.FrameworkElement Process(string input) {
            throw new NotImplementedException();
        }

        public static string LastResult { get; set; }

        public string ForHtml(string input) {
            string code = "using System; " +
"using System.IO;" +
"using System.Linq;" +
"using System.Diagnostics; " +
"namespace DynaCore" +
"{ " +
"   public class DynaCore " +
"   { " +
"       static public string Main(string str) " +
"       { " +
"string _ = \"" + LastResult + "\"; " +
                //"string last = \"testing123\"; " +
"return (" + input + ").ToString();" +
"       } " +
"   } " +
" }";
            return CompileAndRun(code);
        }


        string ExpoloreAssembly(Assembly assembly) {
            string result = "";
            result += "Modules in the assembly:";
            foreach (Module m in assembly.GetModules()) {
                result += m;

                foreach (Type t in m.GetTypes()) {
                    result += t.Name;

                    foreach (MethodInfo mi in t.GetMethods()) {
                        result += mi.Name;
                    }
                }
            }
            return result;
        }

        CompilerParameters CompilerParams;
        string outputDirectory;
        string[] references = { "System.dll", "System.Core.dll" };
        CSharpCodeProvider provider;

        public CSharp() {
            this.CompilerParams = new CompilerParameters();
            this.outputDirectory = Directory.GetCurrentDirectory();
            CompilerParams.GenerateInMemory = true;
            CompilerParams.TreatWarningsAsErrors = false;
            CompilerParams.GenerateExecutable = false;
            CompilerParams.CompilerOptions = "/optimize";
            CompilerParams.ReferencedAssemblies.AddRange(references);
            provider = new CSharpCodeProvider();
        }

        string CompileAndRun(string code) {
            CompilerResults compile = provider.CompileAssemblyFromSource(CompilerParams, code);

            if (compile.Errors.HasErrors) {
                string text = "Compile error: ";
                foreach (CompilerError ce in compile.Errors) {
                    text += "rn" + ce.ToString();
                }
                throw new Exception(text);
            }

            //ExpoloreAssembly(compile.CompiledAssembly);

            Module module = compile.CompiledAssembly.GetModules()[0];
            Type mt = null;
            MethodInfo methInfo = null;

            if (module != null) {
                mt = module.GetType("DynaCore.DynaCore");
            }

            if (mt != null) {
                methInfo = mt.GetMethod("Main");
            }

            if (methInfo != null) {
                var result = methInfo.Invoke(null, new object[] { "here in dyna code" });
                return result.ToString();
            }
            throw new Exception("Failed to find and execute the Main() method in JsonQuery");
        }
    }
}
