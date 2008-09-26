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

namespace SymbolicDifferentiation.Parallel
{
    //TODO: Use PLINQ to parallelize functions
    public static class ParallelFunctions
    {
        public static IEnumerable<double> Add(IEnumerable<IEnumerable<double>> data)
        {
            return data.Combine(item => item.Aggregate((x, y) => x + y));
        }

        public static IEnumerable<double> Mul(IEnumerable<IEnumerable<double>> data)
        {
            return data.Combine(item => item.Aggregate((x, y) => x * y));
        }

        public static IEnumerable<double> Pow(IEnumerable<IEnumerable<double>> data)
        {
            return data.Combine(item => item.Aggregate(Math.Pow));
        }

        public static IEnumerable<double> Max(IEnumerable<IEnumerable<double>> data)
        {
            return data.Combine(item => item.Aggregate((x, y) => x > y ? x : y));
        }
    }
}
