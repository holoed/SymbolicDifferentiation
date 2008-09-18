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
open FS_Utils;

let rec private Create (exp:Expression, a: double seq, b: double seq):double seq =
    match exp with
    | Number n -> seq[n]
    | Variable x -> match x with
                        |"A" -> a
                        |"B" -> b
                        | _ -> failwith "unrecognized variable"
    | Add(x, y) -> Seq.map2(fun left right -> left + right) (Create(x, a, b)) (Create(y, a, b))
    | Mul(x, y) -> Seq.map2(fun left right -> left * right) (Create(x, a, b)) (Create(y, a, b))
    | Pow(x, n) -> Seq.map2(fun left right -> System.Math.Pow(left, right)) (Create(x, a, b)) (seq[n])
    
type Ret(exp:Expression) =
    member x.Execute(a: double seq,b: double seq) = Create(exp, a, b)

let Build = fun exp -> Ret(ToFs exp)