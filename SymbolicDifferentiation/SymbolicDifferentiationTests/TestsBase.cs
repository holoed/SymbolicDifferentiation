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
using SymbolicDifferentiation.Core.AST;
using SymbolicDifferentiation.Core.Tokens;

namespace SymbolicDifferentiation.Tests
{
    public abstract class TestsBase
    {
        protected static Expression Number(double value)
        {
            return new Expression {Value = TokenBuilder.Number(value)};
        }

        protected static Expression Variable(string value)
        {
            return new Expression {Value = TokenBuilder.Variable(value)};
        }

        protected static Expression FunctionApp(string name, params Expression[] args)
        {
            return new FunctionApplicationExpression {Name = TokenBuilder.Variable(name), Arguments = args};
        }

        protected static Expression FunctionDecl(string name, Expression body)
        {
            return FunctionDeclarationExpression.Create(TokenBuilder.Variable(name), body);
        }

        protected static Expression FunctionDecl(string name, IEnumerable<Expression> args, Expression body)
        {
            return FunctionDeclarationExpression.CreateWithArgs(TokenBuilder.Variable(name), args, body);
        }
    }
}