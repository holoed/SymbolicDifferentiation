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

namespace SymbolicDifferentiation.ParserCombinators
{
    public static class Linq
    {
        // LINQ syntax enablers
        public static P<U> Select<T, U>(this P<T> p, Func<T, U> selector)
        {
            return p.Then(x => selector(x).Return());
        }
        public static P<V> SelectMany<T, U, V>(this P<T> p, Func<T, P<U>> selector, Func<T, U, V> projector)
        {
            return p.Then(r1 => selector(r1).Then(r2 => projector(r1, r2).Return()));
        }

        public static P<T> Where<T>(this P<T> p, Func<T, bool> predicate)
        {
            throw new NotImplementedException(); 
        }
    }
}
