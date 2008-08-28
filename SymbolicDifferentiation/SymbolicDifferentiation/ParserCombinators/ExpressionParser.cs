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
using SymbolicDifferentiation.ParserCombinators.Monad;
using SymbolicDifferentiation.Tokens;

namespace SymbolicDifferentiation.ParserCombinators
{
    public class ExpressionParser : TokenParser<Token[]>
    {
        private readonly Parser<Token[], Expression> All;
        private readonly Parser<Token[], Token[]> Id;
        private readonly Parser<Token[], Token[]> LetId;
        private readonly Parser<Token[], Expression> Term;
        private readonly Parser<Token[], Expression> Term1;
        private readonly Parser<Token[], Token[]> Whitespace;
        private readonly Func<Token, Parser<Token[], Token>> WsChr;

        private ExpressionParser()
        {
            Whitespace = Rep(ParseToken(Token.Whitespace));
            WsChr = chr => Whitespace.AND(ParseToken(chr));
            Id = from w in Whitespace
                 from c in ParseToken(Token.IsLetter)
                 from cs in Rep(ParseToken(Token.IsLetterOrDigit))
                 select cs.Aggregate(new[] {c}, (acc, ch) => acc.Concat(new[] {ch}).ToArray());

            Term1 = (from x in Id
                     select new Expression {Value = x.First()});

            LetId = from s in Id where s.First().Equals(new Token(MatchType.Keyword, "let")) select s;

            Term = (
                       (
                           from letId in LetId
                           from x in Term
                           from op in WsChr(TokenBuilder.Symbol("+"))
                           from y in Term
                           select (Expression) new BinaryExpression
                                                   {
                                                       Operator = TokenBuilder.Symbol("+"),
                                                       Left = x,
                                                       Right = y
                                                   })
                           .OR(from t in Term1 select t));
            All = from t in Term from u in WsChr(Token.EOF) select t;
        }

        protected override Parser<Token[], Token> AnyToken
        {
            get { { return input => input.Length > 0 ? new Result<Token[], Token>(input[0], input.Skip(1).ToArray()) : null; } }
        }

        public static Expression Parse(IEnumerable<Token> tokens)
        {
            var parser = new ExpressionParser();
            return parser.All(tokens.ToArray()).Value;
        }
    }
}