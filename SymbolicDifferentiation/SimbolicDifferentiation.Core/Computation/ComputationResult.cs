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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SymbolicDifferentiation.Core.Computation
{
    public class ComputationResult : IEnumerable<KeyValuePair<string, double>>
    {
        private readonly IEnumerable<KeyValuePair<string, double>> _result;

        private ComputationResult(string name, IEnumerable<KeyValuePair<string, double>> result)
        {
            Name = name;
            _result = result;
        }

        public static IEnumerable<KeyValuePair<string, double>> CreateFunctionResult(string name, IEnumerable<KeyValuePair<string, double>> result)
        {
            return new ComputationResult(name, result);
        }

        public static IDictionary<string, IEnumerable<KeyValuePair<string, double>>> CreateDictionary(IEnumerable<IEnumerable<KeyValuePair<string, double>>> data)
        {
            var result = data.ToArray();
            return result.All(item => item is ComputationResult) ? 
                result.Cast<ComputationResult>().ToDictionary(item => item.Name, item => item._result) : 
                result.ToDictionary(item => "", item => item);
        }

        public string Name { get; private set; }

        public IEnumerator<KeyValuePair<string, double>> GetEnumerator()
        {
            return _result.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
