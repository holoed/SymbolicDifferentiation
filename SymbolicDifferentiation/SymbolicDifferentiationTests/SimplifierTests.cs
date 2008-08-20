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
            Assert.AreEqual("5", "2 + 3".Tokenize().Parse().Simplify().ToString(true));
        }

        [Test]
        public void AdditionAndMultiplication()
        {
            Assert.AreEqual("2", "2 * 1 + 0 * 5".Tokenize().Parse().Simplify().ToString(true));
        }

        [Test]
        public void AdditionToZero()
        {
            Assert.AreEqual("2", "2 + 0".Tokenize().Parse().Simplify().ToString(true));
            Assert.AreEqual("5", "0 + 5".Tokenize().Parse().Simplify().ToString(true));
        }

        [Test]
        public void Multiplication()
        {
            Assert.AreEqual("6", "2 * 3".Tokenize().Parse().Simplify().ToString(true));
        }

        [Test]
        public void MultiplicationByOne()
        {
            Assert.AreEqual("2", "2 * 1".Tokenize().Parse().Simplify().ToString(true));
            Assert.AreEqual("5", "1 * 5".Tokenize().Parse().Simplify().ToString(true));
        }

        [Test]
        public void MultiplicationByZero()
        {
            Assert.AreEqual("0", "2 * 0".Tokenize().Parse().Simplify().ToString(true));
            Assert.AreEqual("0", "0 * 5".Tokenize().Parse().Simplify().ToString(true));
        }

        [Test]
        public void MultiplicationMultiplicationByVariable()
        {
            Assert.AreEqual("( 6 * x )", (Number(2)*(Number(3)*Variable("x"))).Simplify().ToString(true));
        }

        [Test]
        public void PowerByOne()
        {
            Assert.AreEqual("2", "2 ^ 1".Tokenize().Parse().Simplify().ToString(true));
            Assert.AreEqual("5", "5 ^ 1".Tokenize().Parse().Simplify().ToString(true));
        }
    }
}