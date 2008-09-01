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
using NUnit.Framework;
using SymbolicDifferentiation.Extensions;
using SymbolicDifferentiation.Tokens;

namespace SymbolicDifferentiation.Tests
{
    [TestFixture]
    public class TokenizeTests
    {
        private static void AssertToken(object value, MatchType type, Token token)
        {
            Assert.AreEqual(type, token.Type);
            Assert.AreEqual(value, token.Value);
        }

        [Test]
        public void MultiplicationAndSquare()
        {
            IEnumerable<Token> tokens = "3x^2".Tokenize();
            AssertToken(3, MatchType.Number, tokens.ElementAt(0));
            AssertToken("x", MatchType.Variable, tokens.ElementAt(1));
            AssertToken("^", MatchType.Symbol, tokens.ElementAt(2));
            AssertToken(2, MatchType.Number, tokens.ElementAt(3));
        }

        [Test]
        public void Number()
        {
            IEnumerable<Token> tokens = "42".Tokenize();
            AssertToken(42, MatchType.Number, tokens.First());
        }

        [Test]
        public void NumberAndVariable()
        {
            IEnumerable<Token> tokens = "2x".Tokenize();
            AssertToken(2, MatchType.Number, tokens.First());
            AssertToken("x", MatchType.Variable, tokens.ElementAt(1));
        }

        [Test]
        public void Polynomial()
        {
            IEnumerable<Token> tokens = "x^2 + 3x + 1".Tokenize();
            AssertToken("x", MatchType.Variable, tokens.ElementAt(0));
            AssertToken("^", MatchType.Symbol, tokens.ElementAt(1));
            AssertToken(2, MatchType.Number, tokens.ElementAt(2));
            AssertToken("+", MatchType.Symbol, tokens.ElementAt(3));
            AssertToken(3, MatchType.Number, tokens.ElementAt(4));
            AssertToken("x", MatchType.Variable, tokens.ElementAt(5));
            AssertToken("+", MatchType.Symbol, tokens.ElementAt(6));
            AssertToken(1, MatchType.Number, tokens.ElementAt(7));
        }

        [Test]
        public void Square()
        {
            IEnumerable<Token> tokens = "x^2".Tokenize();
            AssertToken("x", MatchType.Variable, tokens.ElementAt(0));
            AssertToken("^", MatchType.Symbol, tokens.ElementAt(1));
            AssertToken(2, MatchType.Number, tokens.ElementAt(2));
        }

        [Test]
        public void Symbol()
        {
            AssertToken("^", MatchType.Symbol, "^".Tokenize().First());
            AssertToken("*", MatchType.Symbol, "*".Tokenize().First());
            AssertToken("+", MatchType.Symbol, "+".Tokenize().First());
            AssertToken("(", MatchType.Symbol, "(".Tokenize().First());
            AssertToken(")", MatchType.Symbol, ")".Tokenize().First());
        }

        [Test]
        public void Variable()
        {
            IEnumerable<Token> tokens = "x".Tokenize();
            AssertToken("x", MatchType.Variable, tokens.First());
        }
    }
}