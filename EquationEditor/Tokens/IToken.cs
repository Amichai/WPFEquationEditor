using System;
using System.Windows;
namespace EquationEditor {
    public interface IToken {
        int Children { get; set; }
        TokenType Type { get; set; }
        string Value { get; set; }
       // FrameworkElement GetElement();
    }

    public enum TokenType { number, function, seperator, infixOperator, keyword}
}
