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
using Microsoft.FSharp.Core;
using NUnit.Framework;
using SymbolicDifferentiation.Extensions;
using SymbolicDifferentiation.Parallel;

namespace SymbolicDifferentiation.Tests.Aggregation
{
    [TestFixture]
    public abstract class AggregationTests
    {
        protected Dictionary<string, IEnumerable<double>> _data;

        protected static Dictionary<string, FastFunc<IEnumerable<IEnumerable<double>>, IEnumerable<double>>> _funcs = new Dictionary<string, FastFunc<IEnumerable<IEnumerable<double>>, IEnumerable<double>>>
                                              {
                                                  {"Add", ToFastFunc<IEnumerable<double>>(ParallelFunctions.Add)},
                                                  {"Mul", ToFastFunc<IEnumerable<double>>(ParallelFunctions.Mul)},
                                                  {"Pow", ToFastFunc<IEnumerable<double>>(ParallelFunctions.Pow)},
                                                  {"Max", ToFastFunc<IEnumerable<double>>(ParallelFunctions.Max)}
                                              };

        [SetUp]
        public void SetUp()
        {
            _data = new Dictionary<string, IEnumerable<double>>
                        {
                            {"A", Enumerable.Range(1, 3).Select(i => i + .0)},
                            {"B", Enumerable.Range(5, 3).Select(i => i + .0)},
                            {"C", Enumerable.Range(9, 3).Select(i => i + .0)},
                            {"D", Enumerable.Range(30,3).Select(i => i + .0)},
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

        [Test]
        public void Function()
        {
            var compute = Compute("Max(A , B)");
            CollectionAssert.AreEqual(_data["B"].ToArray(), compute.ToArray());
        }

        [Test]
        public void FunctionOfFunctions()
        {
            var compute = Compute("Max(Max(A,B), Max(A,B))");
            CollectionAssert.AreEqual(_data["B"].ToArray(), compute.ToArray());
        }

        protected abstract double[] Compute(string input);

        protected static FastFunc<IEnumerable<T>, T> ToFastFunc<T>(Converter<IEnumerable<T>, T> func)
        {
            return FuncConvert.ToFastFunc(func);
        }

        protected double[] ComputeParallel(string input, int size)
        {
            return input.FSTokenize().FSParse().FSParallelComputation(_funcs)(_data).Take(size).ToArray();
        }

        protected double[] ComputeSequential(string input, int size)
        {
            return input.FSTokenize().FSParse().FSSequentialComputation(_funcs)(_data).Take(size).ToArray();
        } 
    }
}
 