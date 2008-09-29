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
using SymbolicDifferentiation.Core.AST;
using SymbolicDifferentiation.Extensions;

namespace SymbolicDifferentiation.Tests
{
    [TestFixture]
    public class ExpressionToStringTests
    {
        [Test]
        public void AdditionAssociativity()
        {
            Assert.AreEqual("2+3+4", FSParse("(2+3)+4").FSToString());
            Assert.AreEqual("2+3+4", FSParse("2+(3+4)").FSToString());
        }

        [Test]
        public void MultiplicationAssociativity()
        {
            Assert.AreEqual("2*3*4", FSParse("(2*3)*4").FSToString());
            Assert.AreEqual("2*3*4", FSParse("2*(3*4)").FSToString());
        }

        [Test]
        public void AdditionMultiplicationPrecedence()
        {
            Assert.AreEqual("2*(3+4)", FSParse("2*(3+4)").FSToString());
            Assert.AreEqual("(2+3)*4", FSParse("(2+3)*4").FSToString());
            Assert.AreEqual("2*3+4", FSParse("(2*3)+4").FSToString());
        }

        [Test]
        public void AdditionPowerPrecedence()
        {
            Assert.AreEqual("(3+4)^2", FSParse("(3+4)^2").FSToString());
        }

        [Test]
        public void Function()
        {
            Assert.AreEqual("Max(3,5)", FSParse("Max(3,5)").FSToString());
        }

        [Test]
        public void FunctionOfFunction()
        {
            Assert.AreEqual("Max(Max(3,5),5)", FSParse("Max(Max(3,5),5)").FSToString());
            Assert.AreEqual("Max(6,Max(3,5))", FSParse("Max(6,Max(3,5))").FSToString());
            Assert.AreEqual("Max(Max(7,3),Max(3,5))", FSParse("Max(Max(7,3),Max(3,5))").FSToString());
        }

        private static Expression FSParse(string input)
        {
            return input.FSTokenize().FSParse();
        }
    }
}
