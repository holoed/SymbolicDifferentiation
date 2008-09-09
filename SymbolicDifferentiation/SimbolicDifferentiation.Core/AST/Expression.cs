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

using SymbolicDifferentiation.Core.AST;
using SymbolicDifferentiation.Core.Tokens;

namespace SymbolicDifferentiation.Core.AST
{
    public class Expression
    {
        public Token Value { get; set; }

        public Expression()
        {
        }

        public Expression(Token value) : this()
        {
            Value = value;
        }

        public virtual bool IsSymbol
        {
            get { return Value.Type == MatchType.Symbol; }
        }

        public virtual bool IsNumber
        {
            get { return Value.Type == MatchType.Number; }
        }

        public virtual void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public static Expression operator *(Expression left, Expression right)
        {
            return BuildBinary("*", left, right);
        }

        public static Expression operator /(Expression left, Expression right)
        {
            return BuildBinary("/", left, right);
        }

        public static Expression operator +(Expression left, Expression right)
        {
            return BuildBinary("+", left, right);
        }

        public static Expression operator -(Expression left, Expression right)
        {
            return BuildBinary("-", left, right);
        }

        public static Expression operator ^(Expression left, Expression right)
        {
            return BuildBinary("^", left, right);
        }

        private static Expression BuildBinary(string op, Expression left, Expression right)
        {
            return new BinaryExpression
                       {
                           Left = left,
                           Right = right,
                           Operator = new Token(MatchType.Symbol, op)
                       };
        }

        public override bool Equals(object obj)
        {
            var lit = obj as Expression;
            return lit != null && Equals(lit.Value, Value);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}