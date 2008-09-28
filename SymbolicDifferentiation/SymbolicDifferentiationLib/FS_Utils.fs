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

let private toString (x:Token) = System.Convert.ToString(x.Value)

let rec private toFsVisitor = 
 { new IExpressionVisitor<FS_AbstractSyntaxTree.Expression<'a>> with 
   member v.Visit(x : FunctionApplicationExpression) : FS_AbstractSyntaxTree.Expression<'a> =
      let name = toString x.Name
      let args = Seq.map(fun (arg : Expression) -> (arg.Accept toFsVisitor)) x.Arguments;
      Fun(name, args)
   member v.Visit(x : BinaryExpression ) =  
      let left = x.Left.Accept toFsVisitor
      let right = x.Right.Accept toFsVisitor
      match x.Operator with
      | IsOp("+") result -> left + right
      | IsOp("*") result -> left * right
      | IsOp("^") result -> Pow (left, x.Right.Value.GetValue<'a>())
      | _ -> failwith "unknown operator"
   member v.Visit(x : Expression) = 
      if (x.IsNumber) then
        Number (x.Value.GetValue<'a>())
      else
        Variable (toString x.Value)  }
        
let rec ToFs (x : Expression) =
    x.Accept toFsVisitor    
        
let rec ToCs (x : FS_AbstractSyntaxTree.Expression<'a>) =  
        match x with
        | Variable v -> new Expression(TokenBuilder.Variable(v))
        | Number n -> new Expression(TokenBuilder.Number(n))
        | Add(x,y) -> ToCs(x) + ToCs(y)
        | Mul(x,y) -> ToCs(x) * ToCs(y)
        | Pow(x,y) -> Expression.op_ExclusiveOr(ToCs(x) , new Expression(TokenBuilder.Number(y)))
        | Fun(name, args) -> 
            FunctionApplicationExpression.Create(TokenBuilder.Variable(name), Seq.map ToCs args |> Array.of_seq)




