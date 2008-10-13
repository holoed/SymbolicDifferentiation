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
    public class ExpressionToStringTests
    {
        [Test]
        public void AdditionAssociativity()
        {
            Assert.AreEqual("2+3+4", FSParse("(2+3)+4"));
            Assert.AreEqual("2+3+4", FSParse("2+(3+4)"));
        }

        [Test]
        public void MultiplicationAssociativity()
        {
            Assert.AreEqual("2*3*4", FSParse("(2*3)*4"));
            Assert.AreEqual("2*3*4", FSParse("2*(3*4)"));
        }

        [Test]
        public void AdditionMultiplicationPrecedence()
        {
            Assert.AreEqual("2*(3+4)", FSParse("2*(3+4)"));
            Assert.AreEqual("(2+3)*4", FSParse("(2+3)*4"));
            Assert.AreEqual("2*3+4", FSParse("(2*3)+4"));
        }

        [Test]
        public void AdditionPowerPrecedence()
        {
            Assert.AreEqual("(3+4)^2", FSParse("(3+4)^2"));
        }

        [Test]
        public void FunctionApplication()
        {
            Assert.AreEqual("Max(3,5)", FSParse("Max(3,5)"));
        }

        [Test]
        public void FunctionAppOfFunctionApp()
        {
            Assert.AreEqual("Max(Max(3,5),5)", FSParse("Max(Max(3,5),5)"));
            Assert.AreEqual("Max(6,Max(3,5))", FSParse("Max(6,Max(3,5))"));
            Assert.AreEqual("Max(Max(7,3),Max(3,5))", FSParse("Max(Max(7,3),Max(3,5))"));
        }

        [Test]
        public void FunctionDeclaration()
        {
            Assert.AreEqual("A=Max(3,5)", FSParse("A=Max(3,5)"));
        }

        [Test]
        public void FunctionDeclarationWithArguments()
        {
            Assert.AreEqual("Add(x,y)=x+y", FSParse("Add(x,y)=x+y"));
        }

        private static string FSParse(string input)
        {
            return input.FSTokenize().FSParse().FSToString();
        }
    }
}
