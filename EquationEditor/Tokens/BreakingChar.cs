using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationEditor.Tokens {
    class BreakingChar : IToken{
        public BreakingChar(string val) {
            this.Value = val;
            this.Type = TokenType.seperator;
        }
        public int NumberOfChildren { get; set; }

        public TokenType Type { get; set; }

        public string Value { get; set; }

        public IToken Clone() {
            return new BreakingChar(Value) { NumberOfChildren = this.NumberOfChildren };
        }
    }
}
