﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationEditor.Tokens {
    class Function : IToken, IFunction {
        public Function(string val, int children) {
            this.Value = val;
            this.Type = TokenType.function;
        }
        public int NumberOfChildren { get; set; }

        public TokenType Type { get; set; }

        public string Value { get; set; }
    }
}