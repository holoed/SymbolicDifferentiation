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
open SymbolicDifferentiation.Core.AST;
open SymbolicDifferentiation.Core.Tokens;

let private (|IsOp|_|) expected actual = 
    if (TokenBuilder.Symbol(expected) = actual) then
        Some actual 
    else
        None

let private toDouble (x:Token) = System.Convert.ToDouble(x.Value)

let private toString (x:Token) = System.Convert.ToString(x.Value)

let rec private toFsVisitor = 
 { new IExpressionVisitor<FS_AbstractSyntaxTree.Expression> with 
   member v.Visit(x : FunctionApplicationExpression) : FS_AbstractSyntaxTree.Expression =
      failwith "Not implemented yet"
   member v.Visit(x : BinaryExpression ) =  
      let left = x.Left.Accept toFsVisitor
      let right = x.Right.Accept toFsVisitor
      match x.Operator with
      | IsOp("+") result -> left + right
      | IsOp("*") result -> left * right
      | IsOp("^") result -> Pow (left, toDouble x.Right.Value)
      | _ -> failwith "unknown operator"
   member v.Visit(x : Expression) = 
      if (x.IsNumber) then
        Number (toDouble x.Value)
      else
        Variable (toString x.Value)  }
        
let rec ToFs (x : Expression) =
    x.Accept toFsVisitor    
        
let rec ToCs (x : FS_AbstractSyntaxTree.Expression) =  
        match x with
        | Variable v -> new Expression(new Token(MatchType.Variable, v))
        | Number n -> new Expression(new Token(MatchType.Number, n))
        | Add(x,y) -> ToCs(x) + ToCs(y)
        | Mul(x,y) -> ToCs(x) * ToCs(y)
        | Pow(x,y) -> Expression.op_ExclusiveOr(ToCs(x) , new Expression(new Token(MatchType.Number, y)))




