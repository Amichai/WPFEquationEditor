using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Workbench.Lib
{
    public class Data {
        public static string Format(string content) {
            return XDocument.Parse(content).ToString();
        }
    }

    public class Url
    {
        public static string Load(string url) {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            var response = req.GetResponse();
            var streamResponse = response.GetResponseStream();

            StreamReader streamRead = new StreamReader(streamResponse);
            char[] readBuffer = new char[256];
            int count = streamRead.Read(readBuffer, 0, 256);
            string content = "";
            while (count > 0) {
                string outputData = new string(readBuffer, 0, count);
                content += outputData;
                count = streamRead.Read(readBuffer, 0, 256);
            }
            streamRead.Close();
            streamResponse.Close();
            // Release the response object resources.
            streamResponse.Close();
            return content;
        }
    }
}
