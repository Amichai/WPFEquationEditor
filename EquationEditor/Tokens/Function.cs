using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationEditor.Tokens {
    class Function : IToken, IFunction {
        public Function(string val) {
            this.Value = val;
            this.Type = TokenType.function;
            this.NumberOfChildren = 1;
        }
        public int NumberOfChildren { get; set; }

        public TokenType Type { get; set; }

        public string Value { get; set; }

        public IToken Clone() {
            return new Function(Value) { NumberOfChildren = this.NumberOfChildren };
        }
    }
}
