using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workbench.Lib {
    public static class ExtensionMath {
        public static double Sqrd(this double val) {
            return val * val;
        }

        public static double RadianToDegree(double val) {
            return val * 180 / Math.PI;
        }

        public static double DegreeToRadian(double val) {
            return val * Math.PI / 180;
        }
    }
}
