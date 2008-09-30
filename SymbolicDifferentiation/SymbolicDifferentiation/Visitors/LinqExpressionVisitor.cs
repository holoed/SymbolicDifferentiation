#region License

/* ****************************************************************************
 * Authors: marten_range 
 * 
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
using System.Linq;
using System.Linq.Expressions;
using SymbolicDifferentiation.Core.AST;
using SymbolicDifferentiation.Core.Tokens;
using BinaryExpression=SymbolicDifferentiation.Core.AST.BinaryExpression;
using Expression=System.Linq.Expressions.Expression;

namespace SymbolicDifferentiation.Visitors
{
    public class ToLinqExpressionVisitor : IExpressionVisitor<Expression>
    {
        private static readonly Dictionary<Token, Func<Expression, Expression, Expression>> _token_handlers =
            new Dictionary<Token, Func<Expression, Expression, Expression>>();

        private readonly Dictionary<string, ParameterExpression> _args;

        static ToLinqExpressionVisitor()
        {
            _token_handlers[TokenBuilder.Symbol("+")] = Expression.Add;
            _token_handlers[TokenBuilder.Symbol("-")] = Expression.Subtract;
            _token_handlers[TokenBuilder.Symbol("*")] = Expression.Multiply;
            _token_handlers[TokenBuilder.Symbol("/")] = Expression.Divide;
            _token_handlers[TokenBuilder.Symbol("^")] = Expression.Power;
        }

        public ToLinqExpressionVisitor(IEnumerable<ParameterExpression> args)
        {
            _args = args.ToDictionary(x => x.Name);
        }

        public Expression Visit(FunctionDeclarationExpression expression)
        {
            throw new NotImplementedException();
        }

        public Expression Visit(FunctionApplicationExpression expression)
        {
            throw new NotImplementedException();
        }

        public Expression Visit(BinaryExpression expression)
        {
            var l_left = expression.Left.Accept(this);
            var l_right = expression.Right.Accept(this);
            return _token_handlers[expression.Operator](l_left, l_right);
        }

        public Expression Visit(Core.AST.Expression expression)
        {
            if (expression.IsNumber)
                return Expression.Constant(expression.Value.Value, typeof (double));
            return _args[(string) expression.Value.Value];
        }

        public static Expression GetExpression(Type delegateType, Core.AST.Expression exr, params string[] args)
        {
            var linqArgs = args.Select(x => Expression.Parameter(typeof (double), x)).ToArray();
            return Expression.Lambda(
                delegateType, 
                exr.Accept(new ToLinqExpressionVisitor(linqArgs)), 
                linqArgs);
        }

        public static Expression<Func<double, double>> GetExpression(Core.AST.Expression expression, string arg0)
        {
            return (Expression<Func<double, double>>) GetExpression(
                                                          typeof (Func<double, double>),
                                                          expression,
                                                          arg0);
        }
    }
}