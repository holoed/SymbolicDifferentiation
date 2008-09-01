using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SymbolicDifferentiation.ParserCombinators;
using SymbolicDifferentiation.Tokens;

namespace SymbolicDifferentiation.Tests.ParserCombinators
{
    [TestFixture]
    public class CombinatorParserExtensionsTests
    {
        [Test]
        public void Cons()
        {
            CollectionAssert.AreEqual(new[] { 0 }, CombinatorParserExtensions.Cons(0, new int[0]).ToArray());
            CollectionAssert.AreEqual(new[] {0, 1, 2, 3}, CombinatorParserExtensions.Cons(0, new[] {1, 2, 3}).ToArray());
        }

        [Test]
        public void ASuccessfullParseAndThenAnotherParse()
        {
            Func<IEnumerable<Token>,string> a = tokens => tokens.First().Equals(TokenBuilder.Variable("H"))  ? "Hello" : "";
            Func<IEnumerable<Token>, string> b = tokens => tokens.First().Equals(TokenBuilder.Variable("W")) ? "World" : "";

            P<string> c = CombinatorParserExtensions.Then<string, string>((
                state => new Consumed<string>(true, new ParseResult<string>(a(state.Input), new ParserState(0, state.Input.Skip(1)), new ErrorInfo(0)))),
                prevResult => (state => new Consumed<string>(true, new ParseResult<string>(prevResult + b(state.Input), new ParserState(1, state.Input), new ErrorInfo(0)))));

            var result = c(new ParserState(0, new[] {TokenBuilder.Variable("H"), TokenBuilder.Variable("W")}));

            Assert.AreEqual(true, result.HasConsumedInput);
            Assert.AreEqual("HelloWorld", result.ParseResult.Result);
        }


        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FirstParseFails()
        {
            P<string> c = CombinatorParserExtensions.Then<string, string>((
                state => new Consumed<string>(false, new ParseResult<string>(new ErrorInfo(0)))),
                prevResult => (state => new Consumed<string>(true, new ParseResult<string>(prevResult, new ParserState(1, state.Input), new ErrorInfo(0)))));

            var result = c(new ParserState(0, new[] { TokenBuilder.Variable("T"), TokenBuilder.Variable("W") }));

            Assert.AreEqual(false, result.HasConsumedInput);
            Assert.AreEqual("HelloWorld", result.ParseResult.Result);
        }
    }
}
