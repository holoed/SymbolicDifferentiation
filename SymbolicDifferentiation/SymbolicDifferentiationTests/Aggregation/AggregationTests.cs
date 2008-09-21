﻿#region License

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
using NUnit.Framework;

namespace SymbolicDifferentiation.Tests.Aggregation
{
    [TestFixture]
    public abstract class AggregationTests
    {
        protected Dictionary<string, double[]> _data;

        [SetUp]
        public void SetUp()
        {
            _data = new Dictionary<string, double[]>
                        {
                            {"A", Enumerable.Range(1, 3).Select(i => i + .0).ToArray()},
                            {"B", Enumerable.Range(5, 3).Select(i => i + .0).ToArray()},
                            {"C", Enumerable.Range(9, 3).Select(i => i + .0).ToArray()},
                            {"D", Enumerable.Range(30,3).Select(i => i + .0).ToArray()},
                        };
        }

        [Test]
        public void Add()
        {
            var compute = Compute("A + B");
            CollectionAssert.AreEqual(new[] { 6, 8, 10 }, compute);
        }

        [Test]
        public void Mul()
        {
            CollectionAssert.AreEqual(new[] { 5, 12, 21 }, Compute("A * B"));
        }

        [Test]
        public void AddMul()
        {
            CollectionAssert.AreEqual(new[] { 6, 16, 30 }, Compute("(A + B) * A"));
        }

        [Test]
        public void AddToConstant()
        {
            CollectionAssert.AreEqual(new[] { 11, 12, 13 }, Compute("A + 10"));
            CollectionAssert.AreEqual(new[] { 15, 16, 17 }, Compute("B + 10"));
        }

        [Test]
        public void AddMulToConstant()
        {
            CollectionAssert.AreEqual(new[] { 12, 16, 20 }, Compute("2 * (A + B)"));
        }

        [Test]
        public void Pow()
        {
            CollectionAssert.AreEqual(new[] { 1, 4, 9 }, Compute("A^2"));
            CollectionAssert.AreEqual(new[] { 36, 64, 100 }, Compute("(A + B)^2"));
        }

        [Test]
        public void Polynomial()
        {
            CollectionAssert.AreEqual(new[] { 190, 305, 526 }, Compute("8*A^3 + 5*B^2 + 3*C + D"));
        }

        protected abstract double[] Compute(string input);
    }
}
