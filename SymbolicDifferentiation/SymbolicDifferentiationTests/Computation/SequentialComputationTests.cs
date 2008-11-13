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
using SymbolicDifferentiation.Core.Computation;

namespace SymbolicDifferentiation.Tests.Computation
{
    public class SequentialComputationTests : ComputationTests
    {
        protected override IDictionary<string, Func<IEnumerable<IEnumerable<KeyValuePair<string, Atom>>>, IEnumerable<KeyValuePair<string, Atom>>>> Compute(string input)
        {
            return ComputeSequential(input, 3);
        }
    }
}