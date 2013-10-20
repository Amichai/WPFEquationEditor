using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EquationEditor.InputModules {
    public class EquationEditor : IInputModule {
        public FrameworkElement Process(string input) {
            ParseTree tree = new ParseTree();
            Tokenizer tokenizer = new Tokenizer();
            var queue = tokenizer.Tokenize(input);
            tree.BuildTree(queue);
            return tree.Root.GetElement();
        }


        public string ForHtml(string input) {
            throw new NotImplementedException();
        }
    }
}
