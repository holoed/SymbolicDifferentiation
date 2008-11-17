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
using SymbolicDifferentiation.Core.Computation;
using SymbolicDifferentiation.Core.Tokens;
using SymbolicDifferentiation.Extensions;

namespace SymbolicDifferentiation.Tests.Computation
{
    [TestFixture]
    public abstract class ComputationTests
    {
        protected static IDictionary<string, IEnumerable<KeyValuePair<string, Atom>>> _data = new Dictionary<string, IEnumerable<KeyValuePair<string, Atom>>>
                                                                                                  {
                                                                                                      {"A", Enumerable.Range(1, 3).Select(i => new KeyValuePair<string,Atom>(i.ToString(), i + .0))},
                                                                                                      {"B", Enumerable.Range(5, 3).Select(i => new KeyValuePair<string,Atom>(i.ToString(), i + .0))},
                                                                                                      {"C", Enumerable.Range(9, 3).Select(i => new KeyValuePair<string,Atom>(i.ToString(), i + .0))},
                                                                                                      {"D", Enumerable.Range(30,3).Select(i => new KeyValuePair<string,Atom>(i.ToString(), i + .0))},
                                                                                                  };

        public static IDictionary<string, FastFunc<IEnumerable<IEnumerable<KeyValuePair<string, Atom>>>, IEnumerable<KeyValuePair<string, Atom>>>> Funcs = new Dictionary<string, FastFunc<IEnumerable<IEnumerable<KeyValuePair<string, Atom>>>, IEnumerable<KeyValuePair<string, Atom>>>>
                                                                                                                                                               {
                                                                                                                                                                   {"A", ToFastFunc<IEnumerable<KeyValuePair<string,Atom>>>(input => ParallelFunctions.Data(_data["A"]))},
                                                                                                                                                                   {"B", ToFastFunc<IEnumerable<KeyValuePair<string,Atom>>>(input => ParallelFunctions.Data(_data["B"]))},
                                                                                                                                                                   {"C", ToFastFunc<IEnumerable<KeyValuePair<string,Atom>>>(input => ParallelFunctions.Data(_data["C"]))},
                                                                                                                                                                   {"D", ToFastFunc<IEnumerable<KeyValuePair<string,Atom>>>(input => ParallelFunctions.Data(_data["D"]))},
                                                  
                                                                                                                                                                   {"GreaterThan", ToFastFunc<IEnumerable<KeyValuePair<string,Atom>>>(ParallelFunctions.GreaterThan)},
                                                                                                                                                                   {"LessThan", ToFastFunc<IEnumerable<KeyValuePair<string,Atom>>>(ParallelFunctions.LessThan)},
                                                                                                                                                                   {"Add", ToFastFunc<IEnumerable<KeyValuePair<string,Atom>>>(ParallelFunctions.Add)},
                                                                                                                                                                   {"Sub", ToFastFunc<IEnumerable<KeyValuePair<string,Atom>>>(ParallelFunctions.Sub)},                                                  
                                                                                                                                                                   {"Mul", ToFastFunc<IEnumerable<KeyValuePair<string,Atom>>>(ParallelFunctions.Mul)},
                                                                                                                                                                   {"Div", ToFastFunc<IEnumerable<KeyValuePair<string,Atom>>>(ParallelFunctions.Div)},
                                                                                                                                                                   {"Pow", ToFastFunc<IEnumerable<KeyValuePair<string,Atom>>>(ParallelFunctions.Pow)},
                                                                                                                                                                   {"Max", ToFastFunc<IEnumerable<KeyValuePair<string,Atom>>>(ParallelFunctions.Max)}
                                                                                                                                                               };

        public static readonly KeyValuePair<string, Atom>[][] Empty = new[] { new KeyValuePair<string, Atom>[0] };

        [Test]
        public void GreaterThan()
        {
            CollectionAssert.AreEqual(new Atom[] { false, false, false }, ComputeSingle("A > B"));
        }

        [Test]
        public void LessThan()
        {
            CollectionAssert.AreEqual(new Atom[] { true, true, true }, ComputeSingle("A < B"));
        }

        [Test]
        public void Add()
        {
            CollectionAssert.AreEqual(new Atom[] { 6, 8, 10 }, ComputeSingle("A + B"));
        }

        [Test]
        public void Sub()
        {
            CollectionAssert.AreEqual(new Atom[] { 1-5, 2-6, 3-7 }, ComputeSingle("A - B"));
        }

        [Test]
        public void Mul()
        {
            CollectionAssert.AreEqual(new Atom[] { 5, 12, 21 }, ComputeSingle("A * B"));
        }

        [Test]
        public void Div()
        {
            CollectionAssert.AreEqual(new Atom[] { 1d/5d, 2d/6d, 3d/7d }, ComputeSingle("A / B"));
        }

        [Test]
        public void AddMul()
        {
            CollectionAssert.AreEqual(new Atom[] { 6, 16, 30 }, ComputeSingle("(A + B) * A"));
        }

        [Test]
        public void AddToConstant()
        {
            CollectionAssert.AreEqual(new Atom[] { 11, 12, 13 }, ComputeSingle("A + 10"));
            CollectionAssert.AreEqual(new Atom[] { 15, 16, 17 }, ComputeSingle("B + 10"));
        }

        [Test]
        public void AddMulToConstant()
        {
            CollectionAssert.AreEqual(new Atom[] { 12, 16, 20 }, ComputeSingle("2 * (A + B)"));
        }

        [Test]
        public void Pow()
        {
            CollectionAssert.AreEqual(new Atom[] { 1, 4, 9 }, ComputeSingle("A^2"));
            CollectionAssert.AreEqual(new Atom[] { 36, 64, 100 }, ComputeSingle("(A + B)^2"));
        }

        [Test]
        public void Polynomial()
        {
            CollectionAssert.AreEqual(new Atom[] { 190, 305, 526 }, ComputeSingle("8*A^3 + 5*B^2 + 3*C + D"));
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
            var compute = Compute("X = 2 + 3");
            Assert.IsTrue(compute.ContainsKey("X"));
            CollectionAssert.AreEqual(new[] {new KeyValuePair<string, Atom>(MatchType.Number.ToString(), 5)},
                                      compute["X"](Empty).ToArray());
        }

        [Test]
        public void TwoFunctionDeclarations()
        {
            var compute = Compute(@"X = 2 + 3
                                    Y = C + 5");
            Assert.IsTrue(compute.ContainsKey("X"));
            CollectionAssert.AreEqual(new[]
                                          {
                                              new KeyValuePair<string, Atom>(MatchType.Number.ToString(), 5),
                                          }, compute["X"](Empty).ToArray());

            CollectionAssert.AreEqual(new[]
                                          {
                                              new KeyValuePair<string, Atom>("9", 14),
                                              new KeyValuePair<string, Atom>("10", 15),
                                              new KeyValuePair<string, Atom>("11", 16)
                                          }, compute["Y"](Empty).ToArray());        
        }

        [Test]
        public void FunctionReferencesPreviousFunction()
        {
            var compute = Compute(@"X = A + B
                                    Y = X + C
                                    Z = A + B + C");
            var x = compute["Y"](Empty).ToArray();
            var z = compute["Z"](Empty).ToArray();
            CollectionAssert.AreEqual(
                x, 
                z);
        }

        [Test]
        public void RedefineExistingFunction()
        {
            var compute = Compute(@"X = A + B
                                    X = X + C
                                    Y = A + B + C");
            var x = compute["X"](Empty).ToArray();
            var z = compute["Y"](Empty).ToArray();
            CollectionAssert.AreEqual(
                x,
                z);
        }

        [Test]
        public void UsePreviouslyDefinedFunctionWithArguments()
        {
            var compute = Compute(@"AddStuff(x,y) = x + y
                                    z = AddStuff(A, B)
                                    k = A + B");
            var z = compute["z"](Empty).ToArray();
            var k = compute["k"](Empty).ToArray();
            CollectionAssert.AreEqual(
                z,
                k);
        }

        [Test]
        public void UsePreviouslyDefinedTwoFunctionsWithArguments()
        {
            var compute = Compute(@"AddStuff(x,y) = x + y
                                    PowStuff(x,y) = x ^ y
                                    z = PowStuff(AddStuff(A, B), 2)
                                    k = (A + B)^2");
            var z = compute["z"](Empty).ToArray();
            var k = compute["k"](Empty).ToArray();
            CollectionAssert.AreEqual(
                z,
                k);
        }

        [Test]
        public void CustomFunctionsUsesPreviouslyDefinedCustomFunction()
        {
            var compute = Compute(@"Add(x,y) = x + y
                                    AddAndPow(x,y) = Add(x,y) ^ y
                                    z = AddAndPow(A,B)
                                    k = (A + B)^B");
            var z = compute["z"](Empty).ToArray();
            var k = compute["k"](Empty).ToArray();
            CollectionAssert.AreEqual(
                z,
                k);
        }

        [Test]
        public void ConditionalExpressionGreaterThan()
        {
            CollectionAssert.AreEqual(new Atom[] { 5, 6, 7 }, ComputeSingle("(A > B) ? A : B"));
        }

        [Test]
        public void ConditionalExpressionLessThan()
        {
            CollectionAssert.AreEqual(new Atom[] { 1, 2, 3 }, ComputeSingle("(A < B) ? A : B"));
        }

        private IEnumerable<Atom> ComputeSingle(string input)
        {
            return Compute(input)[""](Empty).Select(item => item.Value).ToArray();
        }

        protected abstract IDictionary<string, Func<IEnumerable<IEnumerable<KeyValuePair<string, Atom>>>, IEnumerable<KeyValuePair<string, Atom>>>> Compute(string input);

        public static FastFunc<IEnumerable<T>, T> ToFastFunc<T>(Converter<IEnumerable<T>, T> func)
        {
            return FuncConvert.ToFastFunc(func);
        }

        protected IDictionary<string, Func<IEnumerable<IEnumerable<KeyValuePair<string, Atom>>>, IEnumerable<KeyValuePair<string, Atom>>>> ComputeParallel(string input, int size)
        {
            return input.FSTokenize().FSParse().FSParallelComputation(Funcs)().ToDictionary(item => item.Key,
                                                                                            item => ConvertToFunc(item.Value));
        }

        protected IDictionary<string, Func<IEnumerable<IEnumerable<KeyValuePair<string, Atom>>>, IEnumerable<KeyValuePair<string, Atom>>>> ComputeSequential(string input, int size)
        {
            return input.FSTokenize().FSParse().FSSequentialComputation(Funcs)().ToDictionary(item => item.Key,
                                                                                              item => ConvertToFunc(item.Value));
        }

        public static Func<IEnumerable<IEnumerable<KeyValuePair<string, Atom>>>, IEnumerable<KeyValuePair<string, Atom>>> ConvertToFunc(FastFunc<IEnumerable<IEnumerable<KeyValuePair<string, Atom>>>, IEnumerable<KeyValuePair<string, Atom>>> value)
        {
            return arg => value.Invoke(arg);
        }
    }
}