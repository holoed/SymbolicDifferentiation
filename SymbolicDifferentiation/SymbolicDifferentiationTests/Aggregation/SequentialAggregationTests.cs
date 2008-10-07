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

namespace SymbolicDifferentiation.Tests.Aggregation
{
    public class SequentialAggregationTests : AggregationTests
    {
        protected override IDictionary<string, Func<IEnumerable<IEnumerable<KeyValuePair<string, double>>>, IEnumerable<KeyValuePair<string, double>>>> Compute(string input)
        {
            return ComputeSequential(input, 3);
        }
    }
}
