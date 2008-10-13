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
using SymbolicDifferentiation.Core.AST;
using System.Linq;

namespace SymbolicDifferentiation.Visitors
{
    public class ExpressionEqualityComparer : IExpressionVisitor<bool>
    {
        private readonly Queue<Expression> _stack = new Queue<Expression>();
        private bool _enabled;

        public bool Visit(FunctionDeclarationExpression expression)
        {
            if (_enabled)
            {
                if (_stack.Count == 0)
                    return false;
                var expected = _stack.Dequeue() as FunctionDeclarationExpression;
                if (expected == null) return false;
                if (!Equals(expected.Name, expression.Name))
                    return false;
                if (!Equals(expected.Arguments.Count(), expression.Arguments.Count()))
                    return false;
            }
            else
                _stack.Enqueue(expression);

            foreach (var argument in expression.Arguments)
                if (!argument.Accept(this)) return false;
            return expression.Body.Accept(this);
        }

        public bool Visit(FunctionApplicationExpression expression)
        {
            if (_enabled)
            {
                if (_stack.Count == 0)
                    return false;
                var expected = _stack.Dequeue() as FunctionApplicationExpression;
                if (expected == null) return false;
                if (!Equals(expected.Value, expression.Name))
                    return false;
                if (!Equals(expected.Arguments.Count(), expression.Arguments.Count()))
                    return false;
            }
            else
                _stack.Enqueue(expression);

            foreach (var argument in expression.Arguments)
                if (!argument.Accept(this)) return false;
            return true;
        }

        public bool Visit(BinaryExpression expression)
        {
            if (_enabled)
            {
                if (_stack.Count == 0)
                    return false;
                var expected = _stack.Dequeue() as BinaryExpression;
                if (expected == null) return false;
                if (!Equals(expected.Operator, expression.Operator))
                    return false;
            }
            else
                _stack.Enqueue(expression);

            return expression.Left.Accept(this) && expression.Right.Accept(this);
        }

        public bool Visit(Expression expression)
        {
            if (_enabled)
            {
                if (_stack.Count == 0)
                    return false;
                var expected = _stack.Dequeue();
                if (!Equals(expected.Value, expression.Value))
                    return false;
            }
            else
                _stack.Enqueue(expression);
            return true;
        }

        public static bool AreEqual(Expression expected, Expression actual)
        {
            var assert = new ExpressionEqualityComparer();
            expected.Accept(assert);
            assert._enabled = true;
            return actual.Accept(assert);
        }
    }
}