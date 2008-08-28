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
using SymbolicDifferentiation.ParserCombinators.Monad;
using SymbolicDifferentiation.Tokens;

namespace SymbolicDifferentiation.ParserCombinators
{
    public abstract class TokenParser<TInput> : Parsers<TInput>
    {
        protected abstract Parser<TInput, Token> AnyToken { get; }

        protected Parser<TInput, Token> ParseToken(Token token)
        {
            return from c in AnyToken where c.Equals(token) select c;
        }

        protected Parser<TInput, Token> ParseToken(Predicate<Token> pred)
        {
            return from token in AnyToken where pred(token) select token;
        }
    }
}
