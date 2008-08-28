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
using SymbolicDifferentiation.ParserCombinators;

namespace SymbolicDifferentiation.ParserCombinators
{
    public class ParseResult<T>
    {
        public readonly ErrorInfo ErrorInfo;
        private readonly ParserState _remainingInput;
        private readonly T _result;
        public readonly bool Succeeded;

        public ParseResult(T result, ParserState remainingInput, ErrorInfo errorInfo)
        {
            _result = result;
            _remainingInput = remainingInput;
            ErrorInfo = errorInfo;
            Succeeded = true;
        }

        public ParseResult(ErrorInfo errorInfo)
        {
            ErrorInfo = errorInfo;
        }

        public T Result
        {
            get
            {
                if (!Succeeded) throw new InvalidOperationException();
                return _result;
            }
        }

        public ParserState RemainingInput
        {
            get
            {
                if (!Succeeded) throw new InvalidOperationException();
                return _remainingInput;
            }
        }

        public ParseResult<T> MergeError(ErrorInfo otherError)
        {
            return Succeeded ? 
                new ParseResult<T>(Result, RemainingInput, ErrorInfo.Merge(otherError)) : 
                new ParseResult<T>(ErrorInfo.Merge(otherError));
        }

        public ParseResult<T> SetExpectation(string label)
        {
            return Succeeded ? 
                new ParseResult<T>(Result, RemainingInput, ErrorInfo.SetExpectation(label)) : 
                new ParseResult<T>(ErrorInfo.SetExpectation(label));
        }

        public override bool Equals(object obj)
        {
            var other = obj as ParseResult<T>;
            return other != null && 
                (Equals(other.Result, Result) &&
                (Equals(other.RemainingInput, RemainingInput) &&
                Equals(other.ErrorInfo, ErrorInfo)));
        }

        public override int GetHashCode()
        {
            return Succeeded.GetHashCode();
        }

        public override string ToString()
        {
            return Succeeded ?
                string.Format("Success: {0}", Result) : 
                string.Format("Failure: {0}", ErrorInfo);
        }
    }
}