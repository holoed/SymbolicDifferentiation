﻿#region License

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

using System.Collections.Generic;
using NUnit.Framework;
using SymbolicDifferentiation.Core.AST;
using SymbolicDifferentiation.Core.Tokens;

namespace SymbolicDifferentiation.Tests
{
    [TestFixture]
    public abstract class ParserTests : TestsBase
    {
        protected abstract Expression Parse(IEnumerable<Token> tokens);

        [Test]
        public void AdditionAdditionBinaryExpression()
        {
            ExpressionAssert.AreEqual((Number(2) + Number(3)) + Number(4), Parse(Tokenizer.Tokenize("2 + 3 + 4")));
        }

        [Test]
        public void AdditionBinaryExpression()
        {
            ExpressionAssert.AreEqual(Number(2) + Number(3), Parse(Tokenizer.Tokenize("2 + 3")));
        }

        [Test]
        public void AdditionMultiplication()
        {
            ExpressionAssert.AreEqual(Number(2) + (Number(3)*Number(4)), Parse(Tokenizer.Tokenize("2 + 3 * 4")));
        }

        [Test]
        public void AditionAdditionAdditionBinaryExpression()
        {
            ExpressionAssert.AreEqual(((Number(2) + Number(3)) + Number(4)) + Number(5),
                                      Parse(Tokenizer.Tokenize("2 + 3 + 4 + 5")));
        }

        [Test]
        public void Expression()
        {
            ExpressionAssert.AreEqual(Number(3), Parse(Tokenizer.Tokenize("3")));
        }

        [Test]
        public void FirstOrderPolynomial()
        {
            ExpressionAssert.AreEqual(Number(2)*Variable("x") + Number(1), Parse(Tokenizer.Tokenize("2*x + 1")));
        }

        [Test]
        public void FourthOrderPolynomial()
        {
            ExpressionAssert.AreEqual(
                ((Number(2)*(Variable("x") ^ Number(4))) + (Number(5)*(Variable("x") ^ Number(3)))) +
                Number(3)*(Variable("x") ^ Number(2)) + (Number(2)*Variable("x")) + Number(1),
                Parse(Tokenizer.Tokenize("2*x^4 + 5*x^3 + 3*x^2 + 2*x + 1")));
        }

        [Test]
        public void MultiplicationAddition()
        {
            ExpressionAssert.AreEqual((Number(2)*Number(3)) + Number(4), Parse(Tokenizer.Tokenize("2 * 3 + 4")));
        }

        [Test]
        public void MultiplicationAndPowerBinaryExpression()
        {
            ExpressionAssert.AreEqual(Number(3)*(Variable("x") ^ Number(2)), Parse(Tokenizer.Tokenize("3*x^2")));
        }

        [Test]
        public void MultiplicationBinaryExpression()
        {
            ExpressionAssert.AreEqual(Number(2)*Number(3), Parse(Tokenizer.Tokenize("2 * 3")));
        }

        [Test]
        public void MultiplicationMultiplicationBinaryExpression()
        {
            ExpressionAssert.AreEqual((Number(2)*Number(3))*Number(4), Parse(Tokenizer.Tokenize("2 * 3 * 4")));
        }

        [Test]
        public void MultiplicationMultiplicationMultiplicationBinaryExpression()
        {
            ExpressionAssert.AreEqual(((Number(2)*Number(3))*Number(4))*Number(5),
                                      Parse(Tokenizer.Tokenize("2 * 3 * 4 * 5")));
        }

        [Test]
        public void MultiplicationToVariableBinaryExpression()
        {
            ExpressionAssert.AreEqual(Number(2)*Variable("x"), Parse(Tokenizer.Tokenize("2*x")));
        }

        [Test]
        public void PowerBinaryExpression()
        {
            ExpressionAssert.AreEqual(Variable("x") ^ Number(2), Parse(Tokenizer.Tokenize("x^2")));
        }

        [Test]
        public void SecondOrderPolynomial()
        {
            ExpressionAssert.AreEqual(Number(3)*(Variable("x") ^ Number(2)) + (Number(2)*Variable("x")) + Number(1),
                                      Parse(Tokenizer.Tokenize("3*x^2 + 2*x + 1")));
        }

        [Test]
        public void ThirdOrderPolynomial()
        {
            ExpressionAssert.AreEqual(
                (Number(5)*(Variable("x") ^ Number(3))) + Number(3)*(Variable("x") ^ Number(2)) +
                (Number(2)*Variable("x")) + Number(1),
                Parse(Tokenizer.Tokenize("5*x^3 + 3*x^2 + 2*x + 1")));
        }
    }
}