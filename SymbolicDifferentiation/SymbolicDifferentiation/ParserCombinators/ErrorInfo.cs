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
using System.Linq;
using System.Text;
using SymbolicDifferentiation.ParserCombinators;

namespace SymbolicDifferentiation.ParserCombinators
{
    public class ErrorInfo
    {
        public readonly IEnumerable<string> Expectations;
        public readonly string Message;
        public readonly int Position;

        public ErrorInfo(int position) : this(position, Enumerable.Empty<string>(), "unknown error")
        {
        }

        public ErrorInfo(int position, IEnumerable<string> expectations, string message)
        {
            Position = position;
            Expectations = expectations;
            Message = message;
        }

        public ErrorInfo Merge(ErrorInfo other)
        {
            return new ErrorInfo(other.Position, Expectations.Concat(other.Expectations), other.Message);
        }

        public ErrorInfo SetExpectation(string label)
        {
            return string.IsNullOrEmpty(label) ? 
                new ErrorInfo(Position, Enumerable.Empty<string>(), Message) :
                new ErrorInfo(Position, CombinatorParserExtensions.Cons(label, Enumerable.Empty<string>()), Message);
        }

        public override string ToString()
        {
            var expectations = Expectations.ToList();
            var result = new StringBuilder(string.Format("At position {0}, {1}", Position, Message));
            if (expectations.Count != 0)
            {
                result.Append(", ");
                if (expectations.Count == 1)
                {
                    result.Append("expected " + expectations[0]);
                }
                else
                {
                    result.Append("expected ");
                    for (int i = 0; i < expectations.Count - 2; ++i)
                    {
                        result.Append(expectations[i]);
                        result.Append(", ");
                    }
                    result.Append(expectations[expectations.Count - 2]);
                    result.Append(" or ");
                    result.Append(expectations[expectations.Count - 1]);
                }
            }
            return result.ToString();
        }
    }
}