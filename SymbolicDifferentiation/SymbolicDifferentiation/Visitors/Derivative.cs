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
using System.Collections.Generic;
using SymbolicDifferentiation.AST;
using SymbolicDifferentiation.Extensions;
using SymbolicDifferentiation.Tokens;

namespace SymbolicDifferentiation.Visitors
{
    public class Derivative : IExpressionVisitor
    {
        private readonly Stack<Expression> _stack = new Stack<Expression>();

        public static string Of(string input)
        {
            return Deriv(input.
                Tokenize().
                Expand().
                Parse()).
                Simplify().
                ToTokens(false).
                Shrink().
                ToStringExpression();
        }

        void IExpressionVisitor.Visit(BinaryExpression expression)
        {
            HandleAddition(expression);
            HandleMultiplication(expression);
            HandlePower(expression);
        }

        void IExpressionVisitor.Visit(Expression expression)
        {
            Token token;
            if (expression.Value.Type == MatchType.Number)
                token = TokenBuilder.Number(0);
            else if (expression.Value.Type == MatchType.Variable)
                token = TokenBuilder.Number(1);
            else
                return;

            _stack.Push(new Expression {Value = token});
        }

        private static Expression Deriv(Expression input)
        {
            var deriv = new Derivative();
            input.Accept(deriv);
            return deriv._stack.Pop();
        }

        private void HandleAddition(BinaryExpression expression)
        {
            if (expression.Operator.Equals(TokenBuilder.Symbol("+")))
                _stack.Push(Deriv(expression.Left) + Deriv(expression.Right));
        }

        private void HandleMultiplication(BinaryExpression expression)
        {
            if (expression.Operator.Equals(TokenBuilder.Symbol("*")))
                _stack.Push((expression.Left*Deriv(expression.Right)) + (Deriv(expression.Left)*expression.Right));
        }

        private void HandlePower(BinaryExpression expression)
        {
            if (expression.Operator.Equals(TokenBuilder.Symbol("^")))
                _stack.Push(expression.Right*(expression.Left ^
                                              new Expression
                                                  {
                                                      Value =
                                                          (TokenBuilder.Number(
                                                          Double.Parse(expression.Right.ToTokens(true).ToStringExpression()) - 1))
                                                  }));
        }
    }
}