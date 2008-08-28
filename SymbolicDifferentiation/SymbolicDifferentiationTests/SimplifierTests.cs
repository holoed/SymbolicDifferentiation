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

using NUnit.Framework;
using SymbolicDifferentiation.Extensions;

namespace SymbolicDifferentiation.Tests
{
    [TestFixture]
    public class SimplifierTests : TestsBase
    {
        [Test]
        public void Addition()
        {
            Assert.AreEqual("5", Simplify("2 + 3"));
        }

        [Test]
        public void AdditionAndMultiplication()
        {
            Assert.AreEqual("2", Simplify("2 * 1 + 0 * 5"));
        }

        [Test]
        public void AdditionToZero()
        {
            Assert.AreEqual("2", Simplify("2 + 0"));
            Assert.AreEqual("5", Simplify("0 + 5"));
        }

        [Test]
        public void Multiplication()
        {
            Assert.AreEqual("6", Simplify("2 * 3"));
        }

        [Test]
        public void MultiplicationByOne()
        {
            Assert.AreEqual("2", Simplify("2 * 1"));
            Assert.AreEqual("5", Simplify("1 * 5"));
        }

        [Test]
        public void MultiplicationByZero()
        {
            Assert.AreEqual("0", Simplify("2 * 0"));
            Assert.AreEqual("0", Simplify("0 * 5"));
        }

        [Test]
        public void MultiplicationMultiplicationByVariable()
        {
            Assert.AreEqual("(6*x)", (Number(2) * (Number(3) * Variable("x"))).Simplify().ToTokens(true).ToStringExpression());
        }

        [Test]
        public void PowerByOne()
        {
            Assert.AreEqual("2", Simplify("2 ^ 1"));
            Assert.AreEqual("5", Simplify("5 ^ 1"));
        }

        private static string Simplify(string input)
        {
            return input.Tokenize().BasicParse().Simplify().ToTokens(true).ToStringExpression();
        }
    }
}