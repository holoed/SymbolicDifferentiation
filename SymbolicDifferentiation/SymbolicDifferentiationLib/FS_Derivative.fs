// * **********************************************************************************************
// * Copyright (c) Edmondo Pentangelo. 
// *
// * This source code is subject to terms and conditions of the Microsoft Public License. 
// * A copy of the license can be found in the License.html file at the root of this distribution. 
// * By using this source code in any fashion, you are agreeing to be bound by the terms of the 
// * Microsoft Public License.
// *
// * You must not remove this notice, or any other, from this software.
// * **********************************************************************************************

#light

open FS_AbstractSyntaxTree;
open SymbolicDifferentiation.Core.Visitors;
open SymbolicDifferentiation.Core.AST;
open SymbolicDifferentiation.Core.Tokens;

let private ToFs (x : SymbolicDifferentiation.Core.AST.Expression) =  
        let visitor = new ToDiscriminatedUnion()
        x.Accept(visitor)
        visitor.Result  
        
let rec private ToCs (x : FS_AbstractSyntaxTree.Expression) =  
        match x with
        | Variable v -> new Expression(new Token(MatchType.Variable, v))
        | Number n -> new Expression(new Token(MatchType.Number, n))
        | Add(x,y) -> ToCs(x) + ToCs(y)
        | Mul(x,y) -> ToCs(x) * ToCs(y)
        | Pow(x,y) -> Expression.op_ExclusiveOr(ToCs(x) , new Expression(new Token(MatchType.Number, y)))

let rec private Deriv expression =  
    match expression with
        | Variable _ -> Number 1.0
        | Number _   -> Number 0.0
        | Add(x,y)   -> (Deriv x) + (Deriv y)
        | Mul(x,y)   -> (x * Deriv(y)) + (Deriv(x) * y)
        | Pow(x,y)   -> Number (y) * Pow(x,(y - 1.0))
        
let Derivate x = 
    x |> ToFs |> Deriv |> ToCs
        







