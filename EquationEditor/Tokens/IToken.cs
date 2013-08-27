using System;
using System.Windows;
namespace EquationEditor {
    public interface IToken {
        int NumberOfChildren { get; set; }
        TokenType Type { get; set; }
        string Value { get; set; }
    }

    public enum TokenType { number, function, seperator, infixOperator, keyword}
}
