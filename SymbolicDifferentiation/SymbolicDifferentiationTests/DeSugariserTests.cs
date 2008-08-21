using System.Linq;
using NUnit.Framework;
using SymbolicDifferentiation.Extensions;
using SymbolicDifferentiation.Tokens;

namespace SymbolicDifferentiation.Tests
{
    [TestFixture]
    public class DeSugariserTests
    {
        [Test]
        public void Test()
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
            CollectionAssert.AreEqual(expected, input.DeSugar().ToArray());
        }
    }
}
