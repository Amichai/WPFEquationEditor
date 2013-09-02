using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EquationEditor {
    class Util {
        public static FrameworkElement AsTextBlock(string text, HorizontalAlignment alignment = HorizontalAlignment.Center) {
            return new TextBlock() { Text = text, TextAlignment = TextAlignment.Center, 
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                TextWrapping = System.Windows.TextWrapping.Wrap,
                HorizontalAlignment = alignment};
        }

        /*
         static void CompileAndRun(string[] code) {
            CompilerParameters CompilerParams = new CompilerParameters();

            CompilerParams.GenerateInMemory = true;
            CompilerParams.TreatWarningsAsErrors = false;
            CompilerParams.GenerateExecutable = false;
            CompilerParams.CompilerOptions = "/optimize";

            string[] references = { "System.dll" };

            CompilerParams.ReferencedAssemblies.AddRange(references);

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerResults compile = provider.CompileAssemblyFromSource(CompilerParams, code);

            if (compile.Errors.HasErrors) {
                string text = "Compile error: ";
                foreach (CompilerError ce in compile.Errors) {
                    text += "rn" + ce.ToString();
                }
                throw new Exception(text);
            }

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
                Console.WriteLine(methInfo.Invoke(null, new object[] { "here in dyna code" }));
            }
        }
         */ 
    }
}
