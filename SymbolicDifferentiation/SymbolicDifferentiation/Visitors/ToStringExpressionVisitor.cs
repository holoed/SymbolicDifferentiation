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
using System.IO;
using SymbolicDifferentiation.AST;

namespace SymbolicDifferentiation.Visitors
{
    public class ToStringExpressionVisitor : IExpressionVisitor
    {
        private readonly bool _grouping;
        private readonly StringWriter _writer;

        public ToStringExpressionVisitor(bool grouping)
        {
            _grouping = grouping;
            _writer = new StringWriter();
        }

        public string Result
        {
            get { return _writer.ToString().Trim(); }
        }

        public void Visit(BinaryExpression expression)
        {
            var action = new Action(() =>
                                        {
                                            expression.Left.Accept(this);
                                            _writer.Write(expression.Operator.Value);
                                            expression.Right.Accept(this);
                                        });

            if (_grouping)
            {
                _writer.Write("(");
                action();
                _writer.Write(")");
            }
            else
                action();
        }

        public void Visit(Expression expression)
        {
            _writer.Write(" {0} ", expression.Value);
        }
    }
}