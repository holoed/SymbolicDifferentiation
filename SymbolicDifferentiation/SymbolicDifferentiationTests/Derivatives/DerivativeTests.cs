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

namespace SymbolicDifferentiation.Tests.Derivatives
{
    [TestFixture]
    public abstract class DerivativeTests
    {
        protected abstract string Derivate(string input);

        [Test]
        public void Constant()
        {
            Assert.AreEqual("0", Derivate("5"));
        }

        [Test]
        public void FirstOrderPolynomial()
        {
            Assert.AreEqual("2", Derivate("2x + 1"));
        }

        [Test]
        public void Linear()
        {
            Assert.AreEqual("1", Derivate("x"));
        }

        [Test]
        public void MultiplicationWithConstant()
        {
            Assert.AreEqual("2", Derivate("2x"));
        }

        [Test]
        public void SecondOrderPolynomial()
        {
            Assert.AreEqual("6*x+2", Derivate("3x^2 + 2x + 1"));
        }

        [Test]
        public void Square()
        {
            Assert.AreEqual("2*x", Derivate("x^2"));
        }

        [Test]
        public void SquareAndMultiplication()
        {
            Assert.AreEqual("6*x", Derivate("3x^2"));
        }

        [Test]
        public void ThirdOrderPolynomial()
        {
            Assert.AreEqual("15*x^2+6*x+2", Derivate("5x^3 + 3x^2 + 2x + 1"));
        }

        [Test]
        public void BinomialSquare()
        {
            Assert.AreEqual("2*(x+2)", Derivate("(x + 2)^2"));
        }
    }
}