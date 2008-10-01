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
using NUnit.Framework;
using SymbolicDifferentiation.Core.Tokens;

namespace SymbolicDifferentiation.Tests.LexicalAnalysis
{
    [TestFixture]
    public abstract class TokenizerTests
    {
        protected abstract IEnumerable<Token> Tokenize(string input);

        private static void AssertToken(object value, MatchType type, Token token)
        {
            Assert.AreEqual(type, token.Type);
            Assert.AreEqual(value, token.Value);
        }

        [Test]
        public void MultiplicationAndSquare()
        {
            var tokens = Tokenize("3x^2");
            AssertToken(3, MatchType.Number, tokens.ElementAt(0));
            AssertToken("x", MatchType.Variable, tokens.ElementAt(1));
            AssertToken("^", MatchType.Symbol, tokens.ElementAt(2));
            AssertToken(2, MatchType.Number, tokens.ElementAt(3));
        }

        [Test]
        public void Number()
        {
            var tokens = Tokenize("42");
            AssertToken(42, MatchType.Number, tokens.First());
        }

        [Test]
        public void NumberAndVariable()
        {
            var tokens = Tokenize("2x");
            AssertToken(2, MatchType.Number, tokens.First());
            AssertToken("x", MatchType.Variable, tokens.ElementAt(1));
        }

        [Test]
        public void Polynomial()
        {
            var tokens = Tokenize("x^2 + 3x + 1");
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
            var tokens = Tokenize("x^2");
            AssertToken("x", MatchType.Variable, tokens.ElementAt(0));
            AssertToken("^", MatchType.Symbol, tokens.ElementAt(1));
            AssertToken(2, MatchType.Number, tokens.ElementAt(2));
        }

        [Test]
        public void Symbol()
        {
            AssertToken("^", MatchType.Symbol, Tokenize("^").First());
            AssertToken("*", MatchType.Symbol, Tokenize("*").First());
            AssertToken("+", MatchType.Symbol, Tokenize("+").First());
            AssertToken("(", MatchType.Symbol, Tokenize("(").First());
            AssertToken(")", MatchType.Symbol, Tokenize(")").First());
            AssertToken(",", MatchType.Symbol, Tokenize(",").First());
            AssertToken("=", MatchType.Symbol, Tokenize("=").First());
        }

        [Test]
        public void Variable()
        {
            var tokens = Tokenize("x");
            AssertToken("x", MatchType.Variable, tokens.First());
        }

        [Test]
        public void CarriageReturn()
        {
            var tokens = Tokenize("x" + Environment.NewLine + " y");
            var list = tokens.ToList();
            Assert.AreEqual(3, list.Count);
            AssertToken("x", MatchType.Variable, list[0]);
            AssertToken("\n", MatchType.EOL, list[1]);
            AssertToken("y", MatchType.Variable, list[2]);
        }
    }
}