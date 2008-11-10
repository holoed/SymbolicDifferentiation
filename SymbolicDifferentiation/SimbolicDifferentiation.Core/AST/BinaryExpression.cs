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
using SymbolicDifferentiation.Core.Tokens;

namespace SymbolicDifferentiation.Core.AST
{
    public class BinaryExpression : Expression
    {
        public Expression Left { set; get; }
        public Expression Right { set; get; }

        public Token Operator
        {
            set { Value = value; }
            get { return Value; }
        }

        public override bool IsSymbol
        {
            get { return false; }
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return String.Format("({0} {1} {2})", Left, Operator, Right);
        }
    }
}