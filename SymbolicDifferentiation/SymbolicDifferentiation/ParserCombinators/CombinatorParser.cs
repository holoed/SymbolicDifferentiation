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
using SymbolicDifferentiation.AST;
using SymbolicDifferentiation.Tokens;

namespace SymbolicDifferentiation.ParserCombinators
{
    // representation type for parsers
    public delegate Consumed<T> P<T>(ParserState input);

    public static class CombinatorParser
    {
        private static readonly P<Expression> DigitVal = Sat(Token.IsLetterOrDigit).Then(c => Return((new Expression { Value = c })));
        public static IEnumerable<T> Cons<T>(T x, IEnumerable<T> rest)
        {
            yield return x;
            foreach (T t in rest)
                yield return t;
        }

        private static ParseResult<T> Parse<T>(this P<T> p, IEnumerable<Token> toParse)
        {
            return p(new ParserState(0, toParse)).ParseResult;
        }

        private static P<T> Return<T>(T x)
        {
            return input => new Consumed<T>(false, new ParseResult<T>(x, input, new ErrorInfo(input.Position)));
        }

        private static P<Func<T, T, T>> Return<T>(Func<T, T, T> f)
        {
            return Return<Func<T, T, T>>(f);
        }

        private static P<U> Then_<T, U>(this P<T> p1, P<U> p2)
        {
            return p1.Then(dummy => p2);
        }

        private static P<U> Then<T, U>(this P<T> p1, Func<T, P<U>> f)
        {
            return input =>
            {
                Consumed<T> consumed1 = p1(input);
                if (consumed1.ParseResult.Succeeded)
                {
                    Consumed<U> consumed2 =
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

        private static P<T> Or<T>(this P<T> p1, P<T> p2)
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

        private static P<T> Tag<T>(this P<T> p, string label)
        {
            return input => p(input).Tag(label);
        }

        // Sat(pred) succeeds parsing a character only if the character matches the predicate
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


        private static P<Token> Literal(Token c)
        {
            return Sat(x => x.Equals(c)).Tag("character '" + c + "'");
        }

        private static P<T> Chainl1<T>(this P<T> p, P<Func<T, T, T>> op)
        {
            return p.Then(x =>Chainl1Helper(x, p, op));
        }

        public static P<T> Chainr1<T>(this P<T> p, P<Func<T, T, T>> op)
        {
            return p.Then(x => ( op.Then(f => Chainr1(p, op).Then(y => Return(f(x, y))))).Or(Return(x)));
        }

        private static P<T> Chainl1Helper<T>(T x, P<T> p, P<Func<T, T, T>> op)
        {
            return op.Then(f => p.Then(y => Chainl1Helper(f(x, y), p, op))).Or(Return(x));
        }

        public static Expression Parse(IEnumerable<Token> input)
        {
            var addOp =
                    Literal(TokenBuilder.Symbol("+")).Then_(Return((Expression x, Expression y) => x + y))
                    .Or(
                    Literal(TokenBuilder.Symbol("-")).Then_(Return((Expression x, Expression y) => x - y)))
                    .Tag("add/subtract op");

            var mulOp =
                Literal(TokenBuilder.Symbol("*")).Then_(Return((Expression x, Expression y) => x * y))
                .Or(
                Literal(TokenBuilder.Symbol("/")).Then_(Return((Expression x, Expression y) => x / y)))
                .Tag("multiply/divide op");

            var expOp = Literal(TokenBuilder.Symbol("^")).Then_(Return((Expression x, Expression y) => x ^ y)).
                Tag("exponentiation op");

            var factor = DigitVal.Chainr1(expOp);
            var term = factor.Chainl1(mulOp);
            var expr = term.Chainl1(addOp);


            return expr.Parse(input).Result;
        }
    }
}