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

using System.Linq;
using NUnit.Framework;
using SymbolicDifferentiation.Extensions;
using SymbolicDifferentiation.Tokens;

namespace SymbolicDifferentiation.Tests
{
    [TestFixture]
    public class SugarTests
    {
        [Test]
        public void DesugarMultiplications()
        {
            var input = new[]
                            {
                                TokenBuilder.Number(3), 
                                TokenBuilder.Variable("x")
                            };
            
            var expected = new[]
                               {
                                   TokenBuilder.Number(3), 
                                   TokenBuilder.Symbol("*"), 
                                   TokenBuilder.Variable("x")
                               };
            CollectionAssert.AreEqual(expected, input.Expand().ToArray());
        }

        [Test]
        public void SugarMultiplications()
        {
            var input = new[]
                            {
                                TokenBuilder.Number(3), 
                                TokenBuilder.Symbol("*"), 
                                TokenBuilder.Variable("x")
                            };

            var expected = new[]
                               {
                                   TokenBuilder.Number(3), 
                                   TokenBuilder.Variable("x")
                               };
            CollectionAssert.AreEqual(expected, input.Shrink().ToArray());
        }
    }
}
