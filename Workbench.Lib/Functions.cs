﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workbench.Lib {
    public static class Functions {
        //public static JObject Eval(this Func<double, double> inputFunction, double x0, double xf, double dx = 0) {
        //    if (dx == 0) {
        //        dx = (xf - x0) / 100.0;
        //    }
        //    List<Tuple<double, double>> xyVals = new List<Tuple<double, double>>();
        //    for (double x = x0; x < xf; x += dx) {
        //        xyVals.Add(new Tuple<double, double>(x, inputFunction(x)));
        //    }
        //    var j = JArray.FromObject(xyVals);
        //    var root = new JObject(new JProperty("Chart", j));
        //    return root;
        //}

        /// <summary>
        /// Finds the input value of a function such that the output is zero with the range of min and max
        /// </summary>
        /// <param name="function">The function to zero</param>
        /// <param name="min">The lower bound of the "zero search"</param>
        /// <param name="max">The upper bound of the "zero search"</param>
        /// <param name="counter">The amount of iterations to convergence</param>
        /// <param name="eps">Desired precision</param>
        /// <returns></returns>
        public static double Zero(this Func<double, double> function, double min, double max, out int counter, double eps = .001) {
            double lowerBound = min,
                upperBound = max;
            counter = 0;
            double range = long.MaxValue;
            double tryIndex = long.MinValue;
            double tryEval = double.MinValue;
            while (Math.Abs(tryEval) > eps /*&& range > 1.0e-9 */&& counter < 10000) {
                counter++;
                range = upperBound - lowerBound;
                double maxEval = function(upperBound);
                double minEval = function(lowerBound);
                bool signMax = (maxEval > 0);
                bool signMin = (minEval > 0);
                if (signMax == signMin) {
                    return double.NaN;
                }
                tryIndex = lowerBound + (range / 2);
                tryEval = function(tryIndex);
                bool trySign = tryEval > 0;
                if (trySign == signMax) {
                    upperBound = tryIndex;
                } else lowerBound = tryIndex;
            }
            return tryIndex;
        }

        public static double Zero(this Func<double, double> function, double min, double max, int maxIter = 10000, double eps = .001) {
            double lowerBound = min,
                upperBound = max;
            int counter = 0;
            double range = long.MaxValue;
            double tryIndex = long.MinValue;
            double tryEval = double.MinValue;
            while (Math.Abs(tryEval) > eps /*&& range > 1.0e-9 */&& counter < maxIter) {
                counter++;
                range = upperBound - lowerBound;
                double maxEval = function(upperBound);
                double minEval = function(lowerBound);
                bool signMax = (maxEval > 0);
                bool signMin = (minEval > 0);
                if (signMax == signMin) {
                    return double.NaN;

                }
                tryIndex = lowerBound + (range / 2);
                tryEval = function(tryIndex);
                bool trySign = tryEval > 0;
                if (trySign == signMax) {
                    upperBound = tryIndex;
                } else lowerBound = tryIndex;
            }
            return tryIndex;
        }

        public static double Derivative(this Func<double, double> function, double at, double epsilon = .01) {
            return (function(at + epsilon / 2) - function(at - epsilon / 2)) / epsilon;
        }

        public static Func<double, double> Derivative(this Func<double, double> function, double eps = .01) {
            return i => function.Derivative(i, eps);
        }

        public static Func<double, double> Inverse(this Func<double, double> function, double min, double max, double eps = .01) {
            Func<double, double> toReturn;
            toReturn = val => {
                Func<double, double> toZero = i => function(i) - val;
                return toZero.Zero(min, max, 10000, .001);
            };
            return toReturn;
        }

        public static Func<double, double> ToFuncDoubleDouble(this Delegate function, params object[] parameters) {
            Func<double, double> result = i => {
                var parametersToPass = parameters.ToList();
                parametersToPass.Add(i);
                return (double)function.DynamicInvoke((object[])parametersToPass.ToArray());
            };
            return result;
        }

        public static Func<T, U> ToFunc<T, U>(this Delegate function, params object[] parameters) {
            Func<T, U> result = i => {
                var parametersToPass = parameters.ToList();
                parametersToPass.Add(i);
                return (U)function.DynamicInvoke((object[])parametersToPass.ToArray());
            };
            return result;
        }

        public static Func<T, U> ToFunc<T, U>(int index, Delegate function, params object[] parameters) {
            Func<T, U> result = i => {
                var parametersToPass = parameters.ToList();
                parametersToPass.Insert(index, i);
                return (U)function.DynamicInvoke((object[])parametersToPass.ToArray());
            };
            return result;
        }

        public static Func<double, double> ToFuncDoubleDouble(int inputIndex, Delegate function, params object[] parameters) {
            Func<double, double> result = i => {
                var parametersToPass = parameters.ToList();
                parametersToPass.Insert(inputIndex, i);
                return (double)function.DynamicInvoke((object[])parametersToPass.ToArray());
            };
            return result;
        }

        public static double Inverse(this Func<double, double> function, double val, double min, double max, double eps = .01) {
            Func<double, double> toZero = i => function(i) - val;
            return toZero.Zero(min, max, 10000, .001);
        }
    }

    ///TODO: implement numerical mathematics using sympy
}
