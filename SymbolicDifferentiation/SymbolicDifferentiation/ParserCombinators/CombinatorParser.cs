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
using SymbolicDifferentiation.AST;
using SymbolicDifferentiation.Tokens;

namespace SymbolicDifferentiation.ParserCombinators
{
    public static class CombinatorParser
    {
        public static Expression Parse(IEnumerable<Token> input)
        {
            var addOp = ("+".Op((x, y) => x + y)).Or("-".Op((x, y) => x - y)).Tag("add/subtract op");
            var mulOp = ("*".Op((x, y) => x * y)).Or("/".Op((x, y) => x / y)).Tag("multiply/divide op");
            var expOp = ("^".Op((x, y) => x ^ y)).Tag("exponentiation op");

            var expr = CombinatorParserExtensions.DigitVal.Chainr1(expOp).Chainl1(mulOp).Chainl1(addOp);

            return expr.Parse(input).Result;
        }
    }
}