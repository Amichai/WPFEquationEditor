using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workbench.Lib {
    public class Vec2 {
        private Vec2() { }
        public bool DefaultToDegrees = false;
        public static Vec2 MagnitudeAngle(double mag, double theta, bool degrees = false) {
            
            if (degrees) {
                theta *= Math.PI / 180;
                
            }
            Vec2 toReturn = new Vec2() {
                X = mag * Math.Cos(theta),
                Y = mag * Math.Sin(theta)
            };
            if (degrees) {
                toReturn.DefaultToDegrees = true;
            }

            return toReturn;
        }   

        public double Mag {
            get {
                return Math.Sqrt(X * X + Y * Y);
            }
        }

        public double Angle(bool degrees = false) {
            var result = Math.Atan2(X, Y);
            if (degrees || this.DefaultToDegrees) {
                return ExtensionMath.RadianToDegree(result);
            } else {
                return result;
            }
        }

        public static Vec2 FromComponents(double x, double y) {
            return new Vec2() { X = x, Y = y };
        }
        public double X { get; set; }
        public double Y { get; set; }
    }
}
