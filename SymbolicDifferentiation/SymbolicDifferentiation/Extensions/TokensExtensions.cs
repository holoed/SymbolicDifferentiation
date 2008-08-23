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
using SymbolicDifferentiation.Tokens;

namespace SymbolicDifferentiation.Extensions
{
    public static class TokensExtensions
    {
        public static IEnumerable<Token> Tokenize(this string input)
        {
            return Tokenizer.Tokenize(input);
        }

        public static IEnumerable<Token> Expand(this IEnumerable<Token> tokens)
        {
            return Sugar.Expand(tokens);
        }

        public static IEnumerable<Token> Shrink(this IEnumerable<Token> tokens)
        {
            return Sugar.Shrink(tokens);
        }

        public static string ToStringExpression(this IEnumerable<Token> tokens)
        {
            return string.Join("", tokens.Select(token => token.ToString()).ToArray());
        }
    }
}