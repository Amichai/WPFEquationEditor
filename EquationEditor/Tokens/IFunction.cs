using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationEditor.Tokens {
    interface IFunction {
        int NumberOfChildren { get; set; }
    }
}
