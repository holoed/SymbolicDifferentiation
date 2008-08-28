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

using NUnit.Framework;
using SymbolicDifferentiation.ParserCombinators;
using SymbolicDifferentiation.Tokens;

namespace SymbolicDifferentiation.Tests.ParserCombinators
{
    [TestFixture]
    public class ExpressionParserTests : TestsBase
    {
        [Test]
        public void SimpleAddition()
        {
            var actual =
                ExpressionParser.Parse(new[]                               
                {
                    new Token(MatchType.Keyword, "let"),   //TODO: Remove the need for let                 
                    Token.Whitespace,                    
                    TokenBuilder.Variable("x"),                    
                    TokenBuilder.Symbol("+"),                    
                    TokenBuilder.Number(2),
                    Token.EOF
                });
            ExpressionAssert.AreEqual(Variable("x") + Number(2), actual);

        }
    }
}
