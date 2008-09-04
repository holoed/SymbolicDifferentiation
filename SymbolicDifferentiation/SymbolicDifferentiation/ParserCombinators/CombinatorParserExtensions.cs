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
using System.Collections.Generic;
using System.Linq;
using SimbolicDifferentiation.Core.Tokens;
using SymbolicDifferentiation.AST;

namespace SymbolicDifferentiation.ParserCombinators
{
    // representation type for parsers
    public delegate Consumed<T> P<T>(ParserState input);

    public static class CombinatorParserExtensions
    {
        public static readonly P<Expression> DigitVal = from c in Sat(Token.IsLetterOrDigit) select new Expression { Value = c };

        public static ParseResult<T> Parse<T>(this P<T> p, IEnumerable<Token> toParse)
        {
            return p(new ParserState(0, toParse)).ParseResult;
        }

        public static P<U> Then<T, U>(this P<T> p1, Func<T, P<U>> f)
        {
            return input =>
            {
                var consumed1 = p1(input);
                if (consumed1.ParseResult.Succeeded)
                {
                    var consumed2 =
                        f(consumed1.ParseResult.Result)(consumed1.ParseResult.RemainingInput);
                    return new Consumed<U>(consumed1.HasConsumedInput || consumed2.HasConsumedInput,
                                           consumed2.HasConsumedInput
                                               ? consumed2.ParseResult
                                               : consumed2.ParseResult.MergeError(
                                                     consumed1.ParseResult.ErrorInfo));
                }
                else
                {
                    return new Consumed<U>(consumed1.HasConsumedInput,
                                           new ParseResult<U>(consumed1.ParseResult.ErrorInfo));
                }
            };
        }

        public static P<T> Or<T>(this P<T> p1, P<T> p2)
        {
            return input =>
            {
                var consumed1 = p1(input);
                if (consumed1.ParseResult.Succeeded || consumed1.HasConsumedInput)
                    return consumed1;
                else
                {
                    var consumed2 = p2(input);
                    return consumed2.HasConsumedInput
                               ? consumed2
                               : new Consumed<T>(
                                   consumed2.HasConsumedInput,
                                   consumed2.ParseResult.MergeError(consumed1.ParseResult.ErrorInfo));
                }
            };
        }

        public static P<T> Return<T>(this T x)
        {
            return input => new Consumed<T>(false, new ParseResult<T>(x, input, new ErrorInfo(input.Position)));
        }

        public static P<T> Tag<T>(this P<T> p, string label)
        {
            return input => p(input).Tag(label);
        }

        public static P<T> Chainl1<T>(this P<T> p, P<Func<T, T, T>> op)
        {
            return p.Then(x => Chainl1Helper(x, p, op));
        }

        public static P<T> Chainr1<T>(this P<T> p, P<Func<T, T, T>> op)
        {
            return p.Then(x => (op.Then(f => Chainr1(p, op).Then(y => f(x, y).Return()))).Or(x.Return()));
        }

        private static P<T> Chainl1Helper<T>(T x, P<T> p, P<Func<T, T, T>> op)
        {
            return op.Then(f => p.Then(y => Chainl1Helper(f(x, y), p, op))).Or(x.Return());
        }

        private static P<Token> Sat(Predicate<Token> pred)
        {
            return input =>
                input.Position >= input.Input.Count() ?
                new Consumed<Token>(false,
                    new ParseResult<Token>(
                        new ErrorInfo(input.Position,
                            Enumerable.Empty<string>(),
                            "unexpected end of input"))) :
                (!pred(input.Input.ElementAt(input.Position)) ?
                new Consumed<Token>(false,
                    new ParseResult<Token>(
                        new ErrorInfo(input.Position,
                            Enumerable.Empty<string>(),
                            "unexpected character '" + input.Input.ElementAt(input.Position) + "'"))) :
                            new Consumed<Token>(true,
                                new ParseResult<Token>(
                                    input.Input.ElementAt(input.Position),
                                    new ParserState(input.Position + 1, input.Input), new ErrorInfo(input.Position + 1))));
        }


        public static P<Token> Literal(this Token c)
        {
            return Sat(x => x.Equals(c)).Tag("character '" + c + "'");
        }

        public static IEnumerable<T> Cons<T>(T x, IEnumerable<T> rest)
        {
            yield return x;
            foreach (var t in rest)
                yield return t;
        }
    }
}