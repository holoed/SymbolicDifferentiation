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

namespace SymbolicDifferentiation.Core.AST
{
    public class ConditionalExpression : Expression
    {
        public Expression Condition { private set; get; }
        public Expression Success { private set; get; }
        public Expression Failure { private set; get; }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return String.Format("({0}) ? {1} : {2}", Condition, Success, Failure);
        }

        public static Expression Create(Expression condition, Expression success, Expression failure)
        {
            return new ConditionalExpression{ Condition = condition, Success = success, Failure = failure };
        }
    }
}
