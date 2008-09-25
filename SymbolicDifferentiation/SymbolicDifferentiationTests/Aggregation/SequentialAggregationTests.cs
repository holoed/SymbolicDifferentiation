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

using System.Collections.Generic;
using System.Linq;
using Microsoft.FSharp.Core;
using SymbolicDifferentiation.Extensions;

namespace SymbolicDifferentiation.Tests.Aggregation
{
    public class SequentialAggregationTests : AggregationTests
    {
        protected override double[] Compute(string input)
        {
            return input.FSTokenize().FSParse().FSAggregateFunction(new Dictionary<string, FastFunc<IEnumerable<IEnumerable<double>>, IEnumerable<double>>>
                                                                        {
                                                                            {"Add", ToFastFunc<IEnumerable<double>>(args => FS_Functions.add(args.ElementAt(0), args.ElementAt(1)))},
                                                                            {"Mul", ToFastFunc<IEnumerable<double>>(args => FS_Functions.mul(args.ElementAt(0), args.ElementAt(1)))},
                                                                            {"Pow", ToFastFunc<IEnumerable<double>>(args => FS_Functions.pow(args.ElementAt(0), args.ElementAt(1)))},
                                                                            {"Max", ToFastFunc<IEnumerable<double>>(args => FS_Functions.max(args.ElementAt(0), args.ElementAt(1)))}
                                                                        })(_data).Take(3).ToArray();
        }
    }
}
