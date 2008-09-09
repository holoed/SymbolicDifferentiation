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
using SymbolicDifferentiation.Core.Tokens;
using SymbolicDifferentiation.Visitors;
using System.Collections.Generic;

namespace SymbolicDifferentiation.Extensions
{
    public static class ExpressionExtension
    {
        public static IEnumerable<Token> ToTokens(this Expression expression, bool grouping)
        {
            var visitor = new ToTokensExpressionVisitor(grouping);
            expression.Accept(visitor);
            return visitor.Result;
        }

        public static string CSDerive(this Expression expression)
        {
            return Derivative.Deriv(expression).
                Simplify().
                ToTokens(false).
                Shrink().
                ToStringExpression();
        }

        public static string FSDerive(this Expression expression)
        {
            return FS_Derivative.Derivate(expression).
                Simplify().
                ToTokens(false).
                Shrink().
                ToStringExpression();
        }
    }
}