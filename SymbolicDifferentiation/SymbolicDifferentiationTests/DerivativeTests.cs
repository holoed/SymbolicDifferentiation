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
using SymbolicDifferentiation.Visitors;

namespace SymbolicDifferentiation.Tests
{
    [TestFixture]
    public class DerivativeTests
    {
        [Test]
        public void Constant()
        {
            Assert.AreEqual("0", Derivative.Of("5"));
        }

        [Test]
        public void FirstOrderPolynomial()
        {
            Assert.AreEqual("2", Derivative.Of("2x + 1"));
        }

        [Test]
        public void Linear()
        {
            Assert.AreEqual("1", Derivative.Of("x"));
        }

        [Test]
        public void MultiplicationWithConstant()
        {
            Assert.AreEqual("2", Derivative.Of("2x"));
        }

        [Test]
        public void SecondOrderPolynomial()
        {
            Assert.AreEqual("6 * x + 2", Derivative.Of("3x^2 + 2x + 1"));
        }

        [Test]
        public void Square()
        {
            Assert.AreEqual("2 * x", Derivative.Of("x^2"));
        }

        [Test]
        public void SquareAndMultiplication()
        {
            Assert.AreEqual("6 * x", Derivative.Of("3x^2"));
        }

        [Test]
        public void ThirdOrderPolynomial()
        {
            Assert.AreEqual("15 * x ^ 2 + 6 * x + 2", Derivative.Of("5x^3 + 3x^2 + 2x + 1"));
        }
    }
}