using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SymbolicDifferentiation.Tokens;

namespace SymbolicDifferentiation
{
    public static class DeSugariser
    {
        public static IEnumerable<Token> This(IEnumerable<Token> tokens)
        {
            Func<IEnumerable<Token>, IEnumerable<Token>, IEnumerable<Token>> ds = null;
            ds = (i, o) => i.Count() > 1 ? 
                i.First().Type == MatchType.Number && i.Skip(1).First().Type == MatchType.Variable ?
                ds(i.Skip(1), o.Concat(i.Take(1)).Concat(new[]{TokenBuilder.Symbol("*")})) :
                ds(i.Skip(1), o.Concat(i.Take(1))) : o.Concat(i);

            var desugar = ds.Curry()(new Token[0]);

            return desugar(tokens);
        }

        private static Func<V, Func<T,K>> Curry<T,K,V>(this Func<T,V,K> func)
        {
            return a => b => func(b, a);
        }
    }
}
