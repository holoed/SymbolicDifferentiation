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
using SymbolicDifferentiation.Core.AST;
using SymbolicDifferentiation.Core.Tokens;

namespace SymbolicDifferentiation.Visitors
{
    public class BasicParser : IExpressionVisitor
    {
        private readonly Token _addition = TokenBuilder.Symbol("+");
        private readonly Token _exponentiation = TokenBuilder.Symbol("^");
        private readonly Token _multiplication = TokenBuilder.Symbol("*");
        private readonly Stack<Expression> _stack = new Stack<Expression>();

        public void Visit(BinaryExpression expression)
        {
            _stack.Push(expression);
        }

        public void Visit(Expression expression)
        {
            if (_stack.Count > 0 && _stack.Peek().IsSymbol)
            {
                Expression symbol = _stack.Pop();
                _stack.Push(expression);
                _stack.Push(symbol);
            }
            else
                _stack.Push(expression);
        }

        public static Expression Parse(IEnumerable<Token> tokens)
        {
            IEnumerable<Expression> expressions = tokens.Select(token => new Expression {Value = token});
            var parser = new BasicParser();
            return parser.Parse(expressions);
        }

        private Expression Parse(IEnumerable<Expression> expressions)
        {
            return expressions.Aggregate(ComposeExpressions);
        }

        private Expression ComposeExpressions(Expression arg1, Expression arg2)
        {
            arg1.Accept(this);
            arg2.Accept(this);
            ComputeStack();
            return _stack.Pop();
        }

        private void ComputeStack()
        {
            if (_stack.Count > 2 && _stack.Peek().IsSymbol)
            {
                Expression op = _stack.Pop();
                Expression right = _stack.Pop();
                Expression left = _stack.Pop();
                Expression rightMost = GetRightMost(left);

                if ((left.Value.Equals(_addition) &&
                     op.Value.Equals(_multiplication)) ||
                    (rightMost.Value.Equals(_multiplication) &&
                     op.Value.Equals(_exponentiation)))
                {
                    var binExp = (BinaryExpression) rightMost;
                    binExp.Right = Binary(op, binExp.Right, right);
                    _stack.Push(left);
                }
                else
                    _stack.Push(Binary(op, left, right));
            }
        }

        private static BinaryExpression Binary(Expression op, Expression left, Expression right)
        {
            return new BinaryExpression {Operator = op.Value, Left = left, Right = right};
        }

        private static Expression GetRightMost(Expression expression)
        {
            var next = expression as BinaryExpression;
            while (next != null && next.Right != null && next.Right is BinaryExpression)
            {
                next = next.Right as BinaryExpression;
            }
            return next ?? expression;
        }
    }
}