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
using SymbolicDifferentiation.Core.Tokens;

namespace SymbolicDifferentiation.Core.AST
{
    public class FunctionDeclarationExpression : Expression
    {
        private FunctionDeclarationExpression()
        {
        }

        public static Expression Create(Token name, Expression body)
        {
            return new FunctionDeclarationExpression { Name = name, Arguments = new Expression[0], Body = body };
        }

        public static Expression CreateWithArgs(Token name, IEnumerable<Expression> args, Expression body)
        {
            return new FunctionDeclarationExpression { Name = name, Arguments = args, Body = body };
        }

        public Token Name
        {
            set { Value = value; }
            get { return Value; }
        }

        public IEnumerable<Expression> Arguments { get; set; }

        public Expression Body { get; set; }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
