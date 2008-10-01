﻿#region License

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
using Microsoft.FSharp.Core;
using SymbolicDifferentiation.Core.AST;
using SymbolicDifferentiation.Visitors;
using System.Linq;

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

        public static string CSDerive(this IEnumerable<Expression> expressions)
        {
            return expressions.Select(expression => expression.CSDerive()).Aggregate((x, y) => x + y);
        }

        public static string FSDerive(this Expression expression)
        {
            return FS_Derivative.Derivate(expression).
                FSSimplify().
                FSToString();
        }

        public static string FSToString(this Expression expression)
        {
            return FS_ExpressionToString.ToString(FS_Utils.ToFs<double>(expression));
        }

        public static string FSToString(this IEnumerable<Expression> expressions)
        {
            return
                expressions.Select(expression => expression.FSToString()).
                Aggregate((x, y) => x + Environment.NewLine + y);
        }

        public static Func<Dictionary<string, IEnumerable<double>>, IEnumerable<double>> FSSequentialComputation(this Expression expression, Dictionary<string, FastFunc<IEnumerable<IEnumerable<double>>, IEnumerable<double>>> funcs)
        {
            return FS_Compute.Build(expression, funcs).Execute;
        }

        public static Func<Dictionary<string, IEnumerable<double>>, IEnumerable<double>> FSParallelComputation(this Expression expression, Dictionary<string, FastFunc<IEnumerable<IEnumerable<double>>, IEnumerable<double>>> funcs)
        {
            return FS_Compute.BuildParallel(expression, funcs).Execute;
        }
    }
}