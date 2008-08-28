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

namespace SymbolicDifferentiation.Tokens
{
    public enum MatchType
    {
        Number,
        Variable,
        Symbol,
        Whitespace,
        Keyword,
        EOF
    }

    public class Token
    {
        public static Token EOF = new Token(MatchType.EOF, "\0");
        public static Token Whitespace = new Token(MatchType.Whitespace, " ");

        public Token(MatchType type, object value)
            : this(type, value, 0, 0)
        { }

        public Token(MatchType type, object value, int index, int length)
        {
            Type = type;
            Value = value;
            Index = index;
            Length = length;
        }

        public MatchType Type { private set; get; }
        public object Value { private set; get; }
        public int Index { private set; get; }
        public int Length { private set; get; }

        public override bool Equals(object obj)
        {
            var token = obj as Token;
            if (token == null) return false;
            return Equals(token.Type, Type) && Equals(token.Value, Value);
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() ^ Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static Token operator *(Token left, Token right)
        {
            if (left.Type == MatchType.Number && right.Type == MatchType.Number)
                return new Token(MatchType.Number, ((double)left.Value) * ((double)right.Value));
            throw new ArgumentOutOfRangeException("Cannot multiply if both operands are not numbers");
        }

        public static Token operator +(Token left, Token right)
        {
            if (left.Type == MatchType.Number && right.Type == MatchType.Number)
                return new Token(MatchType.Number, ((double)left.Value) + ((double)right.Value));
            throw new ArgumentOutOfRangeException("Cannot add if both operands are not numbers");
        }

        public static bool IsLetter(Token token)
        {
            return 
                token.Type == MatchType.Variable || 
                token.Type == MatchType.Number ||
                token.Type == MatchType.Keyword;
        }

        public static bool IsLetterOrDigit(Token token)
        {
            return token.Type == MatchType.Variable || token.Type == MatchType.Number;
        }
    }
}
