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
            return input.FSTokenize().FSParse().FSAggregateFunction(new Dictionary<string, FastFunc<IEnumerable<double>, FastFunc<IEnumerable<double>, IEnumerable<double>>>>
                                                                        {
                                                                            {"Add", ToFastFunc<IEnumerable<double>>(FS_Functions.add)},
                                                                            {"Mul", ToFastFunc<IEnumerable<double>>(FS_Functions.mul)},
                                                                            {"Pow", ToFastFunc<IEnumerable<double>>(FS_Functions.pow)},
                                                                            {"Max", ToFastFunc<IEnumerable<double>>(FS_Functions.max)}
                                                                        })(_data).Take(3).ToArray();
        }
    }
}
