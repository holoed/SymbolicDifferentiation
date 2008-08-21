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
using SymbolicDifferentiation.Tokens;

namespace SymbolicDifferentiation.Extensions
{
    public static class TokensExtensions
    {
        public static IEnumerable<Token> Tokenize(this string input)
        {
            return Tokenizer.Tokenize(input);
        }

        public static IEnumerable<Token> DeSugar(this IEnumerable<Token> tokens)
        {
            return DeSugariser.This(tokens);
        }
    }
}