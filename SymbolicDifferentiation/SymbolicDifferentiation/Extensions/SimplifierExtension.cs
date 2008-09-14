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

using SymbolicDifferentiation.Core.AST;
using SymbolicDifferentiation.Visitors;

namespace SymbolicDifferentiation.Extensions
{
    public static class SimplifierExtension
    {
        public static Expression CSSimplify(this Expression expression)
        {
            return Simplifier.Simplify(expression);
        }

        public static Expression FSSimplify(this Expression expression)
        {
            return FS_Simplifier.Simplify(expression);
        }
    }
}