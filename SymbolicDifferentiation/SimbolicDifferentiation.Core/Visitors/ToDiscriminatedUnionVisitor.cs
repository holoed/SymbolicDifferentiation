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
using SymbolicDifferentiation.Core.Tokens;

namespace SymbolicDifferentiation.Core.Visitors
{
    public class ToDiscriminatedUnion : IExpressionVisitor
    {
        private readonly Stack<FS_AbstractSyntaxTree.Expression> _stack = new Stack<FS_AbstractSyntaxTree.Expression>();

        public void Visit(BinaryExpression expression)
        {
            expression.Right.Accept(this);
            expression.Left.Accept(this);
            if (expression.Operator.Equals(TokenBuilder.Symbol("+")))
                _stack.Push(new FS_AbstractSyntaxTree.Expression._Add(_stack.Pop(), _stack.Pop()));
            if (expression.Operator.Equals(TokenBuilder.Symbol("*")))
                _stack.Push(new FS_AbstractSyntaxTree.Expression._Mul(_stack.Pop(), _stack.Pop()));
            if (expression.Operator.Equals(TokenBuilder.Symbol("^")))
                _stack.Push(new FS_AbstractSyntaxTree.Expression._Pow(_stack.Pop(), _stack.Pop().Number1));
        }

        public void Visit(Expression expression)
        {
            if (expression.IsNumber)
                _stack.Push(new FS_AbstractSyntaxTree.Expression._Number((double)expression.Value.Value));
            else
                _stack.Push(new FS_AbstractSyntaxTree.Expression._Variable((string)expression.Value.Value));
        }

        public FS_AbstractSyntaxTree.Expression Result
        {
            get { return _stack.Pop(); }
        }
    }
}
