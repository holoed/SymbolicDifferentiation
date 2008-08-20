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

using SymbolicDifferentiation.AST;
using SymbolicDifferentiation.Visitors;

namespace SymbolicDifferentiation.Extensions
{
    public static class ExpressionExtension
    {
        public static string ToString(this Expression expression, bool grouping)
        {
            var visitor = new ToStringExpressionVisitor(grouping);
            expression.Accept(visitor);
            return visitor.Result;
        }
    }
}