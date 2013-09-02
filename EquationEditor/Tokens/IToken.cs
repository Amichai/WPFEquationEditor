using System;
using System.Windows;
namespace EquationEditor {
    public interface IToken {
        int NumberOfChildren { get; set; }
        TokenType Type { get; set; }
        string Value { get; set; }
        IToken Clone();
    }

    public enum TokenType { number, function, seperator, infixOperator, keyword}
}
