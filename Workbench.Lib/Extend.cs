using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;

namespace Workbench.Lib {

    public static class Extend {
        ///TODO: move this class to the workbench project so that we have access to the 
        ///scripting engine element and we can exectue the code to a) test the syntax, 
        ///b) give access to the method in the current document
        public static string Method(string className, string returnType, string signature, string body) {
            string toAppend = @"
namespace Workbench.Lib {
    public partial class " + className + " { public static "
                          + returnType + " " + signature + " { return " + body.TrimEnd(';') + "; } } }";
            string xmlFilepath = @"..\..\..\Workbench.Lib\UserDefinedMethods.xml";
            XElement methods = XElement.Load(xmlFilepath);
            if (methods.Elements("Method").Any(i => i.Attribute("className").Value == className
                && i.Attribute("signature").Value == signature)) {
                return "A method with this name already exists";
            }
            
            var root = new XElement("Method");
            root.Add(new XAttribute("className", className));
            root.Add(new XAttribute("returnType", returnType));
            root.Add(new XAttribute("signature", signature));
            root.Add(new XAttribute("body", body));
            methods.Add(root);
            methods.Save(xmlFilepath);


            System.IO.File.AppendAllText(@"..\..\..\Workbench.Lib\UserDefined.cs",
                toAppend);

            return "Success";
        }
    }
}
