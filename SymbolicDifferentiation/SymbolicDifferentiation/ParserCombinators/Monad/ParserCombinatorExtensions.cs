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

namespace SymbolicDifferentiation.ParserCombinators.Monad
{
    public static class ParserCombinatorExtensions
    {
        public static Parser<TInput, TValue> OR<TInput, TValue>(this Parser<TInput, TValue> parser1, Parser<TInput, TValue> parser2)
        {
            return input => parser1(input) ?? parser2(input);
        }
        public static Parser<TInput, TValue2> AND<TInput, TValue1, TValue2>(this Parser<TInput, TValue1> parser1, Parser<TInput, TValue2> parser2)
        {
            return input => parser2(parser1(input).Rest);
        }
    }
}