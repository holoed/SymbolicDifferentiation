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
using SymbolicDifferentiation.Tokens;

namespace SymbolicDifferentiation.Visitors
{
    public class Simplifier : IExpressionVisitor
    {
        private readonly Expression _one = new Expression {Value = TokenBuilder.Number(1)};
        private readonly Stack<Expression> _stack = new Stack<Expression>();
        private readonly Expression _zero = new Expression {Value = TokenBuilder.Number(0)};

        public void Visit(BinaryExpression expression)
        {
            expression.Left.Accept(this);
            expression.Right.Accept(this);

            Expression right = _stack.Pop();
            Expression left = _stack.Pop();

            if (HandleMultiplicationByZero(expression, left, right)) return;
            if (HandleAdditionToZero(expression, left, right)) return;
            if (HandleMultiplicationByOne(expression, left, right)) return;
            if (HandleRaiseToPowerOne(expression, left, right)) return;
            if (HandleSimpleOperation(expression, left, right)) return;
            if (HandleDoubleMultiplicationOperation(expression, left, right)) return;

            _stack.Push(new BinaryExpression
                            {
                                Operator = expression.Operator,
                                Left = left,
                                Right = right
                            });
        }

        public void Visit(Expression expression)
        {
            _stack.Push(expression);
        }

        public static Expression Simplify(Expression expression)
        {
            Expression simplifiedExpression;
            while (true)
            {
                var simplifier = new Simplifier();
                expression.Accept(simplifier);
                simplifiedExpression = simplifier._stack.Pop();
                if (ExpressionEqualityComparer.AreEqual(simplifiedExpression, expression))
                    break;
                expression = simplifiedExpression;
            }

            return simplifiedExpression;
        }

        private bool HandleDoubleMultiplicationOperation(BinaryExpression expression, Expression left, Expression right)
        {
            if (expression.Operator.Equals(TokenBuilder.Symbol("*")))
            {
                var binExp = right as BinaryExpression;
                if (binExp != null && binExp.Left.IsNumber && binExp.Operator.Equals(TokenBuilder.Symbol("*")))
                {
                    _stack.Push(Solve(expression.Operator, left, binExp.Left)*binExp.Right);
                    return true;
                }
            }
            return false;
        }

        private bool HandleSimpleOperation(BinaryExpression expression, Expression left, Expression right)
        {
            if (expression.Operator.Equals(TokenBuilder.Symbol("*")) ||
                expression.Operator.Equals(TokenBuilder.Symbol("+")))
            {
                if (left.IsNumber && right.IsNumber)
                {
                    _stack.Push(Solve(expression.Operator, left, right));
                    return true;
                }
            }
            return false;
        }

        private bool HandleRaiseToPowerOne(BinaryExpression expression, Expression left, Expression right)
        {
            if (expression.Operator.Equals(TokenBuilder.Symbol("^")))
            {
                if (!left.Equals(_one) && right.Equals(_one))
                {
                    _stack.Push(left);
                    return true;
                }
            }
            return false;
        }

        private bool HandleMultiplicationByOne(BinaryExpression expression, Expression left, Expression right)
        {
            if (expression.Operator.Equals(TokenBuilder.Symbol("*")))
            {
                if (left.Equals(_one) && !right.Equals(_one))
                {
                    _stack.Push(right);
                    return true;
                }
                if (!left.Equals(_one) && right.Equals(_one))
                {
                    _stack.Push(left);
                    return true;
                }
            }
            return false;
        }

        private bool HandleAdditionToZero(BinaryExpression expression, Expression left, Expression right)
        {
            if (expression.Operator.Equals(TokenBuilder.Symbol("+")))
            {
                if (left.Equals(_zero) && !right.Equals(_zero))
                {
                    _stack.Push(right);
                    return true;
                }
                if (!left.Equals(_zero) && right.Equals(_zero))
                {
                    _stack.Push(left);
                    return true;
                }
            }
            return false;
        }

        private bool HandleMultiplicationByZero(BinaryExpression expression, Expression left, Expression right)
        {
            if (expression.Operator.Equals(TokenBuilder.Symbol("*")) &&
                (left.Equals(_zero) || right.Equals(_zero)))
            {
                _stack.Push(_zero);
                return true;
            }
            return false;
        }

        private static Expression Solve(Token op, Expression left, Expression right)
        {
            if (op.Equals(TokenBuilder.Symbol("+")))
                return new Expression {Value = left.Value + right.Value};
            if (op.Equals(TokenBuilder.Symbol("*")))
                return new Expression {Value = left.Value*right.Value};
            throw new ArgumentOutOfRangeException("op", "Operation not supported");
        }
    }
}