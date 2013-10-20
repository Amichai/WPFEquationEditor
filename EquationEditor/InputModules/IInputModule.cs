﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EquationEditor {
    public interface IInputModule {
        FrameworkElement Process(string input);
        string ForHtml(string input);
    }
}
