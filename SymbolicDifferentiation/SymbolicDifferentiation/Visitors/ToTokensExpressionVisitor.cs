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

namespace SymbolicDifferentiation.Visitors
{
    public class ToTokensExpressionVisitor : IExpressionVisitor<Unit>
    {
        private readonly bool _grouping;
        private readonly List<Token> _tokens;

        public ToTokensExpressionVisitor(bool grouping)
        {
            _grouping = grouping;
            _tokens = new List<Token>();
        }

        public IEnumerable<Token> Result
        {
            get { return _tokens; }
        }

        public Unit Visit(BinaryExpression expression)
        {
            var action = new Action(() =>
                                        {
                                            expression.Left.Accept(this);
                                            _tokens.Add(expression.Operator);
                                            expression.Right.Accept(this);
                                        });

            if (_grouping)
            {
                _tokens.Add(TokenBuilder.Symbol("("));
                action();
                _tokens.Add(TokenBuilder.Symbol(")"));
            }
            else
                action();
            return default(Unit);
        }

        public Unit Visit(Expression expression)
        {
            _tokens.Add(expression.Value);
            return default(Unit);
        }
    }
}