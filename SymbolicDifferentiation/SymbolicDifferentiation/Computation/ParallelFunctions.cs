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

using System.Linq;
using System.Collections.Generic;
using SymbolicDifferentiation.Core.Computation;
using SymbolicDifferentiation.Core.Extensions;

namespace SymbolicDifferentiation.Computation
{
    public static class ParallelFunctions
    {
        public static IEnumerable<KeyValuePair<string, Atom>> Add(IEnumerable<IEnumerable<KeyValuePair<string, Atom>>> input)
        {
            return input.Combine(item => item.Aggregate((x, y) => new KeyValuePair<string, Atom>(x.Key, x.Value + y.Value)));
        }

        public static IEnumerable<KeyValuePair<string, Atom>> Sub(IEnumerable<IEnumerable<KeyValuePair<string, Atom>>> input)
        {
            return input.Combine(item => item.Aggregate((x, y) => new KeyValuePair<string, Atom>(x.Key, x.Value - y.Value)));
        }

        public static IEnumerable<KeyValuePair<string, Atom>> Mul(IEnumerable<IEnumerable<KeyValuePair<string, Atom>>> input)
        {
            return input.Combine(item => item.Aggregate((x, y) => new KeyValuePair<string, Atom>(x.Key, x.Value * y.Value)));
        }

        public static IEnumerable<KeyValuePair<string, Atom>> Pow(IEnumerable<IEnumerable<KeyValuePair<string, Atom>>> input)
        {
            return input.Combine(item => item.Aggregate((x, y) => new KeyValuePair<string, Atom>(x.Key, x.Value ^ y.Value)));
        }

        public static IEnumerable<KeyValuePair<string, Atom>> Max(IEnumerable<IEnumerable<KeyValuePair<string, Atom>>> input)
        {
            return input.Combine(item => item.Aggregate((x, y) => x.Value > y.Value ? x : y));
        }

        public static IEnumerable<KeyValuePair<string, Atom>> Div(IEnumerable<IEnumerable<KeyValuePair<string, Atom>>> input)
        {
            return input.Combine(item => item.Aggregate((x, y) => new KeyValuePair<string, Atom>(x.Key, x.Value / y.Value)));
        }

        public static IEnumerable<KeyValuePair<string, Atom>> Data(IEnumerable<KeyValuePair<string, Atom>> data)
        {
            return data;
        }

        public static IEnumerable<KeyValuePair<string, Atom>> GreaterThan(IEnumerable<IEnumerable<KeyValuePair<string, Atom>>> input)
        {
            return input.Combine(item => item.Aggregate((x, y) => new KeyValuePair<string, Atom>(x.Key, x.Value > y.Value)));
        }

        public static IEnumerable<KeyValuePair<string, Atom>> LessThan(IEnumerable<IEnumerable<KeyValuePair<string, Atom>>> input)
        {
            return input.Combine(item => item.Aggregate((x, y) => new KeyValuePair<string, Atom>(x.Key, x.Value < y.Value)));
        }
    }
}