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

using System.Linq;

namespace SymbolicDifferentiation.ParserCombinators.Monad
{
    public abstract class Parsers<TInput>
    {
        public Parser<TInput, TValue> Succeed<TValue>(TValue value)
        {
            return input => new Result<TInput, TValue>(value, input);
        }
        public Parser<TInput, TValue[]> Rep<TValue>(Parser<TInput, TValue> parser)
        {
            return Rep1(parser).OR(Succeed(new TValue[0]));
        }
        public Parser<TInput, TValue[]> Rep1<TValue>(Parser<TInput, TValue> parser)
        {
            return from x in parser
                   from xs in Rep(parser)
                   select (new[] { x }).Concat(xs).ToArray();
        }
    }
}