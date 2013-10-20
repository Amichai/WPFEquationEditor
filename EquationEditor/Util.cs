using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EquationEditor {
    public static class Util {
        public static FrameworkElement AsTextBlock(string text, double fontSize, HorizontalAlignment alignment = HorizontalAlignment.Center, TextAlignment textAlignment = TextAlignment.Center) {
            return new TextBlock() { Text = text, TextAlignment = textAlignment, 
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                TextWrapping = System.Windows.TextWrapping.Wrap,
                HorizontalAlignment = alignment, FontSize = fontSize };
        }

        public static string GetContent(this string url) {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            var response = req.GetResponse();
            var streamResponse = response.GetResponseStream();

            StreamReader streamRead = new StreamReader(streamResponse);
            char[] readBuffer = new char[256];
            int count = streamRead.Read(readBuffer, 0, 256);
            string content = "";
            while (count > 0) {
                string outputData = new string(readBuffer, 0, count);
                content += outputData;
                count = streamRead.Read(readBuffer, 0, 256);
            }
            streamRead.Close();
            streamResponse.Close();
            // Release the response object resources.
            streamResponse.Close();
            return content;
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
