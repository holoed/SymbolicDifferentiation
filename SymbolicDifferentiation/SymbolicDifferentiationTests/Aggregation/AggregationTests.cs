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
using SymbolicDifferentiation.Computation;
using SymbolicDifferentiation.Core.Tokens;
using SymbolicDifferentiation.Extensions;

namespace SymbolicDifferentiation.Tests.Aggregation
{
    [TestFixture]
    public abstract class AggregationTests
    {
        protected static Dictionary<string, IEnumerable<KeyValuePair<string, double>>> _data = new Dictionary<string, IEnumerable<KeyValuePair<string, double>>>
                        {
                            {"A", Enumerable.Range(1, 3).Select(i => new KeyValuePair<string,double>(i.ToString(), i + .0))},
                            {"B", Enumerable.Range(5, 3).Select(i => new KeyValuePair<string,double>(i.ToString(), i + .0))},
                            {"C", Enumerable.Range(9, 3).Select(i => new KeyValuePair<string,double>(i.ToString(), i + .0))},
                            {"D", Enumerable.Range(30,3).Select(i => new KeyValuePair<string,double>(i.ToString(), i + .0))},
                        };

        public static Dictionary<string, FastFunc<IEnumerable<IEnumerable<KeyValuePair<string, double>>>, IEnumerable<KeyValuePair<string, double>>>> Funcs = new Dictionary<string, FastFunc<IEnumerable<IEnumerable<KeyValuePair<string, double>>>, IEnumerable<KeyValuePair<string, double>>>>
                                              {
                                                  {"A", ToFastFunc<IEnumerable<KeyValuePair<string,double>>>(input => ParallelFunctions.Data(_data["A"]))},
                                                  {"B", ToFastFunc<IEnumerable<KeyValuePair<string,double>>>(input => ParallelFunctions.Data(_data["B"]))},
                                                  {"C", ToFastFunc<IEnumerable<KeyValuePair<string,double>>>(input => ParallelFunctions.Data(_data["C"]))},
                                                  {"D", ToFastFunc<IEnumerable<KeyValuePair<string,double>>>(input => ParallelFunctions.Data(_data["D"]))},
                                                  
                                                  {"Add", ToFastFunc<IEnumerable<KeyValuePair<string,double>>>(ParallelFunctions.Add)},
                                                  {"Sub", ToFastFunc<IEnumerable<KeyValuePair<string,double>>>(ParallelFunctions.Sub)},                                                  
                                                  {"Mul", ToFastFunc<IEnumerable<KeyValuePair<string,double>>>(ParallelFunctions.Mul)},
                                                  {"Div", ToFastFunc<IEnumerable<KeyValuePair<string,double>>>(ParallelFunctions.Div)},
                                                  {"Pow", ToFastFunc<IEnumerable<KeyValuePair<string,double>>>(ParallelFunctions.Pow)},
                                                  {"Max", ToFastFunc<IEnumerable<KeyValuePair<string,double>>>(ParallelFunctions.Max)}
                                              };

        [Test]
        public void Add()
        {
            CollectionAssert.AreEqual(new[] { 6, 8, 10 }, ComputeSingle("A + B"));
        }

        [Test]
        public void Sub()
        {
            CollectionAssert.AreEqual(new[] { 1-5, 2-6, 3-7 }, ComputeSingle("A - B"));
        }

        [Test]
        public void Mul()
        {
            CollectionAssert.AreEqual(new[] { 5, 12, 21 }, ComputeSingle("A * B"));
        }

        [Test]
        public void Div()
        {
            CollectionAssert.AreEqual(new[] { 1d/5d, 2d/6d, 3d/7d }, ComputeSingle("A / B"));
        }

        [Test]
        public void AddMul()
        {
            CollectionAssert.AreEqual(new[] { 6, 16, 30 }, ComputeSingle("(A + B) * A"));
        }

        [Test]
        public void AddToConstant()
        {
            CollectionAssert.AreEqual(new[] { 11, 12, 13 }, ComputeSingle("A + 10"));
            CollectionAssert.AreEqual(new[] { 15, 16, 17 }, ComputeSingle("B + 10"));
        }

        [Test]
        public void AddMulToConstant()
        {
            CollectionAssert.AreEqual(new[] { 12, 16, 20 }, ComputeSingle("2 * (A + B)"));
        }

        [Test]
        public void Pow()
        {
            CollectionAssert.AreEqual(new[] { 1, 4, 9 }, ComputeSingle("A^2"));
            CollectionAssert.AreEqual(new[] { 36, 64, 100 }, ComputeSingle("(A + B)^2"));
        }

        [Test]
        public void Polynomial()
        {
            CollectionAssert.AreEqual(new[] { 190, 305, 526 }, ComputeSingle("8*A^3 + 5*B^2 + 3*C + D"));
        }

        [Test]
        public void Function()
        {
            var compute = ComputeSingle("Max(A , B)");
            CollectionAssert.AreEqual(_data["B"].Select(item=>item.Value).ToArray(), compute.ToArray());
        }

        [Test]
        public void FunctionOfFunctions()
        {
            var compute = ComputeSingle("Max(Max(A,B), Max(A,B))");
            CollectionAssert.AreEqual(_data["B"].Select(item => item.Value).ToArray(), compute.ToArray());
        }

        [Test]
        public void SingleFunctionDeclaration()
        {
            var compute = Compute("A = 2 + 3");
           Assert.IsTrue(compute.ContainsKey("A"));
           CollectionAssert.AreEqual(new [] { new KeyValuePair<string, double>(MatchType.Number.ToString(), 5) }, compute["A"].ToArray());
        }

        [Test]
        public void TwoFunctionDeclarations()
        {
            var compute = Compute(@"A = 2 + 3
                                    B = C + 5");
            Assert.IsTrue(compute.ContainsKey("A"));
            CollectionAssert.AreEqual(new[]
                                          {
                                              new KeyValuePair<string, double>(MatchType.Number.ToString(), 5),
                                          }, compute["A"].ToArray());

            CollectionAssert.AreEqual(new[]
                                          {
                                              new KeyValuePair<string, double>("9", 14),
                                              new KeyValuePair<string, double>("10", 15),
                                              new KeyValuePair<string, double>("11", 16)
                                          }, compute["B"].ToArray());        
        }

        private IEnumerable<double> ComputeSingle(string input)
        {
            return Compute(input).First().Value.Select(item => item.Value).ToArray();
        }

        protected abstract IDictionary<string, IEnumerable<KeyValuePair<string, double>>> Compute(string input);

        protected static FastFunc<IEnumerable<T>, T> ToFastFunc<T>(Converter<IEnumerable<T>, T> func)
        {
            return FuncConvert.ToFastFunc(func);
        }

        protected IDictionary<string, IEnumerable<KeyValuePair<string, double>>> ComputeParallel(string input, int size)
        {
            return input.FSTokenize().FSParse().FSParallelComputation(Funcs)(_data);
        }

        protected IDictionary<string, IEnumerable<KeyValuePair<string, double>>> ComputeSequential(string input, int size)
        {
            return input.FSTokenize().FSParse().FSSequentialComputation(Funcs)(_data);
        } 
    }
}
 