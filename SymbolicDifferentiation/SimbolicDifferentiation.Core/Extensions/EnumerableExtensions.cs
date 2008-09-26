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

namespace SymbolicDifferentiation.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Combine<T>(this IEnumerable<IEnumerable<T>> data, Func<IEnumerable<T>, T> func)
        {
            var enumerators = new SequenceOfSequences<T>(data.Select(enumerator => enumerator.GetEnumerator()).ToArray());

            while (enumerators.MoveNext())
                yield return func(enumerators.Current);
        }

        private class SequenceOfSequences<T>
        {
            private readonly IEnumerable<IEnumerator<T>> _enumerators;
            private readonly HashSet<IEnumerator<T>> _finished;
            private readonly Dictionary<IEnumerator<T>, T> _last;

            public SequenceOfSequences(IEnumerable<IEnumerator<T>> enumerators)
            {
                _last = new Dictionary<IEnumerator<T>, T>();
                _finished = new HashSet<IEnumerator<T>>();
                _enumerators = enumerators;
            }

            public bool MoveNext()
            {
                foreach (var enumerator in _enumerators.Except(_finished))
                    if (!enumerator.MoveNext())
                        _finished.Add(enumerator);
                return _enumerators.Any(enumerator => !_finished.Contains(enumerator));
            }

            public IEnumerable<T> Current
            {
                get
                {
                    foreach (var enumerator in _enumerators)
                        if (!_finished.Contains(enumerator))
                            yield return _last[enumerator] = enumerator.Current;
                        else
                            yield return _last[enumerator];
                }
            }
        }
    }
}