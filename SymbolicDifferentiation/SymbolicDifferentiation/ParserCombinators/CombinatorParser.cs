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
        private static readonly P<Expression> DigitVal = from c in Sat(Token.IsLetterOrDigit) select new Expression { Value = c };

        public static IEnumerable<T> Cons<T>(T x, IEnumerable<T> rest)
        {
            yield return x;
            foreach (T t in rest)
                yield return t;
        }

        private static P<T> Return<T>(T x)
        {
            return input => new Consumed<T>(false, new ParseResult<T>(x, input, new ErrorInfo(input.Position)));
        }

        private static P<Func<T, T, T>> Return<T>(Func<T, T, T> f)
        {
            return Return<Func<T, T, T>>(f);
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

        private static P<Func<Expression, Expression, Expression>> BinOp(string symbol, Func<Expression, Expression, Expression> func)
        {
            return Literal(TokenBuilder.Symbol(symbol)).Then_(Return(func));
        }
       
        public static Expression Parse(IEnumerable<Token> input)
        {
            var addOp = BinOp("+", (x, y) => x + y).Or(BinOp("-", (x, y) => x - y)).Tag("add/subtract op");

            var mulOp = BinOp("*", (x, y) => x * y).Or(BinOp("/", (x, y) => x / y)).Tag("multiply/divide op");

            var expOp = BinOp("^", (x, y) => x ^ y).Tag("exponentiation op");

            var factor = DigitVal.Chainr1(expOp);
            var term = factor.Chainl1(mulOp);
            var expr = term.Chainl1(addOp);

            return expr.Parse(input).Result;
        }

       
    }
}