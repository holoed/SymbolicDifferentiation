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
using System.Linq.Expressions;
using NUnit.Framework;
using SymbolicDifferentiation.Extensions;
using SymbolicDifferentiation.Tokens;
using SymbolicDifferentiation.Visitors;
using Expression=SymbolicDifferentiation.AST.Expression;

namespace SymbolicDifferentiation.Tests
{
    [TestFixture]
    public class LinqExpressionVisitorTests
    {
        [Test]
        public void Costant()
        {
            var expression = new Expression {Value = TokenBuilder.Number(5)};
            var linqExp = ToLinqExpressionVisitor.GetExpression(expression, "x");
            Assert.IsInstanceOfType(typeof(ConstantExpression), linqExp.Body);
            var constant = (ConstantExpression) linqExp.Body;
            Assert.AreEqual(ExpressionType.Constant, constant.NodeType);
            Assert.AreEqual(5, constant.Value);
            Assert.AreEqual(typeof(Double), constant.Type);
        }

        [Test]
        public void Variable()
        {
            var expression = new Expression { Value = TokenBuilder.Variable("x") };
            var linqExp = ToLinqExpressionVisitor.GetExpression(expression, "x");
            Assert.IsInstanceOfType(typeof(ParameterExpression), linqExp.Body);
            var parameter = (ParameterExpression)linqExp.Body;
            Assert.AreEqual(ExpressionType.Parameter, parameter.NodeType);
            Assert.AreEqual("x", parameter.Name);
            Assert.AreEqual(typeof(Double), parameter.Type);
        }

        [Test]
        public void AddBinaryExpression()
        {
            var expression = new Expression {Value = TokenBuilder.Variable("x")} +
                             new Expression {Value = TokenBuilder.Number(5)};
            var linqExp = ToLinqExpressionVisitor.GetExpression(expression, "x");
            Assert.IsInstanceOfType(typeof(BinaryExpression), linqExp.Body);
            var binaryExpression = (BinaryExpression)linqExp.Body;
            AssertBinary(binaryExpression, ExpressionType.Add);
            AssertConstant(((ConstantExpression)binaryExpression.Right), 5);
            AssertParameter((ParameterExpression)binaryExpression.Left, "x");
        }

        [Test]
        public void MultiplyBinaryExpression()
        {
            var expression = new Expression { Value = TokenBuilder.Variable("x") } *
                             new Expression { Value = TokenBuilder.Number(5) };
            var linqExp = ToLinqExpressionVisitor.GetExpression(expression, "x");
            Assert.IsInstanceOfType(typeof(BinaryExpression), linqExp.Body);
            var binaryExpression = (BinaryExpression)linqExp.Body;
            AssertBinary(binaryExpression, ExpressionType.Multiply);
            AssertConstant(((ConstantExpression)binaryExpression.Right), 5);
            AssertParameter((ParameterExpression)binaryExpression.Left, "x");
        }

        [Test]
        public void PowerBinaryExpression()
        {
            var expression = new Expression { Value = TokenBuilder.Variable("x") } ^
                             new Expression { Value = TokenBuilder.Number(5) };
            var linqExp = ToLinqExpressionVisitor.GetExpression(expression, "x");
            Assert.IsInstanceOfType(typeof(BinaryExpression), linqExp.Body);
            var binaryExpression = (BinaryExpression)linqExp.Body;
            AssertBinary(binaryExpression, ExpressionType.Power);
            AssertConstant(((ConstantExpression)binaryExpression.Right), 5);
            AssertParameter((ParameterExpression)binaryExpression.Left,"x");
        }

        [Test]
        public void AddMultiplyPrecedence()
        {
            var expression = new Expression { Value = TokenBuilder.Number(6) } +
                            (new Expression { Value = TokenBuilder.Number(4) } *
                            new Expression { Value = TokenBuilder.Number(3) });
            var linqExp = ToLinqExpressionVisitor.GetExpression(expression, "x");
            Assert.IsInstanceOfType(typeof(BinaryExpression), linqExp.Body);
            var binaryExpression = (BinaryExpression)linqExp.Body;
            AssertBinary(binaryExpression, ExpressionType.Add);
            AssertConstant(((ConstantExpression)binaryExpression.Left), 6);
            var right = (BinaryExpression) binaryExpression.Right;
            AssertBinary(right, ExpressionType.Multiply);
            AssertConstant(((ConstantExpression)right.Left), 4);
            AssertConstant(((ConstantExpression)right.Right), 3);
        }

        [Test]
        public void CSharpExpressionMatchesLinqExpressionValue()
        {
            var input = "2*x ^ 4 + 5*x ^ 3 + 3*x ^ 2 + 2*x + 1";
            var expression = Tokenizer.Tokenize(input).Parse();
            var linqExp = ToLinqExpressionVisitor.GetExpression(expression, "x");

            var func = linqExp.Compile();

            Func<double, double> f = x => 2 * Math.Pow(x, 4) + 5 * Math.Pow(x, 3) + 3 * Math.Pow(x, 2) + 2 * x + 1;

            for (double i = 0; i < 100; i+=.1)            
                Assert.AreEqual(f(i), func(i));
        }

        private static void AssertBinary(System.Linq.Expressions.Expression binaryExpression, ExpressionType type)
        {
            Assert.AreEqual(type, binaryExpression.NodeType);
            Assert.AreEqual(typeof(Double), binaryExpression.Type);
        }

        private static void AssertParameter(ParameterExpression parameter, string name)
        {
            Assert.AreEqual(ExpressionType.Parameter, parameter.NodeType);
            Assert.AreEqual(name, parameter.Name);
            Assert.AreEqual(typeof(Double), parameter.Type);
        }

        private static void AssertConstant(ConstantExpression constant, double value)
        {
            Assert.AreEqual(ExpressionType.Constant, constant.NodeType);
            Assert.AreEqual(value, constant.Value);
            Assert.AreEqual(typeof(Double), constant.Type);
        }
    }
}
