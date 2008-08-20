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

namespace SymbolicDifferentiation.Tokens
{
    public static class TokenBuilder
    {
        public static Token Number(double value)
        {
            return new Token(MatchType.Number, value);
        }

        public static Token Variable(string value)
        {
            return new Token(MatchType.Variable, value);
        }

        public static Token Symbol(string value)
        {
            return new Token(MatchType.Symbol, value);
        }
    }
}