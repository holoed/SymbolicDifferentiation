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
using SymbolicDifferentiation.Core.AST;
using SymbolicDifferentiation.Visitors;

namespace SymbolicDifferentiation.Extensions
{
    public static class ExpressionExtension
    {
        public static string CSDerive(this Expression expression)
        {
            return Derivative.Deriv(expression).
                CSSimplify().
                FSToString();
        }

        public static string FSDerive(this Expression expression)
        {
            return FS_Derivative.Derivate(expression).
                FSSimplify().
                FSToString();
        }

        public static string FSToString(this Expression expression)
        {
            return FS_ExpressionToString.ToString(FS_Utils.ToFs(expression));
        }

        public static Func<Dictionary<string, double[]>, double[]> FSAggregateFunction(this Expression expression)
        {
            return FS_Aggregation.Build(expression).Execute;
        }

        public static Func<Dictionary<string, double[]>, double[]> FSParallelAggregateFunction(this Expression expression, int size)
        {
            return FS_Aggregation.BuildParallel(8, size, expression).Execute;
        }
    }
}