#region License

/* ****************************************************************************
 * Copyright (c) Edmondo Pentangelo. 
 *
 * This source code is subject to terms and conditions of the Microsoft Public License. 
 * A copy of the license can be found in the License.html file at the root of this distribution. 
 * By using this source code in any fashion, you are agreeing to be bound by the terms of the 
 * Microsoft Public License.
 *
 * You must not remove this notice, or any other, from this software.
 * ***************************************************************************/

#endregion

using System;
using System.Linq;
using System.Collections.Generic;
using SymbolicDifferentiation.Core.Extensions;

namespace SymbolicDifferentiation.Computation
{
    public static class ParallelFunctions
    {
        public static IEnumerable<KeyValuePair<string, double>> Add(IEnumerable<IEnumerable<KeyValuePair<string, double>>> input)
        {
            return input.Combine(item => item.Aggregate((x, y) => new KeyValuePair<string, double>(x.Key, x.Value + y.Value)));
        }

        public static IEnumerable<KeyValuePair<string, double>> Sub(IEnumerable<IEnumerable<KeyValuePair<string, double>>> input)
        {
            return input.Combine(item => item.Aggregate((x, y) => new KeyValuePair<string, double>(x.Key, x.Value - y.Value)));
        }

        public static IEnumerable<KeyValuePair<string, double>> Mul(IEnumerable<IEnumerable<KeyValuePair<string, double>>> input)
        {
            return input.Combine(item => item.Aggregate((x, y) => new KeyValuePair<string, double>(x.Key, x.Value * y.Value)));
        }

        public static IEnumerable<KeyValuePair<string, double>> Pow(IEnumerable<IEnumerable<KeyValuePair<string, double>>> input)
        {
            return input.Combine(item => item.Aggregate((x, y) => new KeyValuePair<string, double>(x.Key, Math.Pow(x.Value, y.Value))));
        }

        public static IEnumerable<KeyValuePair<string, double>> Max(IEnumerable<IEnumerable<KeyValuePair<string, double>>> input)
        {
            return input.Combine(item => item.Aggregate((x, y) => x.Value > y.Value ? x : y));
        }

        public static IEnumerable<KeyValuePair<string, double>> Div(IEnumerable<IEnumerable<KeyValuePair<string, double>>> input)
        {
            return input.Combine(item => item.Aggregate((x, y) => new KeyValuePair<string, double>(x.Key, x.Value / y.Value)));
        }

        public static IEnumerable<KeyValuePair<string, double>> Data(IEnumerable<KeyValuePair<string, double>> data)
        {
            return data;
        }

        public static IEnumerable<KeyValuePair<string, double>> GreaterThan(IEnumerable<IEnumerable<KeyValuePair<string, double>>> input)
        {
            return input.Combine(item => item.Aggregate((x, y) => new KeyValuePair<string, double>(x.Key, x.Value > y.Value ? 1 : -1)));
        }

        public static IEnumerable<KeyValuePair<string, double>> LessThan(IEnumerable<IEnumerable<KeyValuePair<string, double>>> input)
        {
            return input.Combine(item => item.Aggregate((x, y) => new KeyValuePair<string, double>(x.Key, x.Value < y.Value ? 1 : -1)));
        }
    }
}