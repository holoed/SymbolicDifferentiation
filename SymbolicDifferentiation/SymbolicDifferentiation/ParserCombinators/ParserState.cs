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
using SimbolicDifferentiation.Core.Tokens;

namespace SymbolicDifferentiation.ParserCombinators
{
    public class ParserState
    {
        public readonly IEnumerable<Token> Input;
        public readonly int Position;

        public ParserState(int position, IEnumerable<Token> input)
        {
            Position = position;
            Input = input;
        }
    }
}