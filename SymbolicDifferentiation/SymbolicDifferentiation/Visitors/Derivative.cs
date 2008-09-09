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
using Microsoft.FSharp.Core;
using SymbolicDifferentiation.Core.AST;
using SymbolicDifferentiation.Core.Tokens;
using SymbolicDifferentiation.Extensions;

namespace SymbolicDifferentiation.Visitors
{
    public class Derivative : IExpressionVisitor<Unit>
    {
        private readonly Stack<Expression> _stack = new Stack<Expression>();

        Unit IExpressionVisitor<Unit>.Visit(BinaryExpression expression)
        {
            HandleAddition(expression);
            HandleMultiplication(expression);
            HandlePower(expression);
            return default(Unit);
        }

        Unit IExpressionVisitor<Unit>.Visit(Expression expression)
        {
            Token token;
            if (expression.Value.Type == MatchType.Number)
                token = TokenBuilder.Number(0);
            else if (expression.Value.Type == MatchType.Variable)
                token = TokenBuilder.Number(1);
            else
                return default(Unit);

            _stack.Push(new Expression {Value = token});
            return default(Unit);
        }

        public static Expression Deriv(Expression input)
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