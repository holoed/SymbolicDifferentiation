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
using Microsoft.FSharp.Core;
using SymbolicDifferentiation.Core.AST;

namespace SymbolicDifferentiation.Visitors
{
    public class ExpressionEqualityComparer : IExpressionVisitor<Unit>
    {
        private readonly Queue<Expression> _stack = new Queue<Expression>();
        private bool _enabled;
        private bool _equals = true;

        public Unit Visit(BinaryExpression expression)
        {
            if (_enabled)
            {
                if (_stack.Count == 0)
                {
                    _equals = false;
                    return default(Unit);
                }
                Expression expectedOperator = _stack.Dequeue();
                if (!Equals(expectedOperator.Value, expression.Operator))
                {
                    _equals = false;
                    return default(Unit);
                }
            }
            else
                _stack.Enqueue(new Expression {Value = expression.Operator});

            expression.Left.Accept(this);
            expression.Right.Accept(this);
            return default(Unit);
        }

        public Unit Visit(Expression expression)
        {
            if (_enabled)
            {
                if (_stack.Count == 0)
                {
                    _equals = false;
                    return default(Unit);
                }
                Expression expected = _stack.Dequeue();
                if (!Equals(expected.Value, expression.Value))
                {
                    _equals = false;
                    return default(Unit);
                }
            }
            else
                _stack.Enqueue(expression);
            return default(Unit);
        }

        public static bool AreEqual(Expression expected, Expression actual)
        {
            var assert = new ExpressionEqualityComparer();
            expected.Accept(assert);
            assert._enabled = true;
            actual.Accept(assert);
            return assert._equals;
        }
    }
}