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

namespace SymbolicDifferentiation.ParserCombinators.Monad
{
    public class Result<TInput, TValue>
    {
        public readonly TValue Value;
        public readonly TInput Rest;
        public Result(TValue value, TInput rest) { Value = value; Rest = rest; }
    }

    public delegate Result<TInput, TValue> Parser<TInput, TValue>(TInput input);
}