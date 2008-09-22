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
using NUnit.Framework;
using SymbolicDifferentiation.Core.AST;
using SymbolicDifferentiation.Visitors;

namespace SymbolicDifferentiation.Tests
{
    public class ExpressionAssert
    {
        public static void AreEqual(Expression expected, Expression actual)
        {
            Assert.IsInstanceOfType(expected.GetType(), actual);
            Assert.IsTrue(ExpressionEqualityComparer.AreEqual(expected, actual),
                          String.Format("\nExpected {0}\nbut was  {1}", expected, actual));
        }
    }
}