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
using System.Linq;
using NUnit.Framework;
using SymbolicDifferentiation.Extensions;

namespace SymbolicDifferentiation.Tests.Aggregation
{
    [TestFixture]
    public class AggregationTests
    {
        private IEnumerable<double> _a;
        private IEnumerable<double> _b;

        [SetUp]
        public void SetUp()
        {
            _a = Enumerable.Range(1, 3).Select(i => i + .0);
            _b = Enumerable.Range(5, 7).Select(i => i + .0);
        }

        [Test]
        public void Add()
        {
            CollectionAssert.AreEqual(new[] {6, 8, 10}, Generate("A + B")(_a, _b).ToArray());
        }

        [Test]
        public void Mul()
        {
            CollectionAssert.AreEqual(new[] { 5, 12, 21 }, Generate("A * B")(_a, _b).ToArray());
        }

        [Test]
        public void AddMul()
        {
            CollectionAssert.AreEqual(new[] { 6, 16, 30 }, Generate("(A + B) * A")(_a, _b).ToArray());
        }

        private static Func<IEnumerable<double>, IEnumerable<double>, IEnumerable<double>> Generate(string input)
        {
            return input.FSTokenize().FSParse().FSAggregateFunction();
        }
    }
}
