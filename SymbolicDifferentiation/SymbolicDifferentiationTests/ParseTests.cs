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
using SymbolicDifferentiation.Tokens;

namespace SymbolicDifferentiation.Tests
{
    [TestFixture]
    public class ParseTests : TestsBase
    {
        [Test]
        public void AdditionAdditionBinaryExpression()
        {
            ExpressionAssert.AreEqual((Number(2) + Number(3)) + Number(4), Tokenizer.Tokenize("2 + 3 + 4").Parse());
        }

        [Test]
        public void AdditionBinaryExpression()
        {
            ExpressionAssert.AreEqual(Number(2) + Number(3), Tokenizer.Tokenize("2 + 3").Parse());
        }

        [Test]
        public void AdditionMultiplication()
        {
            ExpressionAssert.AreEqual(Number(2) + (Number(3)*Number(4)), Tokenizer.Tokenize("2 + 3 * 4").Parse());
        }

        [Test]
        public void AditionAdditionAdditionBinaryExpression()
        {
            ExpressionAssert.AreEqual(((Number(2) + Number(3)) + Number(4)) + Number(5),
                                      Tokenizer.Tokenize("2 + 3 + 4 + 5").Parse());
        }

        [Test]
        public void Expression()
        {
            ExpressionAssert.AreEqual(Number(3), Tokenizer.Tokenize("3").Parse());
        }

        [Test]
        public void FirstOrderPolynomial()
        {
            ExpressionAssert.AreEqual(Number(2)*Variable("x") + Number(1), Tokenizer.Tokenize("2*x + 1").Parse());
        }

        [Test]
        public void FourthOrderPolynomial()
        {
            ExpressionAssert.AreEqual(
                ((Number(2)*(Variable("x") ^ Number(4))) + (Number(5)*(Variable("x") ^ Number(3)))) +
                Number(3)*(Variable("x") ^ Number(2)) + (Number(2)*Variable("x")) + Number(1),
                Tokenizer.Tokenize("2*x^4 + 5*x^3 + 3*x^2 + 2*x + 1").Parse());
        }

        [Test]
        public void MultiplicationAddition()
        {
            ExpressionAssert.AreEqual((Number(2)*Number(3)) + Number(4), Tokenizer.Tokenize("2 * 3 + 4").Parse());
        }

        [Test]
        public void MultiplicationAndPowerBinaryExpression()
        {
            ExpressionAssert.AreEqual(Number(3)*(Variable("x") ^ Number(2)), Tokenizer.Tokenize("3*x^2").Parse());
        }

        [Test]
        public void MultiplicationBinaryExpression()
        {
            ExpressionAssert.AreEqual(Number(2)*Number(3), Tokenizer.Tokenize("2 * 3").Parse());
        }

        [Test]
        public void MultiplicationMultiplicationBinaryExpression()
        {
            ExpressionAssert.AreEqual((Number(2)*Number(3))*Number(4), Tokenizer.Tokenize("2 * 3 * 4").Parse());
        }

        [Test]
        public void MultiplicationMultiplicationMultiplicationBinaryExpression()
        {
            ExpressionAssert.AreEqual(((Number(2)*Number(3))*Number(4))*Number(5),
                                      Tokenizer.Tokenize("2 * 3 * 4 * 5").Parse());
        }

        [Test]
        public void MultiplicationToVariableBinaryExpression()
        {
            ExpressionAssert.AreEqual(Number(2)*Variable("x"), Tokenizer.Tokenize("2*x").Parse());
        }

        [Test]
        public void PowerBinaryExpression()
        {
            ExpressionAssert.AreEqual(Variable("x") ^ Number(2), Tokenizer.Tokenize("x^2").Parse());
        }

        [Test]
        public void SecondOrderPolynomial()
        {
            ExpressionAssert.AreEqual(Number(3)*(Variable("x") ^ Number(2)) + (Number(2)*Variable("x")) + Number(1),
                                      Tokenizer.Tokenize("3*x^2 + 2*x + 1").Parse());
        }

        [Test]
        public void ThirdOrderPolynomial()
        {
            ExpressionAssert.AreEqual(
                (Number(5)*(Variable("x") ^ Number(3))) + Number(3)*(Variable("x") ^ Number(2)) +
                (Number(2)*Variable("x")) + Number(1),
                Tokenizer.Tokenize("5*x^3 + 3*x^2 + 2*x + 1").Parse());
        }
    }
}