using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Workbench.Lib {
    public class Help {
        public static string InstanceMethods(Type t) {
            MethodInfo[] methodInfos = t.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            return methodInfoString(methodInfos);
        }

        public static string InstanceMethods(object o) {
            return InstanceMethods(o.GetType());
        }

        public static string StaticMethods(object o) {
            return StaticMethods(o.GetType());
        }

        private static string methodInfoString(MethodInfo[] info) {
            ///TODO: use a string builder
            string output = "";
            foreach (var m in info) {
                output += m.ReturnType.Name + " " + m.Name + " " + string.Concat(m.GetParameters().Select(j => j.Name + " ")) + "\n";
            }
            return output.TrimEnd('\n');
        }

        public static string StaticMethods(Type t) {
            MethodInfo[] methodInfos = t.GetMethods(BindingFlags.Public | BindingFlags.Static);
            return methodInfoString(methodInfos);
        }
    }
}
