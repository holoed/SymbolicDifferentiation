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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.FSharp.Core;
using Function =
    System.Func
        <
            System.Collections.Generic.IEnumerable
                <System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, double>>>,
            System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, double>>>;
using FastFunction =
    Microsoft.FSharp.Core.FastFunc
        <
            System.Collections.Generic.IEnumerable
                <System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, double>>>,
            System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, double>>>;

namespace SymbolicDifferentiation.Core.Computation
{
    public class ComputationResult : IEnumerable<KeyValuePair<string, double>>
    {
        private ComputationResult(string name, IEnumerable<string> args, FastFunc<IDictionary<string, FastFunction>, IEnumerable<KeyValuePair<string, double>>> body)
        {
            Name = name;
            Args = args;
            Body = body;
        }

        private FastFunc<IDictionary<string, FastFunction>, IEnumerable<KeyValuePair<string, double>>> Body { get; set; }

        private IEnumerable<string> Args { get; set; }
      
        public string Name { get; private set; }

        public IEnumerator<KeyValuePair<string, double>> GetEnumerator()
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static IEnumerable<KeyValuePair<string, double>> CreateFunction(string name, IEnumerable<string> args, FastFunc<IDictionary<string, FastFunction>, IEnumerable<KeyValuePair<string, double>>> body)
        {
            return new ComputationResult(name, args, body);
        }

        public static IDictionary<string, Function> CreateDictionary(
            IEnumerable<IEnumerable<KeyValuePair<string, double>>> data)
        {
            var result = data.ToArray();
            return result.All(item => item is ComputationResult)
                       ?
                           result.Cast<ComputationResult>().ToDictionary(item => item.Name, item => ComputationResultToFunction(item))
                       :
                           result.ToDictionary(item => "", item => EnumerableToFunction(item));
        }

        public static IDictionary<string, FastFunction> MergeDictionaries(
            IDictionary<string, FastFunction> a,
            IDictionary<string, FastFunction> b)
        {
            return b.Select(item => item).
                Concat(a.Select(item => item)).
                Distinct(new KeyComparer()).
                ToDictionary(item => item.Key, item => item.Value);
        }

        public static IDictionary<string, FastFunction> ToFastFunc(
            IDictionary<string, Function> funcs)
        {
            return funcs.Select(
                item =>
                new KeyValuePair<string, FastFunction>(item.Key,
                                                       FuncConvert.ToFastFunc(
                                                           (Converter
                                                               <IEnumerable<IEnumerable<KeyValuePair<string, double>>>,
                                                               IEnumerable<KeyValuePair<string, double>>>)
                                                           (arg => item.Value(arg))))).ToDictionary(item => item.Key,
                                                                                                    item => item.Value);
        }

        private static Function EnumerableToFunction(IEnumerable<KeyValuePair<string, double>> item)
        {
            return input => item;
        }

        private static Function ComputationResultToFunction(ComputationResult item)
        {
            return input => item.Body.Invoke(ToFastFunc(CreateInputDictionary(input, item.Args)));
        }

        private static IDictionary<string, Function> CreateInputDictionary(IEnumerable<IEnumerable<KeyValuePair<string, double>>> input, IEnumerable<string> args)
        {
            var argsEnumerator = args.GetEnumerator();
            var inputEnumerator = input.GetEnumerator();

            var dic = new Dictionary<string, Function>();

            while (argsEnumerator.MoveNext() && inputEnumerator.MoveNext())
            {
                var current = inputEnumerator.Current;
                dic.Add(argsEnumerator.Current, i => current);
            }
            return dic;
        }
    }

    public class KeyComparer : IEqualityComparer<KeyValuePair<string, FastFunction>>
    {
        public bool Equals(KeyValuePair<string, FastFunction> x, KeyValuePair<string, FastFunction> y)
        {
            return x.Key == y.Key;
        }

        public int GetHashCode(KeyValuePair<string, FastFunction> obj)
        {
            return obj.GetHashCode();
        }
    }
}