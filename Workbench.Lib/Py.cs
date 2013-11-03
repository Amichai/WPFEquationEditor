using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workbench.Lib {
    public class Py {
        public static object Compute(string input) {
            return PythonEngine.Instance.Execute(input);
        }
    }
}
