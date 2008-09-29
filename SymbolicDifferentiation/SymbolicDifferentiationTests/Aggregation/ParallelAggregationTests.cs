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
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace SymbolicDifferentiation.Tests.Aggregation
{
    public class ParallelAggregationTests : AggregationTests
    {
        protected override double[] Compute(string input)
        {
            return ComputeParallel(input, 3);
        }

        [Test]
        [Ignore("long running performance test")]
        public void AddLots()
        {
            var expression = "(((A + B) * (A + B)) * ((A + B) * (A + B))) + (((A + B) * (A + B)) * ((A + B) * (A + B)))";

            const int _size = 1000000;

            _data = new Dictionary<string, IEnumerable<double>>
                        {
                            {"A", Enumerable.Range(0, _size).Select(i => i + .0)},
                            {"B", Enumerable.Range(_size, _size).Select(i => i + .0)},
                        };
             

            var watch = new Stopwatch();
            watch.Reset();
            Console.WriteLine("Start sequential...");
            watch.Start();
            var result2 = ComputeSequential(expression, _size);
            watch.Stop();
            Console.WriteLine("Sequential elapsed:{0}", watch.Elapsed);
            watch.Reset();
            Console.WriteLine("Start parallel...");
            watch.Start();
            var result = ComputeParallel(expression, _size);
            watch.Stop();
            Console.WriteLine("Parallel elapsed:{0}", watch.Elapsed);

            CollectionAssert.AreEqual(result, result2);
        }
    }
}
