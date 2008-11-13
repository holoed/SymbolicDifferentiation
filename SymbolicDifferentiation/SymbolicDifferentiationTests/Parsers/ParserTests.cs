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

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SymbolicDifferentiation.Core.AST;
using SymbolicDifferentiation.Core.Tokens;

namespace SymbolicDifferentiation.Tests.Parsers
{
    [TestFixture]
    public abstract class ParserTests : TestsBase
    {
        protected abstract Expression Parse(IEnumerable<Token> tokens);
        protected abstract IEnumerable<Expression> ParseMultiple(IEnumerable<Token> tokens);

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
        public void DivisionBinaryExpression()
        {
            ExpressionAssert.AreEqual(Number(2) / Number(3), Parse(Tokenizer.Tokenize("2 / 3")));
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

        [Test]
        public void AdditionMultiplicationGrouped()
        {
            ExpressionAssert.AreEqual((Number(2) + Number(3)) * Number(4), Parse(Tokenizer.Tokenize("(2 + 3) * 4")));
        }

        [Test]
        public void AdditionExponentationGrouped()
        {
            ExpressionAssert.AreEqual((Number(2) + Number(3)) ^ Number(2), Parse(Tokenizer.Tokenize("(2 + 3)^2")));
        }

        [Test]
        public void FunctionApplication()
        {
            var expression = Parse(Tokenizer.Tokenize("Square(3)"));
            ExpressionAssert.AreEqual(FunctionApp("Square", Number(3)), expression);
        }

        [Test]
        public void FunctionApplicationWithTwoArguments()
        {
            var expression = Parse(Tokenizer.Tokenize("Max(a,b)"));
            ExpressionAssert.AreEqual(FunctionApp("Max", Variable("a"), Variable("b")), expression);
        }

        [Test]
        public void FunctionApplicationAndAddition()
        {
            var expression = Parse(Tokenizer.Tokenize("Sin(3) + Cos(2)"));
            ExpressionAssert.AreEqual(FunctionApp("Sin", Number(3)) + FunctionApp("Cos", Number(2)), expression);
        }

        [Test]
        public void FunctionApplicationAndMultiplication()
        {
            var expression = Parse(Tokenizer.Tokenize("Sin(3) * Cos(2)"));
            ExpressionAssert.AreEqual(FunctionApp("Sin", Number(3)) * FunctionApp("Cos", Number(2)), expression);
        }

        [Test]
        public void FunctionApplicationAndMultiplicationAndAddition()
        {
            var expression = Parse(Tokenizer.Tokenize("Sin(3) * (Tan(6) + Cos(2))"));
            ExpressionAssert.AreEqual(FunctionApp("Sin", Number(3)) * (FunctionApp("Tan", Number(6)) + FunctionApp("Cos", Number(2))), expression);
        }

        [Test]
        public void FunctionDeclaration()
        {
            ExpressionAssert.AreEqual(FunctionDecl("A", Number(2)), Parse(Tokenizer.Tokenize("A = 2")));
        }

        [Test]
        public void FunctionDeclarationWithArgument()
        {
            ExpressionAssert.AreEqual(FunctionDecl("f", new[] { Variable("x") }, Variable("x")), Parse(Tokenizer.Tokenize("f(x) = x")));
        }

        [Test]
        public void FunctionDeclarationWithTwoArguments()
        {
            ExpressionAssert.AreEqual(FunctionDecl("f", new[] { Variable("x"), Variable("y") }, Variable("x") + Variable("y")), Parse(Tokenizer.Tokenize("f(x,y) = x + y")));
        }

        [Test]
        public void FunctionDeclarationWithAdditionBody()
        {
            ExpressionAssert.AreEqual(FunctionDecl("A", Number(3) + Number(2)), Parse(Tokenizer.Tokenize("A = 3 + 2")));
        }

        [Test]
        public void FunctionDeclarationWithComplexBody()
        {
            ExpressionAssert.AreEqual(
                FunctionDecl("v", 
                (FunctionApp("sin", Variable("x"))^Number(2)) + 
                (FunctionApp("cos", Variable("x"))^Number(2))) , Parse(Tokenizer.Tokenize("v = sin(x)^2 + cos(x)^2")));
        }

        [Test]
        public void MultipleFunctionsDeclaration()
        {
            var expressions = ParseMultiple(Tokenizer.Tokenize(@"A = 2
                                                                 B = 5"));
            var list = expressions.ToList();
            ExpressionAssert.AreEqual(FunctionDecl("A", Number(2)), list[0]);
            ExpressionAssert.AreEqual(FunctionDecl("B", Number(5)), list[1]);
        }

        [Test]
        public void GreaterThan()
        {
            ExpressionAssert.AreEqual(Number(3) > Number(2), Parse(Tokenizer.Tokenize("3 > 2")));
        }

        [Test]
        public void LessThan()
        {
            ExpressionAssert.AreEqual(Number(3) > Number(2), Parse(Tokenizer.Tokenize("3 > 2")));
        }

        [Test]
        public void GreaterThanAndAddition()
        {
            ExpressionAssert.AreEqual((Number(3) + Variable("x")) > (Number(2) + Variable("x")), Parse(Tokenizer.Tokenize("3 + x > 2 + x")));
        }

        [Test]
        public void LessThanAndAddition()
        {
            ExpressionAssert.AreEqual((Number(3) + Variable("x")) < (Number(2) + Variable("x")), Parse(Tokenizer.Tokenize("3 + x < 2 + x")));
        }

        [Test]
        public void GreaterThanAdditionAndMultiplication()
        {
            ExpressionAssert.AreEqual(((Number(3) + Variable("x")) * Number(2)) < (Number(2) + (Variable("x") * Number(3))), Parse(Tokenizer.Tokenize("(3 + x) * 2 < 2 + x * 3")));
        }

        [Test]
        public void ConditionalExpression()
        {
            ExpressionAssert.AreEqual(Conditional(Variable("x") > Number(1), Number(2), Number(3)), Parse(Tokenizer.Tokenize("(x > 1) ? 2 : 3")));
        }

        [Test]
        public void ConditionalExpressionWithAdditionSubtraction()
        {
            ExpressionAssert.AreEqual(Conditional(Variable("x") + Variable("y") > Number(1), Number(2) + Variable("x"), Number(3) - Variable("y")), Parse(Tokenizer.Tokenize("(x + y > 1) ? 2 + x : 3 - y")));
        }
    }
}