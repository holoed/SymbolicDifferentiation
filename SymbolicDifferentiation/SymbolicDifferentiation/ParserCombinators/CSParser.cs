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

using System.Collections.Generic;
using System.Linq;
using SymbolicDifferentiation.Core.AST;
using SymbolicDifferentiation.Core.Tokens;

namespace SymbolicDifferentiation.ParserCombinators
{
    public static class CSParser
    {
        public static IEnumerable<Expression> Parse(IEnumerable<Token> input)
        {
            var addOp = (new Symbol("+") > ((x, y) => x + y)).Or(new Symbol("-") > ((x, y) => x - y)).Tag("add/subtract op");
            var mulOp = (new Symbol("*") > ((x, y) => x * y)).Or(new Symbol("/") > ((x, y) => x / y)).Tag("multiply/divide op");
            var cmpOp = (new Symbol(">") > ((x, y) => x > y)).Or(new Symbol("<") > ((x, y) => x < y)).Tag("greater than/less than op");
            var expOp = (new Symbol("^") > ((x, y) => x ^ y)).Tag("exponentiation op");

            P<Expression> paren, p1, p2, p3, p4,p5, expr = null, app, cond, decl, declWithArgs;

            paren = from o in new Symbol("(").Literal()
                    from e in expr
                    from c in new Symbol(")").Literal()
                    select e;

            // Function application
            app =
                from name in
                    CSParserLib.Sat(x => x.Type.Equals(MatchType.Variable)).FollowedBy(
                    c => CSParserLib.Sat(t => (t.Value.Equals("("))))
                from o in new Symbol("(").Literal()
                from e in expr.SepBy(new Symbol(",").Literal())
                from c in new Symbol(")").Literal()
                select FunctionApplicationExpression.Create(name, e.ToArray());

            // Conditional expression
            cond =
                from o in new Symbol("(").Literal()
                from condition in expr
                from c in new Symbol(")").Literal()
                from e in new Symbol("?").Literal()
                from success in expr
                from a in new Symbol(":").Literal()
                from failure in expr
                select ConditionalExpression.Create(condition, success, failure);

            // Function application
            decl =
                from name in
                    CSParserLib.Sat(x => x.Type.Equals(MatchType.Variable)).FollowedBy(
                    c => CSParserLib.Sat(t => (t.Value.Equals("="))))
                from o in new Symbol("=").Literal()
                from e in expr
                select FunctionDeclarationExpression.Create(name, e);

            // Function application
            declWithArgs =
                from name in
                    CSParserLib.Sat(x => x.Type.Equals(MatchType.Variable)).FollowedBy(
                    c => CSParserLib.Sat(t => (t.Value.Equals("("))))
                from o in new Symbol("(").Literal()
                from args in expr.SepBy(new Symbol(",").Literal())
                from c in new Symbol(")").Literal()
                from e in new Symbol("=").Literal()
                from body in expr
                select FunctionDeclarationExpression.CreateWithArgs(name, args, body);

            p5 = declWithArgs.Attempt().Or(decl).Or(cond.Attempt()).Or(app).Or(CSParserLib.DigitVal).Or(paren);
            p4 =   p5.Chainr1(expOp);
            p3 =   p4.Chainl1(mulOp);
            p2 =   p3.Chainl1(addOp);
            p1 =   p2.Chainl1(cmpOp);
            expr = p1;
            var exprs = from e in expr.SepBy(new Token(MatchType.EOL, "\n").Literal())
                        select e;

            return exprs.Parse(input).Result;
        }
    }   
}