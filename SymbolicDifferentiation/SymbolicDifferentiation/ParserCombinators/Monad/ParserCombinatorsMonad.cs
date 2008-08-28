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

namespace SymbolicDifferentiation.ParserCombinators.Monad
{
    public static class ParserCombinatorsMonad
    {
        // By providing Select, Where and SelectMany methods on Parser<TInput,TValue> we make the 
        // C# Query expression syntax available for manipulating Parsers.  
        public static Parser<TInput, TValue> Where<TInput, TValue>(this Parser<TInput, TValue> parser, Func<TValue, bool> pred)
        {
            return input =>
                       {
                           var res = parser(input);
                           if (res == null || !pred(res.Value)) return null;
                           return res;                
                       };
        }
        public static Parser<TInput, TValue2> Select<TInput, TValue, TValue2>(this Parser<TInput, TValue> parser, Func<TValue, TValue2> selector)
        {
            return input =>
                       {
                           var res = parser(input);
                           if (res == null) return null;
                           return new Result<TInput, TValue2>(selector(res.Value), res.Rest);
                       };
        }
        public static Parser<TInput, TValue2> SelectMany<TInput, TValue, TIntermediate, TValue2>(this Parser<TInput, TValue> parser, Func<TValue, Parser<TInput, TIntermediate>> selector, Func<TValue, TIntermediate, TValue2> projector)
        {
            return input =>
                       {
                           var res = parser(input);
                           if (res == null) return null;
                           var val = res.Value;
                           var res2 = selector(val)(res.Rest);
                           if (res2 == null) return null;
                           return new Result<TInput, TValue2>(projector(val, res2.Value), res2.Rest);
                       };
        }
    }
}