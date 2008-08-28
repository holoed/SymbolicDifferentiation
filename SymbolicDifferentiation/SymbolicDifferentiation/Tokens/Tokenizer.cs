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
using System.Text.RegularExpressions;

namespace SymbolicDifferentiation.Tokens
{
    public class Tokenizer
    {
        private static readonly Dictionary<MatchType, Func<string, Match>> _patterns = 
            new Dictionary<MatchType, Func<string, Match>>
           {
               { MatchType.Number, input => Regex.Match(input, "^[0-9]+") },
               { MatchType.Variable, input => Regex.Match(input, "^[a-zA-Z]+") },
               { MatchType.Symbol, input => Regex.Match(input, "^[\\^\\+\\*]") },
               { MatchType.Whitespace, input => Regex.Match(input, "^[ ]") }
           };

        public static IEnumerable<Token> Tokenize(string input)
        {
            do
            {
                Token token = NextToken(input);
                input = input.Remove(token.Index, token.Length);
                if (token.Type == MatchType.Whitespace) continue;
                yield return token;
            } while (input.Length > 0);
        }

        private static Token NextToken(string input)
        {
            Match match;
            foreach (var pattern in _patterns)
                if ((match = pattern.Value(input)).Success)
                    return new Token(pattern.Key, GetValue(pattern.Key, match.Value), match.Index, match.Length);
            throw new ArgumentOutOfRangeException("input");
        }

        private static object GetValue(MatchType key, string value)
        {
            switch (key)
            {
                case MatchType.Number:
                    return Double.Parse(value);
                default:
                    return value;
            }
        }
    }
}