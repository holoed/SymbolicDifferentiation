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
using System.Collections.Generic;

namespace SymbolicDifferentiation.Parallel
{
    //TODO: Use PLINQ to parallelize functions
    public static class ParallelFunctions
    {
        public static IEnumerable<double> Add(IEnumerable<double> left, IEnumerable<double> right)
        {
            return Combine(left, right, (x,y) => x + y);
        }

        public static IEnumerable<double> Mul(IEnumerable<double> left, IEnumerable<double> right)
        {
            return Combine(left, right, (x, y) => x * y);
        }

        public static IEnumerable<double> Pow(IEnumerable<double> left, IEnumerable<double> right)
        {
            return Combine(left, right, Math.Pow);
        }

        public static IEnumerable<double> Max(IEnumerable<double> left, IEnumerable<double> right)
        {
            return Combine(left, right, (x, y) => x > y ? x : y);
        }

        private static IEnumerable<double> Combine(IEnumerable<double> left, IEnumerable<double> right, Func<double,double,double> func)
        {
            using (var e1 = left.GetEnumerator())
            using (var e2 = right.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext())
                {
                    yield return func(e1.Current, e2.Current);
                }
            }
        }
    }
}
