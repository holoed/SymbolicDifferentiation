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
using SymbolicDifferentiation.Core.Tokens;

namespace SymbolicDifferentiation
{
    // Just for fun :)
    public static class Sugar
    {
        public static IEnumerable<Token> Expand(IEnumerable<Token> tokens)
        {
            Func<IEnumerable<Token>, IEnumerable<Token>, IEnumerable<Token>> ds = null;
            ds = (i, o) => i.Count() > 1 ? IsMultiplication(i) ?
                ds(i.Skip(1), o.ConcatWithNextToken(i).ConcatMultiplication()) :
                ds(i.Skip(1), o.ConcatWithNextToken(i)) : o.ConcatWithNextToken(i);
            var desugar = ds.Curry()(new Token[0]);

            return desugar(tokens);
        }

        private static bool IsMultiplication(IEnumerable<Token> list)
        {
            return 
                list.First().Type == MatchType.Number && 
                list.Skip(1).First().Type == MatchType.Variable;
        }

        private static IEnumerable<Token> ConcatWithNextToken(this IEnumerable<Token> output, IEnumerable<Token> input)
        {
            return output.Concat(input.Take(1));
        }

        private static IEnumerable<Token> ConcatMultiplication(this IEnumerable<Token> input)
        {
            return input.Concat(new[] { TokenBuilder.Symbol("*") });
        }

        private static Func<V, Func<T,K>> Curry<T,K,V>(this Func<T,V,K> func)
        {
            return b => a => func(a, b);
        }
    }
}
