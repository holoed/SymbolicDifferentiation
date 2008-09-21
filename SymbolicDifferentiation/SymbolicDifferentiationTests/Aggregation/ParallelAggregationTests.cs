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
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using SymbolicDifferentiation.Extensions;

namespace SymbolicDifferentiation.Tests.Aggregation
{
    public class ParallelAggregationTests : AggregationTests
    {
        protected override double[] Compute(string input)
        {
            return ComputeParallel(input, 3);
        }

        private double[] ComputeParallel(string input, int size)
        {
            return input.FSTokenize().FSParse().FSParallelAggregateFunction(size)(_data).Take(size).ToArray();
        }

        private double[] ComputeSequential(string input, int size)
        {
            return input.FSTokenize().FSParse().FSAggregateFunction()(_data).Take(size).ToArray();
        }


        [Test]
        [Ignore("Long running test...")]
        public void AddLots()
        {
            const int _size = 15000000;

            _data = new Dictionary<string, double[]>
                        {
                            {"A", Enumerable.Range(0, _size).Select(i => i + .0).ToArray()},
                            {"B", Enumerable.Range(_size, _size).Select(i => i + .0).ToArray()},
                        };


            var watch = new Stopwatch();
            watch.Reset();
            Console.WriteLine("Start sequential...");
            watch.Start();
            var result2 = ComputeSequential("A + (A * B)", _size);
            watch.Stop();
            Console.WriteLine("Sequential elapsed:{0}", watch.Elapsed);
            watch.Reset();
            Console.WriteLine("Start parallel...");
            watch.Start();
            var result = ComputeParallel("A + (A * B)", _size);
            watch.Stop();
            Console.WriteLine("Parallel elapsed:{0}", watch.Elapsed);

            CollectionAssert.AreEqual(result, result2);
        }
    }
}
