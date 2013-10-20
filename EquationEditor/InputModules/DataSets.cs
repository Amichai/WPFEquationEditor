using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using EquationEditor;
using Newtonsoft.Json.Linq;

namespace EquationEditor.InputModules {
    public class DataSets : IInputModule {
        public FrameworkElement Process(string input) {
            throw new NotImplementedException();
        }

        Dictionary<string, string> sources = new Dictionary<string, string>() {
            { "GRAVESITES", "https://explore.data.gov/resource/veterans-burial-sites.json?" },

        };

        public string ForHtml(string input) {
            var source = input.Split(':').First().ToUpper();
            var baseurl = sources[source];
            var data = baseurl.GetContent();
            return data;
        }
    }
}
