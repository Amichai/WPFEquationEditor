using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationEditor.Tokens {
    class Keyword : IToken {
        public Keyword(string val) {
            this.Value = val;
            this.Type = TokenType.keyword;
        }
        public int NumberOfChildren { get; set; }

        public TokenType Type { get; set; }

        public string Value { get; set; }

        public IToken Clone() {
            return new Keyword(Value) { NumberOfChildren = this.NumberOfChildren };
        }
    }
}
