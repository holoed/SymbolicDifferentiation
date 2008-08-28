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

namespace SymbolicDifferentiation.ParserCombinators
{
    public class Consumed<T>
    {
        public readonly bool HasConsumedInput;
        public readonly ParseResult<T> ParseResult;

        public Consumed(bool hasConsumedInput, ParseResult<T> result)
        {
            HasConsumedInput = hasConsumedInput;
            ParseResult = result;
        }

        public Consumed<T> Tag(string label)
        {
            return HasConsumedInput ? 
                this : 
                new Consumed<T>(HasConsumedInput, ParseResult.SetExpectation(label));
        }

        public override string ToString()
        {
            return ParseResult.ToString();
        }
    }
}