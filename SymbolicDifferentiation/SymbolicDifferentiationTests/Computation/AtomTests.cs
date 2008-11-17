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
using SymbolicDifferentiation.Core.Computation;

namespace SymbolicDifferentiation.Tests.Computation
{
    [TestFixture]
    public class AtomTests
    {
        [Test]
        public void DoubleFromToAtomImplicitConversion()
        {
            Atom atom = 42.2;
            double value = atom;
            Assert.AreEqual(42.2, value);
        }

        [Test]
        public void BoolFromToAtomImplicitConversion()
        {
            Atom atom = true;
            bool value = atom;
            Assert.AreEqual(true, value);
        }

        [Test]
        public void AddAtomDoubles()
        {
            Atom left = 2;
            Atom right = 5;
            double result = left + right;
            Assert.AreEqual(7, result);
        }

        [Test]
        public void SubAtomDoubles()
        {
            Atom left = 2;
            Atom right = 5;
            double result = left - right;
            Assert.AreEqual(-3, result);
        }

        [Test]
        public void MulAtomDoubles()
        {
            Atom left = 2;
            Atom right = 5;
            double result = left * right;
            Assert.AreEqual(10, result);
        }

        [Test]
        public void DivAtomDoubles()
        {
            Atom left = 2.0;
            Atom right = 5.0;
            double result = left / right;
            Assert.AreEqual(2d/5d, result);
        }

        [Test]
        public void PowAtomDoubles()
        {
            Atom left = 2.0;
            Atom right = 5.0;
            double result = left ^ right;
            Assert.AreEqual(Math.Pow(2d,5d), result);
        }

        [Test]
        public void GreaterThan()
        {
            Atom left = 2.0;
            Atom right = 5.0;
            bool result = left > right;
            Assert.AreEqual(false, result);
        }

        [Test]
        public void LessThan()
        {
            Atom left = 2.0;
            Atom right = 5.0;
            bool result = left < right;
            Assert.AreEqual(true, result);
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException), ExpectedMessage = "Double and Bool types are not compatible")]
        public void TypeConsistencyCheck()
        {
            Atom left = 2.0;
            Atom right = true;
            var result = left + right;
        }

        [Test]
        public void AtomBoolEqual()
        {
            Atom left = true;
            Assert.IsTrue(left.Equals(true));
        }

        [Test]
        public void AtomDoubleEqual()
        {
            Atom left = 3.5;
            Assert.IsTrue(left.Equals(3.5));
        }
    }
}
