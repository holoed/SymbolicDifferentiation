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
using SymbolicDifferentiation.Core.AST;
using SymbolicDifferentiation.Core.Tokens;
using SymbolicDifferentiation.Extensions;

namespace SymbolicDifferentiation.Visitors
{
    public class Derivative : IExpressionVisitor<Expression>
    {
        Expression IExpressionVisitor<Expression>.Visit(BinaryExpression expression)
        {
            if (IsOperation(expression, "+"))
                return Deriv(expression.Left) + Deriv(expression.Right);
            if (IsOperation(expression, "*"))
                return (expression.Left*Deriv(expression.Right)) + (Deriv(expression.Left)*expression.Right);
            if (IsOperation(expression, "^"))
                return expression.Right*(expression.Left ^ BuildExpRight(expression));
            return default(Expression);
        }

        private static Expression BuildExpRight(BinaryExpression expression)
        {
            return new Expression { Value = (TokenBuilder.Number(Double.Parse(expression.Right.ToTokens(true).ToStringExpression()) - 1))};
        }

        private static bool IsOperation(BinaryExpression expression, string op)
        {
            return expression.Operator.Equals(TokenBuilder.Symbol(op));
        }

        Expression IExpressionVisitor<Expression>.Visit(Expression expression)
        {
            Token token;
            if (expression.Value.Type == MatchType.Number)
                token = TokenBuilder.Number(0);
            else if (expression.Value.Type == MatchType.Variable)
                token = TokenBuilder.Number(1);
            else
                return default(Expression);

            return new Expression {Value = token};
        }

        public static Expression Deriv(Expression input)
        {
            var deriv = new Derivative();
            return input.Accept(deriv);
        }
    }
}
