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

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}