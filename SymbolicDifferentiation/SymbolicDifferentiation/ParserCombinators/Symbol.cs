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
using SymbolicDifferentiation.Core.AST;
using SymbolicDifferentiation.Core.Tokens;

namespace SymbolicDifferentiation.ParserCombinators
{
    public class Symbol : Token
    {
        public Symbol(string value)
            : base(MatchType.Symbol, value)
        { }

        public static P<Func<Expression, Expression, Expression>> operator >(Symbol token, Func<Expression, Expression, Expression> func)
        {
            return from t in token.Literal() select func;
        }

        public static P<Func<Expression, Expression, Expression>> operator <(Symbol token, Func<Expression, Expression, Expression> func)
        {
            throw new NotImplementedException();
        }
    } 
}
