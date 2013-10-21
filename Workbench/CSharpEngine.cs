using Newtonsoft.Json.Linq;
using Roslyn.Compilers;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Workbench.Lib;

namespace Workbench {
    public class CSharpEngine {
        public CSharpEngine() {
            this.loadScriptEngine();
        }

        private static Session session;
        private static ScriptEngine engine;
        private void loadScriptEngine() {
            engine = new ScriptEngine();
            engine.AddReference(typeof(System.Linq.Enumerable).Assembly.Location);
            engine.AddReference(typeof(Url).Assembly.Location);
            engine.AddReference(typeof(JObject).Assembly.Location);
            engine.AddReference(typeof(XElement).Assembly.Location);
            engine.AddReference(typeof(UIElement).Assembly.Location);
            engine.AddReference(typeof(DependencyObject).Assembly.Location);

            //engine.AddReference(typeof(FunctionLibrary).Assembly.Location);

            ///Untested
            //engine.AddReference(typeof(Uri).Assembly.Location);
            //engine.AddReference(typeof(XmlAttribute).Assembly.Location);

            engine.AddReference(new MetadataFileReference(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.dll"));
            engine.AddReference(new MetadataFileReference(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Xml.dll"));


            engine.ImportNamespace("System");
            engine.ImportNamespace("System.Windows");
            engine.ImportNamespace("System.Collections.Generic");
            engine.ImportNamespace("System.Linq");
            engine.ImportNamespace("System.Text");
            engine.ImportNamespace("System.Diagnostics");
            engine.ImportNamespace("Newtonsoft.Json.Linq");
            engine.ImportNamespace("System.Xml.Linq");
            engine.ImportNamespace("Workbench.Lib");

            session = engine.CreateSession(this);

        }

        public void CSharpAssign(string inputText, string result, int lineNumber) {
            string lastValName = "_" + lineNumber.ToString();
            try {
                session.Execute(@"var " + lastValName + " = " + inputText + ";");
                session.Execute(@"var _" + " = " + lastValName + ";");

            } catch {
                var escapedString = result.Replace("\"", "\"\"");
                var assign = "var " + lastValName + " = @\"" + escapedString + "\";";
                session.Execute(assign);
            }
        }

        public void CSharpAssign(string result, int lineNumber) {
            if (result == null) {
                return;
            }
            string lastValName = "_" + lineNumber.ToString();
            var escapedString = result.Replace("\"", "\"\"");
            var assign = "var " + lastValName + " = @\"" + escapedString + "\";";
            session.Execute(assign);
            session.Execute("var _ = " + lastValName + ";");
        }

        public object AppendCSharp(string inputText, int lineNumber) {
            if (string.IsNullOrWhiteSpace(inputText)) {
                return "";
            }
            try {
                if (session == null) {
                    return "No C# session available.";
                }
                var result = session.Execute(inputText);
                if (result == null) {
                    return "";
                }
                CSharpAssign(inputText, result.ToString(), lineNumber);
                if (inputText.Last() == ';') {
                    return "";
                }
                return result;
            } catch (Exception ex) {
                return ex.Message;
            }
        }
    }
}
