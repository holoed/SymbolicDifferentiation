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
using SymbolicDifferentiation.AST;
using SymbolicDifferentiation.Tokens;
using BinaryExpression=SymbolicDifferentiation.AST.BinaryExpression;
using Expression=System.Linq.Expressions.Expression;

namespace SymbolicDifferentiation.Visitors
{
    using SLE = System.Linq.Expressions;

    public class ToLinqExpressionVisitor : IExpressionVisitor
    {
        private static readonly Dictionary<Token, Func<Expression, Expression, Expression>> _token_handlers =
            new Dictionary<Token, Func<Expression, Expression, Expression>>();

        private readonly Dictionary<string, ParameterExpression> _args;
        private readonly Stack<Expression> _stack = new Stack<Expression>();

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

        public void Visit(BinaryExpression expression)
        {
            expression.Left.Accept(this);
            expression.Right.Accept(this);

            Expression l_right = _stack.Pop();
            Expression l_left = _stack.Pop();

            _stack.Push(_token_handlers[expression.Operator](l_left, l_right));
        }

        public void Visit(AST.Expression expression)
        {
            if (expression.IsNumber)
                _stack.Push(Expression.Constant(expression.Value.Value, typeof (double)));
            else
                _stack.Push(_args[(string) expression.Value.Value]);
        }

        public static Expression GetExpression(
            Type delegateType,
            AST.Expression exr,
            params string[] args)
        {
            ParameterExpression[] linqArgs = args.Select(x => Expression.Parameter(typeof (double), x)).ToArray();

            var linqVisitor = new ToLinqExpressionVisitor(linqArgs);

            exr.Accept(linqVisitor);
            return Expression.Lambda(delegateType, linqVisitor._stack.Pop(), linqArgs);
        }

        public static Expression<Func<double, double>> GetExpression(
            AST.Expression expression,
            string arg0)
        {
            return (Expression<Func<double, double>>) GetExpression(
                                                          typeof (Func<double, double>),
                                                          expression,
                                                          arg0);
        }
    }
}